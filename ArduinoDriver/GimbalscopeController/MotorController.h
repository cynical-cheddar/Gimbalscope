
#include <Adafruit_MotorShield.h>
#include "QuadratureEncoder.h"

class MotorController_c {

    
  public:

    bool restMode = false;
    bool serialDebug = false;
    #define RPM_SAMPLES 20
    float GEARING     = 100;
    float ENCODERMULT = 28;

    bool doneCommand = false;
    byte precision_degrees = 1;

    float currentDegrees = 0.0f;

    float targetRotation = 0.0f;


    int lastCumPos = 0;

    float baseMotorPower = 70.0f;
    float currentMotorPower = 70.0f;
    float minimumMotorPower = 20.0f;

    unsigned long previousTime = micros();
    unsigned long currentTime = micros();

    // settings for monitoring rotational frequency of motor
    // measuring these in degrees per second
    float RPM_degrees_current = 0.0f;
    float RPM_degrees_target = 0.0f;
    // measured in seconds ( default 0.001 seconds )
    float RPM_polling_duration = 0.001f;
    // sample for current RPM degrees:
    float RPM_current_polls[RPM_SAMPLES];
    int RPM_poll_index = 0;

    bool emergencyStop = false;

    float PI_Gain = 1.0f;
    float PI_Gain_Slow = 0.7f;
    
    bool stationary = false;
    int motorNumber = 0;
    Adafruit_MotorShield AFMS;
    Adafruit_DCMotor *motor;
    // Constructor, must exist.
    MotorController_c() {
        
    } 

    void ForceSetOrientation(float newOrientation){
      currentDegrees = newOrientation;
      SetTargetRotation(0);
    }



    void ReceiveCommand(float targetRot, float targetRPM, byte precision){
      SetRestMode(false);
      SetTargetRotation(targetRot);
      SetTargetRPM(targetRPM);
      stationary = false;
      precision_degrees = precision;
      doneCommand = false;
     // Serial.println("Current Orientation: " + String(currentDegrees));
     // Serial.println("precision_degrees: " + String(precision));
    }

    void SetUpMotorShield(){
      if(serialDebug)Serial.println("AFMS begin?");
      AFMS.begin();
      if(serialDebug)Serial.println("AFMS begun");
    }

    void SetUpRpmPolls(){
      for (int i = 0; i < RPM_SAMPLES; i ++){
        RPM_current_polls[i] = 0.0f;
      }
    }
    
    void SetUpMotor(int motorNumberLocal){
      motorNumber = motorNumberLocal;
      motor = AFMS.getMotor(motorNumber);
      if(serialDebug)Serial.println("Motor set up");
      if(serialDebug)Serial.println(motorNumber);
      SetUpRpmPolls();
    }


    

    void SetTargetRPM(float newRPM){
      RPM_degrees_current = 0;
      RPM_degrees_target = newRPM;
      // set current power via heuristic:
      currentMotorPower = map(newRPM ,0, 800, 5, 200);
      if(serialDebug)Serial.println("RPM_degrees_target: " + String(RPM_degrees_target));
    }

    void SetTargetRotation(float newRotation){
      targetRotation = newRotation;
      if(serialDebug)Serial.println("Target rotation: " +String(targetRotation));
    }

    // update pseudo queue structure with our last poll.
    // This is an approximation of 'I' in the PID controller
    void AddToRpmPollQueue(float RPM_poll){

      RPM_current_polls[RPM_poll_index] = RPM_poll;
      RPM_poll_index += 1;
      if(RPM_poll_index >= RPM_SAMPLES -1){
        RPM_poll_index = 0;
      }
    }
    
    float CalculateRpmEstimate(){
      float pollSum = 0.0f;
      for (int i = 0; i < RPM_SAMPLES; i ++){
        pollSum += RPM_current_polls[i];
      }
      return (pollSum / RPM_SAMPLES);
    }

    void EmergencyStop(){
      SetMotorToStationary();
      doneCommand = true;
      motor->setSpeed(0);
      emergencyStop = true;
    }

    void EmergencyResume(){
      
      emergencyStop = false;
    }

    void SetRestMode(bool set){
      if(set){
        motor->setSpeed(0);
        restMode = true;
      }
      else{
        restMode = false;
      }
    }
    
    void UpdateLoop(int cumPos, int posDifference) {
        // calculate new degrees
        if(emergencyStop){
          motor->setSpeed(0);
        }
        if(!emergencyStop && !restMode){
          currentTime = micros();
          float timeDifference = (float)(currentTime - previousTime)/1000000;
          float deg = ((float)cumPos / (GEARING*ENCODERMULT))* 360;
    
          float rotationChange = ((float) posDifference / (GEARING*ENCODERMULT)) *360;
          float RPM_poll = abs(rotationChange / timeDifference);
          
          
          currentDegrees = deg;
          //if(rotationChange > 0.1){
            //Serial.println(currentDegrees);
          //}
          //Serial.println(currentDegrees);
          //Serial.println(targetRotation);
          //Serial.println(RPM_degrees_current);
          
          BangBangControl();
          if(!stationary){ 
            AddToRpmPollQueue(RPM_poll);
          
            RPM_degrees_current = CalculateRpmEstimate();
          }
          
          int cumPosDifference = cumPos - lastCumPos;
          //Serial.println(currentDegrees);
          
          lastCumPos = cumPos;
  
          
          
          previousTime = micros();
        }
    }

    // nest P control withing BangBang control
    void Pcontrol(){
      // compare current RPM to target RPM
      // deliver more power if current RPM < target RPM
      if(RPM_degrees_current < RPM_degrees_target){
        if(RPM_degrees_target < 100){
          currentMotorPower += PI_Gain_Slow;
        }
        else{
          currentMotorPower += PI_Gain;
        }
        
      }
      // deliver less power if current RPM > target RPM
      if(RPM_degrees_current > RPM_degrees_target){
        if(RPM_degrees_target < 100){
          currentMotorPower -= PI_Gain_Slow;
        }
        else{
          currentMotorPower -= PI_Gain;
        }
      }

      // set minimum power
      if(currentMotorPower < minimumMotorPower) currentMotorPower = minimumMotorPower;

      motor->setSpeed(currentMotorPower);
      //Serial.println("Setting motor power of " + String(motorNumber)  + " to " + String(currentMotorPower));
    }
   
    void SetMotorToStationary(){;
       RPM_degrees_current = 0;
       motor->setSpeed(0);
       stationary = true;
       //SetUpRpmPolls();
    }
    
    void BangBangControl(){
      
        // get current rotation in degrees.
      //Serial.println(currentDegrees);
      if(precision_degrees < 1){
        precision_degrees = 5;
      }
     
      if(abs(targetRotation-currentDegrees) < precision_degrees){
        //Serial.println("stationary - targetRot:" + String(targetRotation) + " currentDegrees " + String(currentDegrees));
        SetMotorToStationary();
        doneCommand = true;
      }

     else if(targetRotation > currentDegrees && abs(targetRotation-currentDegrees) > precision_degrees){
      //Serial.println("forwards bang bang");
              motor->run(FORWARD);
              //motor->setSpeed(currentMotorPower);
              Pcontrol();
              stationary = false;
              //Serial.println("FORWARD" + String(motorNumber)  + " " + String(currentDegrees) + " -> " +String(targetRotation));
     }

     else if(targetRotation < currentDegrees && abs(targetRotation-currentDegrees) > precision_degrees){
            //  Serial.println("backwards bang bang");
              motor->run(BACKWARD);
             // motor->setSpeed(currentMotorPower);
              Pcontrol();
              stationary = false;
              //Serial.println("BACKWARD" + String(motorNumber)  + " " + String(currentDegrees) + " -> " +String(targetRotation));
     }
    }

};
