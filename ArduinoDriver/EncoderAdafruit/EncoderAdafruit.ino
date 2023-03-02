
   

#include "MotorController.h"
#include "BrushlessMotorController.h"
#include "QuadratureEncoder.h"
#include "MessageDecoder.h"
#include "GyroscopeController.h"

#include <Servo.h>

#define TRIGGER_PIN 10
//#include <Wire.h>
// TEST VARIABLES FOR IMU DATA




bool serialDebug = true;
Adafruit_DCMotor *debugMotor;


#define brushless_motor_pin_left 8
#define brushless_motor_pin_right 9


int ServoPos = 0;    // variable to store the servo position

volatile float RPM = 0;
volatile uint32_t lastA = 0;

Encoders leftEncoder(2,3);
Encoders rightEncoder(18,19);

float targetRotation = 0.0f;

int pos = 0;

int currentPWM = 90;

long lastLeftEncoderCount = 0;
long lastRightEncoderCount = 0;


int servoAAAA = 0;

long positionLeft  = -999;

MotorController_c motorController_left;
MotorController_c motorController_right;

BrushlessMotorController_c brushlessMotorController_left;
BrushlessMotorController_c brushlessMotorController_right;

// DEFINE STATE MACHINE STATES:
#define STATE_SETUP  0
#define STATE_LOOPING_DEMO_MASTER 1
#define STATE_LOOPING_DEMO_FIRE_COMMAND  2
#define STATE_LOOPING_DEMO_FIRE_LOOP  3
#define STATE_LOOPING_DEMO_RELOAD_COMMAND  4
#define STATE_LOOPING_DEMO_RELOAD_LOOP  5

// State defines listening behaviour
#define STATE_LISTENING_MASTER 6
// States define fire and reload behaviour
#define STATE_FIRE_COMMAND  7
#define STATE_FIRE_LOOP  8
#define STATE_RELOAD_COMMAND  9
#define STATE_RELOAD_LOOP  10


// States define the behaviour to reset the gimbal rotations to zero degrees
#define STATE_ZERO_COMMAND 11
#define STATE_ZERO_LOOP 12

// States define zero-ing setup:
#define STATE_SETUP_ZERO_COMMAND 13
#define STATE_SETUP_ZERO_LOOP 14

// States define sinusoidal gimbal behaviour (not yet implemented)
#define STATE_SINUSOIDAL_COMMAND 15
#define STATE_SINUSOIDAL_LOOP 16

// States define controlling brushless motor power
#define STATE_BRUSHLESS_PWM_COMMAND 17
#define STATE_BRUSHLESS_PWM_LOOP 18

// States define rest
#define STATE_REST  19

// Emergency stop / resume states
#define STATE_EMERGENCY_STOP 20
#define STATE_EMERGENCY_RESUME 21


int STATE = 0;

//======== COMMAND COMMUNICATION VARIABLES ==========//
// GIMBALS //
float fireTargetAngle = 0;
float fireDegreesPerSecond = 0;
byte firePrecision = 0;
float reloadTargetAngle_left = 0;
float reloadTargetAngle_right = 0;
float reloadDegreesPerSecond = 0;
byte reloadPrecision = 0;
// CENTRE SERVO //

// BRUSHLESS MOTORS //
float brushlessLeftPWM = 0;
float brushlessRightPWM = 0;
float brushlessInterpolationTime = 1.0f;


Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 

void setup() {
  delay (3000);
  // WIRE IMU TEST
  if(serialDebug)Serial.begin(9600);           // set up Serial library at 9600 bps
  if(serialDebug){
    while (!Serial) { delay(10); Serial.println("no serial");} // wait until Serial3 console is open, remove if not tethered to computer
    Serial.println("serial done");
  }

  AFMS.begin();
  Serial.println("afms begin");
  
  
  Serial3.begin(9600);           // set up Serial library at 9600 bps
  while (!Serial3) { delay(10); if(serialDebug)Serial.println("no Serial3!"); } // wait until Serial3 console is open
  
  STATE = STATE_SETUP;
  
  pinMode(TRIGGER_PIN, INPUT_PULLUP);

  brushlessMotorController_left.SetUpMotor(brushless_motor_pin_left);
  brushlessMotorController_right.SetUpMotor(brushless_motor_pin_right);


  if(serialDebug)Serial.println("begin...");

  //pinMode(20, INPUT);
  //pinMode(21, INPUT);
  //SetupEncoders();
  pinMode(LED_BUILTIN, OUTPUT);
 // digitalWrite(LED_BUILTIN, LOW);
 
  delay(1000);
  if(serialDebug)Serial.println("done left a");
  motorController_left.AFMS = AFMS;
  if(serialDebug)Serial.println("done left a pt 2");
  motorController_left.SetUpMotorShield();
  if(serialDebug)Serial.println("done left b");
  motorController_left.SetUpMotor(1);
  motorController_left.targetRotation = targetRotation;
  
  if(serialDebug)Serial.println("done right a");
  motorController_right.AFMS = AFMS;
  motorController_right.SetUpMotorShield();
  if(serialDebug)Serial.println("done right b");
  motorController_right.SetUpMotor(2);
  motorController_right.targetRotation = targetRotation;

  
  if(serialDebug)Serial.println("Setting up brushless calibration");

  /*
  delay(100);
  //brushlessMotorController_left.ForceWriteMicroSeconds(1060);
  //brushlessMotorController_right.ForceWriteMicroSeconds(1060);

  delay(5000);
  brushlessMotorController_left.ForceWriteMicroSeconds(1700);
  brushlessMotorController_right.ForceWriteMicroSeconds(1700);
  delay(5000);
  brushlessMotorController_left.ForceWriteMicroSeconds(1060);
  brushlessMotorController_right.ForceWriteMicroSeconds(1060);
  */

  /* gyroscope */
  SetupGyroscope();
  
  delay(1000);
  //delay(10000);
  if(serialDebug)Serial.println("Begun");

  //brushlessMotorController_left.ForceWritePwm(100);
  //brushlessMotorController_right.ForceWritePwm(100);
  
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

void SerialEmergencyPeek(){
  char c = 0;
  if(Serial3.available() > 0){
    delay(3);
    digitalWrite(LED_BUILTIN, HIGH);
    char c = Serial3.peek();  //gets one byte from serial buffer
  }
  if(c == '3'){
    STATE = STATE_EMERGENCY_STOP;
    Serial3.println("###EMERGENCY STOP");
  }
  else if (c == '4'){
    STATE = STATE_EMERGENCY_RESUME;
  }
}


bool SerialDecoder2(){

  // put your main code here, to run repeatedly:
  bool validMessage = false;
  // we shall set max buffer size to be 32 bytes
  String  message;
  
  while(Serial3.available() > 0){
    delay(3);
    digitalWrite(LED_BUILTIN, HIGH);
    if (Serial3.available() >0) {
      char c = Serial3.read();  //gets one byte from serial buffer
      message += c; //makes the string readString
    } 
  }
  digitalWrite(LED_BUILTIN, LOW);
  // now decode the message
  if(message != ""){
    if(serialDebug)Serial.println(message);
    delay(50);
    validMessage = true;
  }
  if(validMessage){
    digitalWrite(LED_BUILTIN, LOW);
    byte motorType = message.charAt(0);
    byte motorID = message.charAt(1);
    byte functionID = message.charAt(2);
    // GIMBAL MOTORS:
    if(motorType == '0'){
      // fire and reload both motors
      if(functionID == '0' && motorID == '0'){
        if(serialDebug)Serial.println("Fire and reload");
        int messageLength = message.length();
        String currentSubMessage = "";
        int parameterNumber = 0;
        bool firstParameter = true;
        
        fireTargetAngle = GetMessageParameter(0, message);
        fireDegreesPerSecond = GetMessageParameter(1, message);
        firePrecision = GetMessageParameter(2, message);

        reloadTargetAngle_left = GetMessageParameter(3, message);
        reloadTargetAngle_right = GetMessageParameter(3, message);
        reloadDegreesPerSecond = GetMessageParameter(4, message);
        reloadPrecision = GetMessageParameter(5, message);
        STATE = STATE_FIRE_COMMAND;
        return;
      }
      // move both gimbal motors to a new base position
      else if(functionID == '1' && motorID == '0'){
        if(serialDebug)Serial.println("Setting gimbal position");
            
        reloadTargetAngle_left = GetMessageParameter(0, message);
        reloadTargetAngle_right = GetMessageParameter(0, message);
        reloadDegreesPerSecond = GetMessageParameter(1, message);
        reloadPrecision = GetMessageParameter(2, message);

        STATE = STATE_RELOAD_COMMAND;
      }
      // move the left gimbal motor to a new base position
      else if(functionID == '1' && motorID == '1'){
        if(serialDebug)Serial.println("Setting gimbal position");
            
        reloadTargetAngle_left = GetMessageParameter(0, message);
        reloadDegreesPerSecond = GetMessageParameter(1, message);
        reloadPrecision = GetMessageParameter(2, message);

        STATE = STATE_RELOAD_COMMAND;
      }
      // move the right gimbal motor to a new base position
      else if(functionID == '1' && motorID == '2'){
        if(serialDebug)Serial.println("Setting gimbal position");
            
        reloadTargetAngle_right = GetMessageParameter(0, message);
        reloadDegreesPerSecond = GetMessageParameter(1, message);
        reloadPrecision = GetMessageParameter(2, message);

        STATE = STATE_RELOAD_COMMAND;
      }
      // sinusoidal movement
      else if(functionID == '2' && motorID == '0'){
        if(serialDebug)Serial.println("Setting gimbal position");
            
        reloadTargetAngle_left = GetMessageParameter(0, message);
        reloadTargetAngle_right = GetMessageParameter(0, message);
        reloadDegreesPerSecond = GetMessageParameter(1, message);
        reloadPrecision = GetMessageParameter(2, message);

        STATE = STATE_RELOAD_COMMAND;
      }
    }
    else if (motorType == '1'){
      if(serialDebug)Serial.println("servo not yet implemented");
    }
    else if (motorType == '2'){
      // set brushless pwm
      if(functionID == '0'){
        float newBrushlessPWM = GetMessageParameter(0, message);
        brushlessInterpolationTime = GetMessageParameter(1, message);
        // set for both motors
        if(motorID == '0'){
          brushlessLeftPWM = newBrushlessPWM;
          brushlessRightPWM = newBrushlessPWM;
          if(serialDebug)Serial.println("pwm acknowledged");
          if(serialDebug)Serial.println(newBrushlessPWM);
        }
        // set for left motor
        else if(motorID == '1'){
          brushlessLeftPWM = newBrushlessPWM;
        }
        // set for right motor
        else if(motorID == '2'){
          brushlessRightPWM = newBrushlessPWM;
        }
        STATE = STATE_BRUSHLESS_PWM_COMMAND;
      }
    }
    else{
      if(serialDebug)Serial.println("motor not yet implemented");
    }
  }
}


int buttonStatus = 0;
// ======================== END DECODER SECTION ========================================//

int generate_telemetry_delay_ms = 500;
long target_time = 0;

void loop() {
  /*
    // garbage telemetry for testing
   if(millis() > target_time){
    target_time = millis() + generate_telemetry_delay_ms;
    String telemetry = "#" + String(random(0,360)) + "," + String(random(0,360)) + "," + String(random(0,360)) + ",";
    Serial3.println(telemetry);

  }
  */
  int pinValue = digitalRead(TRIGGER_PIN);
  
  if(buttonStatus != pinValue){
    buttonStatus = pinValue;
    if(serialDebug)Serial.println(buttonStatus);
  }
  

    // Gyroscope
    GyroscopeUpdateLoop();
    // Gyro telemetry
    if(millis() > target_time){
      target_time = millis() + generate_telemetry_delay_ms;
      String telemetry = "#" + String((int)(Gyro_X*57.2958)) + "," + String((int)(Gyro_Y*57.2958)) + "," + String((int)(Gyro_Z*57.2958)) + ",";
      //String telemetry = "#000,000,000,";
      Serial3.println(telemetry);
      if(serialDebug)Serial.println(telemetry);
    }
  
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

    // UPDATE LOOP FOR BRUSHLESS DRIVERS
    brushlessMotorController_left.UpdateLoop();
    brushlessMotorController_right.UpdateLoop();
    // END UPDATE LOOP FOR BRUSHLESS DRIVERS



    // EMERGENCY STOP CODE:
    SerialEmergencyPeek();

    if(STATE == STATE_EMERGENCY_STOP){
      motorController_right.EmergencyStop();
      motorController_left.EmergencyStop();

      brushlessMotorController_left.ReceiveCommand(50, 1);
      brushlessMotorController_right.ReceiveCommand(50, 1);
      if(serialDebug)Serial.println("EMERGENCY STOP");
    }

    // ======= ENTER DEMO MODE (DEPRECATED) ========= //
    if(STATE == STATE_LOOPING_DEMO_MASTER){
      STATE = STATE_LOOPING_DEMO_FIRE_COMMAND;
    }

    // ======= SETUP AND COMMAND RECEIVE STATES ========== //
    // relevant states
    /*
    STATE_LISTENING_MASTER 6
    STATE_FIRE_COMMAND  7
    STATE_FIRE_LOOP  8
    STATE_RELOAD_COMMAND  9
    STATE_RELOAD_LOOP  10
    */
    if(STATE == STATE_SETUP_ZERO_COMMAND){
      motorController_right.ReceiveCommand(0, 30, 1);
      motorController_left.ReceiveCommand(0, 20, 1);
      STATE = STATE_SETUP_ZERO_LOOP;
    }
    if(STATE == STATE_SETUP_ZERO_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_LISTENING_MASTER;
      }
    }

    if(STATE == STATE_LISTENING_MASTER){
      
      SerialDecoder2();
      
    }
    // ========== FIRE AND RELOAD STATES ===========//
   
    if(STATE == STATE_FIRE_COMMAND){
      digitalWrite(LED_BUILTIN, HIGH);
  
      if(serialDebug)Serial.println(String(fireTargetAngle) + " " + String(fireDegreesPerSecond)+ " " + String( firePrecision ));
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
      motorController_right.ReceiveCommand(-reloadTargetAngle_right, reloadDegreesPerSecond, reloadPrecision);
      motorController_left.ReceiveCommand(reloadTargetAngle_left, reloadDegreesPerSecond, reloadPrecision);
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

    // ================ BRUSHLESS MOTOR CONTROL ================ //

    if(STATE == STATE_BRUSHLESS_PWM_COMMAND){
      brushlessMotorController_left.ReceiveCommand(brushlessLeftPWM, brushlessInterpolationTime);
      brushlessMotorController_right.ReceiveCommand(brushlessRightPWM, brushlessInterpolationTime);
      
      
      STATE = STATE_BRUSHLESS_PWM_LOOP;
    }
    // used to lerp
    if(STATE == STATE_BRUSHLESS_PWM_LOOP){
      if(brushlessMotorController_left.doneCommand && brushlessMotorController_right.doneCommand){
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
