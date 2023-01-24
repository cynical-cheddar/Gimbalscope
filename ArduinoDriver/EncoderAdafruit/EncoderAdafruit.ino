
   

#include "MotorController.h"
#include "QuadratureEncoder.h"

#include <Servo.h>
Servo servo1;

int ServoPos = 0;    // variable to store the servo position

volatile float RPM = 0;
volatile uint32_t lastA = 0;

Encoders leftEncoder(18,19);
Encoders rightEncoder(2,3);

float targetRotation = 90.0f;

int pos = 0;

int currentPWM = 90;

long lastLeftEncoderCount = 0;
long lastRightEncoderCount = 0;


int servoAAAA = 0;

long positionLeft  = -999;

MotorController_c motorController_left;
MotorController_c motorController_right;



// DEFINE STATE MACHINE STATES:
#define STATE_SETUP  0
#define STATE_LOOPING_DEMO_MASTER 1
#define STATE_LOOPING_DEMO_FIRE_COMMAND  2
#define STATE_LOOPING_DEMO_FIRE_LOOP  3
#define STATE_LOOPING_DEMO_RELOAD_COMMAND  4
#define STATE_LOOPING_DEMO_RELOAD_LOOP  5
#define STATE_REST  6
int STATE = 0;


Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 

void setup() {
  STATE = STATE_SETUP;
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
  motorController_left.targetRotation = targetRotation;
  
  Serial.println("done right a");
  motorController_right.AFMS = AFMS;
  motorController_right.SetUpMotorShield();
  Serial.println("done right b");
  motorController_right.SetUpMotor(2);
  motorController_left.targetRotation = targetRotation;


  Serial.println("Begun");

  STATE = STATE_LOOPING_DEMO_MASTER;

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




void loop() {
    //if(Serial.available()){
      //SerialDecoder();
    //}

    servo1.write(servoAAAA);
    
  
    //Serial.println("serialAvailable");
    digitalWrite(LED_BUILTIN, LOW);

    // UPDATE LOOP FOR MOTOR DRIVERS
    long currentLeftEncoderCount = leftEncoder.getEncoderCount();
    long posChangeLeft = currentLeftEncoderCount - lastLeftEncoderCount;
    motorController_left.UpdateLoop(currentLeftEncoderCount, posChangeLeft);
    lastLeftEncoderCount = currentLeftEncoderCount;
    posChangeLeft = 0;

    long currentRightEncoderCount = rightEncoder.getEncoderCount();
    long posChangeRight = currentRightEncoderCount - lastRightEncoderCount;
    motorController_right.UpdateLoop(currentRightEncoderCount, posChangeRight);
    // test sync, reduce motor speed for one of the motors
    lastRightEncoderCount = currentRightEncoderCount;
    posChangeRight = 0;
    // END UPDATE LOOP FOR MOTOR DRIVERS

    if(STATE == STATE_LOOPING_DEMO_MASTER){
      STATE = STATE_LOOPING_DEMO_FIRE_COMMAND;
    }

    // ====== DEMO LOOP FIRE STATES ========//
    if(STATE == STATE_LOOPING_DEMO_FIRE_COMMAND){
      // (target angle, degrees per second, precision)
      motorController_right.ReceiveCommand(-60.0f, 360.0f, 3.0f);
      motorController_left.ReceiveCommand(60.0f, 360.0f, 3.0f);
      STATE = STATE_LOOPING_DEMO_FIRE_LOOP;
    }
    if(STATE == STATE_LOOPING_DEMO_FIRE_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        Serial.println("STATE CHANGE TO STATE_LOOPING_DEMO_RELOAD");
        STATE = STATE_LOOPING_DEMO_RELOAD_COMMAND;
      }
    }
    
    // ====== DEMO LOOP RELOAD STATES ========//
    if(STATE == STATE_LOOPING_DEMO_RELOAD_COMMAND){
      // (target angle, degrees per second, precision)
      motorController_right.ReceiveCommand(0.0f, 35.0f, 0.5f);
      motorController_left.ReceiveCommand(0.0f, 35.0f, 0.5f);
      STATE = STATE_LOOPING_DEMO_RELOAD_LOOP;
    }
    if(STATE == STATE_LOOPING_DEMO_RELOAD_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        Serial.println("STATE CHANGE TO STATE_LOOPING_DEMO_MASTER");
        STATE = STATE_LOOPING_DEMO_MASTER;
      }
    }







    delay(10);

   
   if(true){

     
   }
   
}
