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

    float GEARING     = 100;
    float ENCODERMULT = 28;

    float currentDegrees = 0.0f;

    float targetRotation = 180.0f;

    float targetRPM = 0;

    float currentRPM = 0;

    float currentPWM = 0;

    int lastCumPos = 0;

    float baseMotorPower = 70.0f;

    unsigned long previousTime = micros();
    unsigned long currentTime = micros();


    int motorNumber = 0;
    Adafruit_MotorShield AFMS;
    Adafruit_DCMotor *motor;
    // Constructor, must exist.
    MotorController_c() {
        
    } 

    void SetUpMotorShield(){
      Serial.println("AFMS begin?");
      AFMS.begin();
      Serial.println("AFMS begun");
    }

    void SetUpMotor(int motorNumberLocal){
      motorNumber = motorNumberLocal;
      motor = AFMS.getMotor(motorNumber);
      Serial.println("Motor set up");
      Serial.println(motorNumber);
    }

    void SetTargetRPM(float newRPM){
      targetRPM = newRPM;
    }

    void SetTargetRotation(float newRotation){
      targetRotation = newRotation;
    }


    
    void UpdateLoop(int cumPos, int posDifference) {
        // calculate new degrees
        
        currentTime = micros();
        double timeDifference = (float)(currentTime - previousTime)/1000000;
        float deg = ((float)cumPos / (GEARING*ENCODERMULT))* 360;
  
        float rotationChange = ((float) posDifference / (GEARING*ENCODERMULT));
        currentRPM = rotationChange * timeDifference;
        
        currentDegrees = deg;
        Serial.println(currentDegrees);
        //Serial.println(targetRotation);
        BangBangControl();
        int cumPosDifference = cumPos - lastCumPos;
        //Serial.println(currentDegrees);
        
        lastCumPos = cumPos;
        previousTime = micros();
    }

    void Pcontrol(){
      
    }

    void SetPWM(int desiredPWM){
      //esc.write(desiredPWM);
      if(desiredPWM == 90){
        motor->setSpeed(0);
      }
      if(desiredPWM < 90){
        int pwm = map(desiredPWM, 0, 90, 255, 30);
        motor->run(BACKWARD);
        motor->setSpeed(pwm);
        
        //Serial.println("moving down");
      }
      else if(desiredPWM > 90){
        int pwm = map(desiredPWM, 90, 180, 30, 255);
        
        motor->run(FORWARD);
        motor->setSpeed(pwm);
        
        //Serial.println("moving up");
      }
      
      currentPWM = desiredPWM;
    }

    

    void BangBangControl(){
      
        // get current rotation in degrees.
      //Serial.println(currentDegrees);
      if(abs(targetRotation-currentDegrees) < 1){
          //SetPWM(90);
          motor->setSpeed(0);
      }
      // if target is larger than current, pwm >90
      // if current pwm < 90, do sweep function from 80 to 100, then set pwm to desired >100
      // else set pwm to desired
     else if(targetRotation > currentDegrees && abs(targetRotation-currentDegrees) > 1.1){
        //  if(currentPWM <= 90){
        //      SweepLowToHigh();
        //  }
        //  else{
              //SetPWM(105+5);
              motor->run(FORWARD);
              motor->setSpeed(baseMotorPower);
        // }
        

     }
      // if target is smaller than current, pwm < 90
      // if current pwm > 90, do sweep function from 100 to 80, then set pwm to desired <80
     else if(targetRotation < currentDegrees && abs(targetRotation-currentDegrees) > 1.1){
          //if(currentPWM >= 90){
          //    SweepHighToLow();
          //}
         // else{
              //SetPWM(75-5);
              motor->run(BACKWARD);
              motor->setSpeed(baseMotorPower);
          //}
          

     }
    }

};



#endif
