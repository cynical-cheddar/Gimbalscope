
   

#include "MotorController.h"
#include "QuadratureEncoder.h"

#include <Servo.h>





bool serialDebug = true;
Adafruit_DCMotor *debugMotor;

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

// States define fire and reload behaviour
#define STATE_LISTENING_MASTER 6
#define STATE_FIRE_COMMAND  7
#define STATE_FIRE_LOOP  8
#define STATE_RELOAD_COMMAND  9
#define STATE_RELOAD_LOOP  10


// States define the behaviour to reset the gimbal rotations to zero degrees
#define STATE_ZERO_COMMAND 11
#define STATE_ZERO_LOOP 12


#define STATE_REST  11
int STATE = 0;

//======== COMMAND COMMUNICATION VARIABLES ==========//
float fireTargetAngle = 0;
float fireDegreesPerSecond = 0;
byte firePrecision = 0;
float reloadTargetAngle = 0;
float reloadDegreesPerSecond = 0;
byte reloadPrecision = 0;

Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 

void setup() {
  STATE = STATE_SETUP;
  Serial.begin(9600);           // set up Serial library at 9600 bps


  if(serialDebug)Serial.println("begin...");
  servo1.attach(10);
  //pinMode(20, INPUT);
  //pinMode(21, INPUT);
  //SetupEncoders();
  pinMode(LED_BUILTIN, OUTPUT);
 // digitalWrite(LED_BUILTIN, LOW);
  delay(1000);
  if(serialDebug)Serial.println("done left a");
  motorController_left.AFMS = AFMS;
  motorController_left.SetUpMotorShield();
  if(serialDebug)Serial.println("done left b");
  motorController_left.SetUpMotor(1);
  motorController_left.targetRotation = targetRotation;
  
  if(serialDebug)Serial.println("done right a");
  motorController_right.AFMS = AFMS;
  motorController_right.SetUpMotorShield();
  if(serialDebug)Serial.println("done right b");
  motorController_right.SetUpMotor(2);
  motorController_left.targetRotation = targetRotation;


  if(serialDebug)Serial.println("Begun");

  debugMotor = AFMS.getMotor(4);
  debugMotor->setSpeed(60);
  digitalWrite(LED_BUILTIN, LOW);
  STATE = STATE_LISTENING_MASTER;

  /*
  while(true){
    Serial.println("/");
    delay(100);
  }
  */

}

int i = 0;
bool up = true;


//============================================ DECODER SECTION ========================================//

bool SerialDecoder2(){
  // put your main code here, to run repeatedly:
  bool validMessage = true;
  // we shall set max buffer size to be 32 bytes
  static byte message[32];
  byte parameters[28];
  static unsigned int message_pos = 0;
  while(Serial.available() > 0){
    delay(5);
    Serial.println("-");
    digitalWrite(LED_BUILTIN, HIGH);
    char inByte = Serial.read();

    // add byte to message
    if(inByte != '/' && (message_pos+1 < 32)){
      message[message_pos] = inByte;
      message_pos ++;
    }
    else{
      break;
    }
  }
  digitalWrite(LED_BUILTIN, LOW);
  // now decode the message

  byte motorType = message[0];
  byte motorID = message[1];
  byte functionID = message[2];

  int endByteIndex = message_pos;
  message_pos = 0;

  // failure conditions
  if(motorType > 2) return false;
  if(motorID > 2) return false;
  if(functionID > 1) return false;
  Serial.println("Got send some data");
  // gimbal motors
  if(motorType == 0){
    // specific motor
    
    // FIRE AND RELOAD - expect 6 parameters
    if(functionID == 0){
      int j = 0;
      for (int i = 3; i < endByteIndex; i++){
        parameters[j] = message[i];
        j++;
      }

      //if (motorID == ){
      // EXTRACT VARIABLES from PARAMETERS
      // (target angle, degrees per second, precision) x 2
      // 4 bytes per float
      // (4 + 4 + 1) * 2 = 18 bytes
      if(serialDebug)Serial.println("Doing fire command");
      fireTargetAngle = 0;
      uint8_t bytes[4] = {  parameters[0],  parameters[1],  parameters[2], parameters[3]}; // fill this array with the four bytes you received
      static_assert(sizeof(float) == 4, "float size is expected to be 4 bytes");
      memcpy (&fireTargetAngle, bytes, 4);
      
      fireDegreesPerSecond = 0;
      uint8_t bytes_2[4] = {  parameters[4],  parameters[5],  parameters[6], parameters[7]}; // fill this array with the four bytes you received
      static_assert(sizeof(float) == 4, "float size is expected to be 4 bytes");
      memcpy (&fireDegreesPerSecond, bytes, 4);

        
      firePrecision = parameters[8];

      reloadTargetAngle = 0;
      uint8_t bytes_3[4] = {  parameters[9],  parameters[10],  parameters[11], parameters[12]}; // fill this array with the four bytes you received
      static_assert(sizeof(float) == 4, "float size is expected to be 4 bytes");
      memcpy (&reloadTargetAngle, bytes, 4);
      
      reloadDegreesPerSecond = 0;
      uint8_t bytes_4[4] = {  parameters[13],  parameters[14],  parameters[15], parameters[16]}; // fill this array with the four bytes you received
      static_assert(sizeof(float) == 4, "float size is expected to be 4 bytes");
      memcpy (&reloadDegreesPerSecond, bytes, 4);
        
      reloadPrecision = parameters[17];
      // advance state
      STATE = STATE_FIRE_COMMAND;
      // return true
      return true;
    }



    
    // move to position - expect 3 parameters
    if(functionID == 1){
      
    }
    
  }
  // mid servo
  else if(motorType == 1){
    
  }
  //brushless motors
  else if(motorType == 2){
    
  }
}


// ======================== END DECODER SECTION ========================================//



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

    // ======= COMMAND RECEIVE TEST ========== //
    // relevant states
    /*
    STATE_LISTENING_MASTER 6
    STATE_FIRE_COMMAND  7
    STATE_FIRE_LOOP  8
    STATE_RELOAD_COMMAND  9
    STATE_RELOAD_LOOP  10
    */
    if(STATE == STATE_LISTENING_MASTER){
      SerialDecoder2();
      
    }
    
    if(STATE == STATE_FIRE_COMMAND){
      digitalWrite(LED_BUILTIN, HIGH);
      motorController_right.ReceiveCommand(-fireTargetAngle, fireDegreesPerSecond, firePrecision);
      motorController_left.ReceiveCommand(fireTargetAngle, fireDegreesPerSecond, firePrecision);
      STATE = STATE_FIRE_LOOP;
    }
    if(STATE == STATE_FIRE_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_RELOAD_COMMAND;
      }
    }

    if(STATE == STATE_RELOAD_COMMAND){
      motorController_right.ReceiveCommand(reloadTargetAngle, reloadDegreesPerSecond, reloadPrecision);
      motorController_left.ReceiveCommand(reloadTargetAngle, reloadDegreesPerSecond, reloadPrecision);
      STATE = STATE_RELOAD_LOOP;
    }

    if(STATE == STATE_RELOAD_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_LISTENING_MASTER;
      }
    }

    if(STATE == STATE_ZERO_COMMAND){
      motorController_right.ReceiveCommand(0, 20, 1);
      motorController_left.ReceiveCommand(0, 20, 1);
      STATE = STATE_ZERO_LOOP;
    }
    if(STATE == STATE_ZERO_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_LISTENING_MASTER;
      }
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
        if(serialDebug)Serial.println("STATE CHANGE TO STATE_LOOPING_DEMO_RELOAD");
        STATE = STATE_LOOPING_DEMO_RELOAD_COMMAND;
      }
    }
    
    // ====== DEMO LOOP RELOAD STATES ========//
    if(STATE == STATE_LOOPING_DEMO_RELOAD_COMMAND){
      // (target angle, degrees per second, precision)
      motorController_right.ReceiveCommand(0.0f, 35.0f, 1.0f);
      motorController_left.ReceiveCommand(0.0f, 35.0f, 1.0f);
      STATE = STATE_LOOPING_DEMO_RELOAD_LOOP;
    }
    if(STATE == STATE_LOOPING_DEMO_RELOAD_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        if(serialDebug)Serial.println("STATE CHANGE TO STATE_LOOPING_DEMO_MASTER");
        STATE = STATE_LOOPING_DEMO_MASTER;
      }
    }







    delay(10);

   
   if(true){

     
   }
   
}
