// this #ifndef stops this file
// from being included mored than
// once by the compiler. 
#ifndef _KINEMATICS_H
#define _KINEMATICS_H

#include <Adafruit_MotorShield.h>
#include "QuadratureEncoder.h"
// Class to track robot position.
class MotorController_c {

    
  public:

  
    #define RPM_SAMPLES 20
    float GEARING     = 100;
    float ENCODERMULT = 28;

    bool doneCommand = false;
    float precision_degrees = 2.0f;

    float currentDegrees = 0.0f;

    float targetRotation = 90.0f;


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
    

    float PI_Gain = 0.2f;
    
    bool stationary = false;
    int motorNumber = 0;
    Adafruit_MotorShield AFMS;
    Adafruit_DCMotor *motor;
    // Constructor, must exist.
    MotorController_c() {
        
    } 



    void ReceiveCommand(float targetRot, float targetRPM, float precision){
      SetTargetRotation(targetRot);
      SetTargetRPM(targetRPM);
      precision_degrees = precision;
      doneCommand = false;
    }

    void SetUpMotorShield(){
      Serial.println("AFMS begin?");
      AFMS.begin();
      Serial.println("AFMS begun");
    }

    void SetUpRpmPolls(){
      for (int i = 0; i < RPM_SAMPLES; i ++){
        RPM_current_polls[i] = 0.0f;
      }
    }
    
    void SetUpMotor(int motorNumberLocal){
      motorNumber = motorNumberLocal;
      motor = AFMS.getMotor(motorNumber);
      Serial.println("Motor set up");
      Serial.println(motorNumber);
      SetUpRpmPolls();
    }


    

    void SetTargetRPM(float newRPM){
      RPM_degrees_target = newRPM;
      // set current power via heuristic:
      currentMotorPower = map(newRPM ,0, 580, 22, 255);
    }

    void SetTargetRotation(float newRotation){
      targetRotation = newRotation;
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
    
    void UpdateLoop(int cumPos, int posDifference) {
        // calculate new degrees
        
        currentTime = micros();
        double timeDifference = (float)(currentTime - previousTime)/1000000;
        float deg = ((float)cumPos / (GEARING*ENCODERMULT))* 360;
  
        float rotationChange = ((float) posDifference / (GEARING*ENCODERMULT)) *360;
        float RPM_poll = abs(rotationChange / timeDifference);
        if(!stationary){ 
          AddToRpmPollQueue(RPM_poll);
        
          RPM_degrees_current = CalculateRpmEstimate();
        }
        
        currentDegrees = deg;
        //Serial.println(currentDegrees);
        //Serial.println(targetRotation);
        //Serial.println(RPM_degrees_current);
        
        BangBangControl();
        int cumPosDifference = cumPos - lastCumPos;
        //Serial.println(currentDegrees);
        
        lastCumPos = cumPos;
        previousTime = micros();
    }

    // nest P control withing BangBang control
    void Pcontrol(){
      // compare current RPM to target RPM
      // deliver more power if current RPM < target RPM
      if(RPM_degrees_current < RPM_degrees_target){
        currentMotorPower += PI_Gain;
      }
      // deliver less power if current RPM > target RPM
      if(RPM_degrees_current > RPM_degrees_target){
        currentMotorPower -= PI_Gain;
      }

      // set minimum power
      if(currentMotorPower < minimumMotorPower) currentMotorPower = minimumMotorPower;

      motor->setSpeed(currentMotorPower);
    }
   
    void SetMotorToStationary(){;
       motor->setSpeed(0);
       stationary = true;
       SetUpRpmPolls();
    }
    
    void BangBangControl(){
      
        // get current rotation in degrees.
      //Serial.println(currentDegrees);
      
      if(abs(targetRotation-currentDegrees) < precision_degrees){
        SetMotorToStationary();
        doneCommand = true;
      }
      // if target is larger than current, pwm >90
      // if current pwm < 90, do sweep function from 80 to 100, then set pwm to desired >100
      // else set pwm to desired
     else if(targetRotation > currentDegrees && abs(targetRotation-currentDegrees) > precision_degrees){
        //  if(currentPWM <= 90){
        //      SweepLowToHigh();
        //  }
        //  else{
              //SetPWM(105+5);
              motor->run(FORWARD);
              Pcontrol();
              stationary = false;
        // }
        

     }
      // if target is smaller than current, pwm < 90
      // if current pwm > 90, do sweep function from 100 to 80, then set pwm to desired <80
     else if(targetRotation < currentDegrees && abs(targetRotation-currentDegrees) > precision_degrees){
          //if(currentPWM >= 90){
          //    SweepHighToLow();
          //}
         // else{
              //SetPWM(75-5);
              motor->run(BACKWARD);
              Pcontrol();
              stationary = false;
          //}
          

     }
    }

};



#endif
