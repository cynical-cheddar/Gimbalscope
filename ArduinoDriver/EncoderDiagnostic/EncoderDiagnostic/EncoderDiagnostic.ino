#include "MotorController.h"
#include "QuadratureEncoder.h"

Encoders leftEncoder(2,3);
Encoders rightEncoder(18,19);
long lastLeftEncoderCount = 0;
long lastRightEncoderCount = 0;

MotorController_c motorController_left;
MotorController_c motorController_right;

float GEARING1     = 100;
float ENCODERMULT1 = 28;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.println("begin");
}

void loop() {
    long currentLeftEncoderCount = leftEncoder.getEncoderCount();
    long posChangeLeft = currentLeftEncoderCount - lastLeftEncoderCount;
    motorController_left.UpdateLoop(currentLeftEncoderCount, posChangeLeft);
    lastLeftEncoderCount = currentLeftEncoderCount;
    posChangeLeft = 0;
  //  Serial.println("L");
    Serial.println(((float)currentLeftEncoderCount / (GEARING1*ENCODERMULT1))* 360);
    

    long currentRightEncoderCount = rightEncoder.getEncoderCount();
    long posChangeRight = currentRightEncoderCount - lastRightEncoderCount;
    motorController_right.UpdateLoop(currentRightEncoderCount, posChangeRight);
    // test sync, reduce motor speed for one of the motors
    lastRightEncoderCount = currentRightEncoderCount;
    posChangeRight = 0;

    //Serial.println("R");
   // Serial.println(currentRightEncoderCount);
}
