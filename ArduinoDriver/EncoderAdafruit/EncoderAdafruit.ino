
   

#include "MotorController.h"
#include "QuadratureEncoder.h"

#include <Servo.h>
Servo servo1;

int ServoPos = 0;    // variable to store the servo position

volatile float RPM = 0;
volatile uint32_t lastA = 0;

Encoders leftEncoder(18,19);
Encoders rightEncoder(2,3);

float targetRotation = 999.0f;

int pos = 0;

int currentPWM = 90;



long positionLeft  = -999;

MotorController_c motorController_left;
MotorController_c motorController_right;

Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 

void setup() {
  Serial.begin(9600);           // set up Serial library at 9600 bps
  Serial.println("begin...");
  servo1.attach(10);
  //pinMode(20, INPUT);
  //pinMode(21, INPUT);
  //SetupEncoders();
  pinMode(LED_BUILTIN, OUTPUT);
 // digitalWrite(LED_BUILTIN, LOW);
  delay(1000);
  Serial.println("done left a");
  motorController_left.AFMS = AFMS;
  motorController_left.SetUpMotorShield();
  Serial.println("done left b");
  motorController_left.SetUpMotor(1);

  Serial.println("done right a");
  motorController_right.AFMS = AFMS;
  motorController_right.SetUpMotorShield();
  Serial.println("done right b");
  motorController_right.SetUpMotor(2);


  Serial.println("Begun");

}

int i = 0;
bool up = true;








void SerialDecoder(){
    // put your main code here, to run repeatedly:
  String readstring = "";
  String currentReadString = "";
  while(Serial.available()){
    //Serial.println("serialAvailable");
    digitalWrite(LED_BUILTIN, HIGH);
    if(Serial.available()){
      char c = Serial.read();

      // if the character is not a dash
      if(c != '/'){
        readstring += c;
      }
      else{
        currentReadString = readstring;
        readstring = "";
        break;
      }
    }
    else{
      digitalWrite(LED_BUILTIN, LOW);
    }
  }

    String messageDataString = "";
    char messageTag;

    

    for (int i = 0; i < currentReadString.length(); i++) {
        if(i == 0) messageTag = currentReadString.charAt(i);
        else messageDataString += currentReadString.charAt(i);
    }

        
    int messageDataInt = messageDataString.toInt();

    //if(messageTag == 'h') targetRotation = messageDataInt;
    targetRotation = messageDataInt;
}



long lastLeftEncoderCount = 0;
long lastRightEncoderCount = 0;


int servoAAAA = 0;
void loop() {
    //if(Serial.available()){
      //SerialDecoder();
    //}

    servo1.write(servoAAAA);
    
  
    //Serial.println("serialAvailable");
    digitalWrite(LED_BUILTIN, LOW);

    long currentLeftEncoderCount = leftEncoder.getEncoderCount();
    long posChangeLeft = currentLeftEncoderCount - lastLeftEncoderCount;
    motorController_left.UpdateLoop(currentLeftEncoderCount, posChangeLeft);
    lastLeftEncoderCount = currentLeftEncoderCount;
    motorController_left.SetTargetRPM(180.0f);
    posChangeLeft = 0;

    long currentRightEncoderCount = rightEncoder.getEncoderCount();
    long posChangeRight = currentRightEncoderCount - lastRightEncoderCount;
    motorController_right.UpdateLoop(currentRightEncoderCount, posChangeRight);
    // test sync, reduce motor speed for one of the motors
    lastRightEncoderCount = currentRightEncoderCount;
    motorController_right.SetTargetRPM(180.0f);
    posChangeRight = 0;

    // we should now wait for both motors to be nearly equal for sync

    if(abs( motorController_right.currentDegrees - motorController_right.targetRotation) < 1 && abs( motorController_left.currentDegrees - motorController_left.targetRotation) < 1){
       motorController_right.targetRotation = -motorController_right.targetRotation;
       motorController_left.targetRotation = -motorController_left.targetRotation;
    }




    delay(10);

   
   if(true){

     
   }
   
}
