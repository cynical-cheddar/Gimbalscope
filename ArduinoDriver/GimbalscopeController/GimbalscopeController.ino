// UPDATED VERSION, SENDS orientation info on demand on trigger pull
   

#include "MotorController.h"
#include "BrushlessMotorController.h"
#include "QuadratureEncoder.h"
#include "MessageDecoder.h"
#include "GyroscopeController.h"

#include <Servo.h>

#define TRIGGER_PIN 10
#define SERVO_PIN 17




//#include <Wire.h>
// INPUT VARIABLES
const int buttonPin = 42;  // the number of the pushbutton pin
const int buttonPinTwo = 43;  // the number of the pushbutton pin
// LED VARIABLE
const int greenLEDanaloguePin = 2;
// variables will change:
int buttonState = 0;  // variable for reading the pushbutton status
int buttonStateTwo = 0;  // variable for reading the pushbutton status

int last_buttonState = 0;  // variable for reading the pushbutton status
int last_buttonStateTwo = 0;  // variable for reading the pushbutton status
long triggerHeldDurationMillis = 0;

bool triggerHeld = false;
long triggerHeldStart = 0;

long triggerHoldDurationThreshold = 1000;
bool noTriggerActionUntilRelease = false;


bool serialDebug = false;
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
#define STATE_RELOAD_COMMAND_NO_BEEP 40
#define STATE_RELOAD_LOOP_NO_BEEP 41

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
#define STATE_EMERGENCY_STOP_LOOP 21
#define STATE_EMERGENCY_RESUME 22


int STATE = 0;

//======== COMMAND COMMUNICATION VARIABLES ==========//
// GIMBALS //
float fireTargetAngle_left = 0;
float fireTargetAngle_right = 0; // NEW
float fireDegreesPerSecond = 0;
byte firePrecision = 0;
float reloadTargetAngle_left = 0;
float reloadTargetAngle_right = 0;
float reloadDegreesPerSecond = 0;
byte reloadPrecision = 5;
// CENTRE SERVO //
Servo myservo;
long servo_loop_target_time = 0;
int currentServoAngle = 90;
int servoAngle = 90;
int lastServoAngle = 90;
#define SERVO_DETACHED 0
#define SERVO_COMMAND 1
#define SERVO_LOOP 2
#define SERVO_DONE 3 
#define SERVO_RECENTRE_COMMAND 4
#define SERVO_RECENTRE_LOOP 5
int SERVO_STATE = SERVO_DETACHED;

// BRUSHLESS MOTORS //
float brushlessLeftPWM = 0;
float brushlessRightPWM = 0;
float brushlessInterpolationTime = 1.0f;


Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 

void setup() {
  delay (3000);
  // WIRE IMU TEST
  Serial.begin(9600);           // set up Serial library at 9600 bps

  // myservo.attach(SERVO_PIN);

  if(serialDebug)Serial.println("afms begin");
  
  
  Serial3.begin(9600);           // set up Serial library at 9600 bps
  while (!Serial3) { delay(10); if(serialDebug)Serial.println("no Serial3!"); } // wait until Serial3 console is open
  
  STATE = STATE_SETUP;
  
  pinMode(TRIGGER_PIN, INPUT_PULLUP);

  brushlessMotorController_left.SetUpMotor(brushless_motor_pin_left);
  brushlessMotorController_right.SetUpMotor(brushless_motor_pin_right);


  Serial.println("begin...");

  // INPUT PINS
  pinMode(buttonPin, INPUT);
  pinMode(buttonPinTwo, INPUT);
  // LED pin
  pinMode(A2, OUTPUT);
  // laser pin
  pinMode(A3,OUTPUT);

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

  
  delay(100);
 // brushlessMotorController_left.ForceWriteMicroSeconds(1060);
 // brushlessMotorController_right.ForceWriteMicroSeconds(1060);

  delay(5000);
  brushlessMotorController_left.ForceWriteMicroSeconds(1700);
  brushlessMotorController_right.ForceWriteMicroSeconds(1700);
  delay(5000);
  brushlessMotorController_left.ForceWriteMicroSeconds(1060);
  brushlessMotorController_right.ForceWriteMicroSeconds(1060);
  

  
  
  //delay(1000);
  delay(10000);
  if(serialDebug)Serial.println("Begun");

  brushlessMotorController_left.ForceWritePwm(60);
  brushlessMotorController_right.ForceWritePwm(60);
  delay(100);

  /* gyroscope */
  SetupGyroscope();
  
  digitalWrite(LED_BUILTIN, LOW);
  myservo.attach(SERVO_PIN);
  myservo.write(90);
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


void ZeroGimbals(){
  Serial.println("Zero Gimbals");
  leftEncoder.setEncoderCount(0);
  rightEncoder.setEncoderCount(0);
  delay(100);
  motorController_left.ForceSetOrientation(0);
  motorController_right.ForceSetOrientation(0);
}

//============================================ DECODER SECTION ========================================//

void InputControl(){
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);

  buttonStateTwo = digitalRead(buttonPinTwo);

  if(triggerHeld){
    triggerHeldDurationMillis = millis() - triggerHeldStart;
    //String telemetry = "#" + String((int)(Gyro_X*57.2958)) + "," + String((int)(Gyro_Y*57.2958)) + "," + String((int)(Gyro_Z*57.2958)) + ",";
    //Serial3.println(telemetry);

        
    if(triggerHeldDurationMillis > triggerHoldDurationThreshold && !noTriggerActionUntilRelease){
      if(buttonStateTwo == LOW){
        if(serialDebug)Serial.println("select target");
        Serial3.println("##t");
      }
      else if(buttonStateTwo == HIGH){
        if(serialDebug)Serial.println("calibrate");
        digitalWrite(A3,1);
        delay(40);
        digitalWrite(A3,LOW);
        delay(40);
        digitalWrite(A3,1);
        delay(40);
        digitalWrite(A3,LOW);
        calculate_IMU_error();
        digitalWrite(A3,1);
        Serial3.println("##d");

      }
      noTriggerActionUntilRelease = true;
    }
  }
  
   // TRIGGER BEHAVIOUR
  if(buttonState != last_buttonState){
    last_buttonState = buttonState;
    // pull
    if (buttonState == HIGH) {
      // turn LED on:
      triggerHeld = true;
      triggerHeldStart = millis();
      String telemetry = "#" + String((int)(Gyro_X*57.2958)) + "," + String((int)(Gyro_Y*57.2958)) + "," + String((int)(Gyro_Z*57.2958)) + ",";
      Serial3.println(telemetry);

    // release
    } else {
      if(noTriggerActionUntilRelease == false){
        if(serialDebug)Serial.println("cue");
        Serial3.println("##c");
      }
      noTriggerActionUntilRelease = false;
      triggerHeld = false;
    }

    
  }

  
  // LASER BUTTON BEHAVIOUR
  if(buttonStateTwo != last_buttonStateTwo){
    if (buttonStateTwo == HIGH) {
      // turn LED on:
      
      digitalWrite(A3,1);
    } else {
      // turn LED off:
      digitalWrite(A3,LOW);
    }
    last_buttonStateTwo = buttonStateTwo;
  }
}


bool SerialDecoder2(){

  // put your main code here, to run repeatedly:
  bool validMessage = false;
  // we shall set max buffer size to be 32 bytes
  String  message;
  
  while(Serial3.available() > 0){
    digitalWrite(LED_BUILTIN, HIGH);
    delay(2);
    if (Serial3.available() >0) {
      char c = Serial3.read();  //gets one byte from serial buffer
      message += c; //makes the string readString
      //Serial.println(message);
    } 
  }
  digitalWrite(LED_BUILTIN, LOW);
  // now decode the message
  if(message != ""){
    validMessage = true;
  }
  if(validMessage){
    
    digitalWrite(LED_BUILTIN, LOW);
    char motorType = message.charAt(0);
    char motorID = message.charAt(1);
    char functionID = message.charAt(2);
    if(serialDebug)Serial.println(message);
    if(serialDebug)Serial.println(motorType);
    if(serialDebug)Serial.println(motorID);
    if(serialDebug)Serial.println(functionID);
    // GIMBAL MOTORS:

    if((STATE == STATE_LISTENING_MASTER) || (STATE == STATE_EMERGENCY_STOP) || (STATE == STATE_EMERGENCY_STOP_LOOP)){
      // Zero the gimbals (may be done whenever)
      if (motorType == '0' && motorID == '0' && functionID == '3'){
            ZeroGimbals();
            Serial.println("zero gimbals");
      }
    }
    

    if(STATE != STATE_EMERGENCY_STOP_LOOP){
      if (motorType == '1'){
        if(motorID == '0' && functionID == '0'){
          
          servoAngle = GetMessageParameter(0, message);
          currentServoAngle = servoAngle;
          Serial.println(currentServoAngle);
          myservo.write(currentServoAngle);
          return;
        }
        if(motorID == '0' && functionID == '1' && SERVO_STATE == SERVO_DETACHED){
         // SERVO_STATE = SERVO_RECENTRE_COMMAND;

        }
      }
    }
    if(motorType == '6'){
      // set x to be forces to zero
      Gyro_X = GetMessageParameter(0, message);
    }

    if(STATE == STATE_LISTENING_MASTER){
      if(serialDebug)Serial.println("valid message in STATE_LISTENING_MASTER");

      

    
      if(motorType == '0'){
        if(serialDebug)Serial.println("motor type 0 ");
        // FIRE AND RELOAD both motors
        if(functionID == '0' && motorID == '0'){
          if(serialDebug)Serial.println("Fire and reload");
          int messageLength = message.length();
          String currentSubMessage = "";
          int parameterNumber = 0;
          bool firstParameter = true;

 
          
          fireTargetAngle_left = GetMessageParameter(0, message);
          fireTargetAngle_right = GetMessageParameter(1, message);
          
          
  
          reloadTargetAngle_left = GetMessageParameter(2, message);
          reloadTargetAngle_right = GetMessageParameter(3, message);
          
          
          if(serialDebug)Serial.println("STATE = STATE_FIRE_COMMAND");
          STATE = STATE_FIRE_COMMAND;
          return;
        }
        // receive settings
        else if(functionID == '4' && motorID == '0'){
          fireDegreesPerSecond = GetMessageParameter(0, message);
          firePrecision = GetMessageParameter(1, message);

          reloadDegreesPerSecond = GetMessageParameter(2, message);
          reloadPrecision = GetMessageParameter(3, message);
        }
        // move both gimbal motors to a new base position
        else if(functionID == '1' && motorID == '0'){
          if(serialDebug)Serial.println("Setting gimbal position");
              
          reloadTargetAngle_left = GetMessageParameter(0, message);
          reloadTargetAngle_right = GetMessageParameter(1, message);
          reloadDegreesPerSecond = GetMessageParameter(2, message);
          reloadPrecision = GetMessageParameter(3, message);
  
          STATE = STATE_RELOAD_COMMAND;
          return;
        }
        // move the left gimbal motor to a new base position
        else if(functionID == '1' && motorID == '1'){
          if(serialDebug)Serial.println("Setting gimbal position");
              
          reloadTargetAngle_left = GetMessageParameter(0, message);
          reloadDegreesPerSecond = GetMessageParameter(1, message);
          reloadPrecision = GetMessageParameter(2, message);
  
          STATE = STATE_RELOAD_COMMAND_NO_BEEP;
        }
        // move the right gimbal motor to a new base position
        else if(functionID == '1' && motorID == '2'){
          if(serialDebug)Serial.println("Setting gimbal position");
              
          reloadTargetAngle_right = GetMessageParameter(0, message);
          reloadDegreesPerSecond = GetMessageParameter(1, message);
          reloadPrecision = GetMessageParameter(2, message);
  
          STATE = STATE_RELOAD_COMMAND_NO_BEEP;
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
        else if(functionID == '5' && motorID == '0'){
          if(serialDebug)Serial.println("zero-ing");
          STATE = STATE_ZERO_COMMAND;
        }
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

      
  }
  /*
  // EMERGENCY STOP
  if (motorType == '3'){
    if(serialDebug)Serial.println("EMERGENCY STOP");
    STATE = STATE_EMERGENCY_STOP;
  }
  else if (motorType == '4'){
    if(serialDebug)Serial.println("EMERGENCY RESUME");
    STATE = STATE_EMERGENCY_RESUME;
  }
  */
    
  }
}


int buttonStatus = 0;
// ======================== END DECODER SECTION ========================================//

int generate_telemetry_delay_ms = 400;
long target_time = 0;
int pinValue = 0;
int emergencyStopPinValue = 0;

long currentLeftEncoderCount = 0;
long posChangeLeft = 0;

long currentRightEncoderCount = 0;
long posChangeRight = 0;
long servoDetachTime = 0;
void loop() {
  /*
    // garbage telemetry for testing
   if(millis() > target_time){
    target_time = millis() + generate_telemetry_delay_ms;
    String telemetry = "#" + String(random(0,360)) + "," + String(random(0,360)) + "," + String(random(0,360)) + ",";
    Serial3.println(telemetry);

  }
  */
  // LED awareness:
  if(STATE == STATE_LISTENING_MASTER){
    digitalWrite(A2,HIGH);
  }
  else{
    digitalWrite(A2,LOW);
  }

  // check for emergency stop
  if(STATE != STATE_EMERGENCY_STOP_LOOP){
    emergencyStopPinValue = digitalRead(33);
    if(emergencyStopPinValue == HIGH){
      Serial.println("EMERGENCY STOP BY PIN");
      STATE = STATE_EMERGENCY_STOP;
    }
  }
  if(STATE == STATE_EMERGENCY_STOP_LOOP){
    emergencyStopPinValue = digitalRead(33);
    if(emergencyStopPinValue == LOW){
      Serial.println("EMERGENCY RESUME BY PIN");
      STATE = STATE_EMERGENCY_RESUME;
    }
  }
  //Serial.println(STATE);

  if(serialDebug)Serial.println(STATE);
  //Serial.println(reloadPrecision);
  pinValue = digitalRead(TRIGGER_PIN);
  
  if(buttonStatus != pinValue){
    buttonStatus = pinValue;
    if(serialDebug)Serial.println(buttonStatus);
  }
  

    // Gyroscope
    GyroscopeUpdateLoop();
    // Gyro telemetry
    
    if(millis() > target_time && STATE == STATE_LISTENING_MASTER){
      target_time = millis() + generate_telemetry_delay_ms;
      String telemetry = "#" + String((int)(Gyro_X*57.2958)) + "," + String((int)(Gyro_Y*57.2958)) + "," + String((int)(Gyro_Z*57.2958)) + ",";
      //String telemetry = "#000,000,000,";
      if(STATE != STATE_EMERGENCY_STOP){
        Serial3.println(telemetry);
        if(serialDebug)Serial.println(telemetry);
      }
    }
    
  
    // UPDATE LOOP FOR SERVO
    if(SERVO_STATE == SERVO_COMMAND){
      myservo.attach(SERVO_PIN);
      SERVO_STATE = SERVO_LOOP;
    }
    // one degree per 50 ms
    /*
    if(SERVO_STATE == SERVO_LOOP && millis() > servo_loop_target_time){
      if(servoAngle > currentServoAngle){
        currentServoAngle += 1;
        myservo.write(currentServoAngle);
      }
      else{
        currentServoAngle -= 1;
        myservo.write(currentServoAngle);
      }
      if(abs(currentServoAngle - servoAngle ) < 2){
        SERVO_STATE = SERVO_DONE;
      }
      else{
        servo_loop_target_time = millis() + 5;
      }
    }*/
    if(SERVO_STATE == SERVO_LOOP){
      myservo.write(currentServoAngle);
      SERVO_STATE = SERVO_DONE;
    }
    if(SERVO_STATE == SERVO_DONE){
      //myservo.detach();
      SERVO_STATE = SERVO_DETACHED;
    }
   
    if(SERVO_STATE == SERVO_RECENTRE_COMMAND){
      myservo.attach(SERVO_PIN);
      SERVO_STATE = SERVO_RECENTRE_LOOP;
      servoDetachTime = millis() + 300;
    }
    if(SERVO_STATE == SERVO_RECENTRE_LOOP){
      myservo.write(currentServoAngle);
      //Serial.println("writing servo angle ");
      //Serial.println(currentServoAngle);
      if(millis() > servoDetachTime){
        SERVO_STATE = SERVO_DONE;
      }
    }
    
    // UPDATE LOOP FOR MOTOR DRIVERS
    currentLeftEncoderCount = leftEncoder.getEncoderCount();
    posChangeLeft = currentLeftEncoderCount - lastLeftEncoderCount;
    motorController_left.UpdateLoop(currentLeftEncoderCount, posChangeLeft);
    lastLeftEncoderCount = currentLeftEncoderCount;
    posChangeLeft = 0;
   // Serial.println("L:" + String(currentLeftEncoderCount));
    

    currentRightEncoderCount = rightEncoder.getEncoderCount();
    posChangeRight = currentRightEncoderCount - lastRightEncoderCount;
    motorController_right.UpdateLoop(currentRightEncoderCount, posChangeRight);
    // test sync, reduce motor speed for one of the motors
    lastRightEncoderCount = currentRightEncoderCount;
    posChangeRight = 0;
  //  Serial.println("R:" + String(currentRightEncoderCount));

    // END UPDATE LOOP FOR MOTOR DRIVERS

    // UPDATE LOOP FOR BRUSHLESS DRIVERS
    brushlessMotorController_left.UpdateLoop();
    brushlessMotorController_right.UpdateLoop();
    // END UPDATE LOOP FOR BRUSHLESS DRIVERS

    if(STATE !=  STATE_FIRE_LOOP && STATE != STATE_RELOAD_LOOP)SerialDecoder2();
    

    


    if(STATE == STATE_EMERGENCY_STOP){
      motorController_right.EmergencyStop();
      motorController_left.EmergencyStop();

      brushlessMotorController_left.ReceiveCommand(51, 3);
      brushlessMotorController_right.ReceiveCommand(51, 3);
      if(serialDebug)Serial.println("EMERGENCY STOP");

      motorController_left.SetRestMode(true);
      motorController_right.SetRestMode(true);
      
      STATE = STATE_EMERGENCY_STOP_LOOP;
    }
    else if(STATE == STATE_EMERGENCY_STOP_LOOP){
      motorController_right.EmergencyStop();
      motorController_left.EmergencyStop();
    }
    
    else if(STATE == STATE_EMERGENCY_RESUME){
      motorController_right.EmergencyResume();
      motorController_left.EmergencyResume();
      STATE = STATE_LISTENING_MASTER;
    }

    
    // ======= ENTER DEMO MODE (DEPRECATED) ========= //
    else if(STATE == STATE_LOOPING_DEMO_MASTER){
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
    else if(STATE == STATE_SETUP_ZERO_COMMAND){
      motorController_right.ReceiveCommand(0, 30, 1);
      motorController_left.ReceiveCommand(0, 20, 1);
      STATE = STATE_SETUP_ZERO_LOOP;
    }
    else if(STATE == STATE_SETUP_ZERO_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_LISTENING_MASTER;
      }
    }



    else if(STATE == STATE_LISTENING_MASTER){
      // listen for commands:

      // 
      motorController_left.SetRestMode(false);
      motorController_right.SetRestMode(false);
      
      // default state for listening
      //motor commands can only be given and receievd in this state
      InputControl();

    }
    // ========== FIRE AND RELOAD STATES ===========//
   
    else if(STATE == STATE_FIRE_COMMAND){
      motorController_right.ReceiveCommand(fireTargetAngle_right, fireDegreesPerSecond, firePrecision);
      motorController_left.ReceiveCommand(fireTargetAngle_left, fireDegreesPerSecond, firePrecision);
      Serial3.println("##f");
      myservo.detach();
      STATE = STATE_FIRE_LOOP;
    }
    else if(STATE == STATE_FIRE_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_RELOAD_COMMAND;
      }
    }

    else if(STATE == STATE_RELOAD_COMMAND){
      motorController_right.ReceiveCommand(reloadTargetAngle_right, reloadDegreesPerSecond*2, reloadPrecision);
      motorController_left.ReceiveCommand(reloadTargetAngle_left, reloadDegreesPerSecond, reloadPrecision);
      STATE = STATE_RELOAD_LOOP;
      
    }
    else if(STATE == STATE_RELOAD_COMMAND_NO_BEEP){
      motorController_right.ReceiveCommand(reloadTargetAngle_right, reloadDegreesPerSecond*2, reloadPrecision);
      motorController_left.ReceiveCommand(reloadTargetAngle_left, reloadDegreesPerSecond, reloadPrecision);
      STATE = STATE_RELOAD_LOOP;
    }

    else if(STATE == STATE_RELOAD_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        // recentre servo
        myservo.attach(SERVO_PIN);
        // signal that the trigger is available again
        Serial3.println("##p");
        STATE = STATE_LISTENING_MASTER;
      }
    }
    else if(STATE == STATE_RELOAD_LOOP_NO_BEEP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        // recentre servo
        SERVO_STATE = SERVO_RECENTRE_COMMAND;
        // signal that the trigger is available again
        STATE = STATE_LISTENING_MASTER;
      }
    }

    else if(STATE == STATE_ZERO_COMMAND){
      motorController_right.ReceiveCommand(0, 80, 1);
      motorController_left.ReceiveCommand(0, 80, 1);
      STATE = STATE_ZERO_LOOP;
    }
    else if(STATE == STATE_ZERO_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        STATE = STATE_LISTENING_MASTER;
      }
    }

    // ================ BRUSHLESS MOTOR CONTROL ================ //

    else if(STATE == STATE_BRUSHLESS_PWM_COMMAND){
      brushlessMotorController_left.ReceiveCommand(brushlessLeftPWM, brushlessInterpolationTime);
      brushlessMotorController_right.ReceiveCommand(brushlessRightPWM, brushlessInterpolationTime);
      
      
      STATE = STATE_BRUSHLESS_PWM_LOOP;
    }
    // used to lerp
    else if(STATE == STATE_BRUSHLESS_PWM_LOOP){
      if(brushlessMotorController_left.doneCommand && brushlessMotorController_right.doneCommand){
        STATE = STATE_LISTENING_MASTER;
      }
    }

    




    

    // ====== DEMO LOOP FIRE STATES ========//
    else if(STATE == STATE_LOOPING_DEMO_FIRE_COMMAND){
      // (target angle, degrees per second, precision)
      motorController_right.ReceiveCommand(-60.0f, 360.0f, 3.0f);
      motorController_left.ReceiveCommand(60.0f, 360.0f, 3.0f);
      STATE = STATE_LOOPING_DEMO_FIRE_LOOP;
    }
    else if(STATE == STATE_LOOPING_DEMO_FIRE_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        if(serialDebug)Serial.println("STATE CHANGE TO STATE_LOOPING_DEMO_RELOAD");
        STATE = STATE_LOOPING_DEMO_RELOAD_COMMAND;
      }
    }
    
    // ====== DEMO LOOP RELOAD STATES ========//
    else if(STATE == STATE_LOOPING_DEMO_RELOAD_COMMAND){
      // (target angle, degrees per second, precision)
      motorController_right.ReceiveCommand(0.0f, 35.0f, 1.0f);
      motorController_left.ReceiveCommand(0.0f, 35.0f, 1.0f);
      STATE = STATE_LOOPING_DEMO_RELOAD_LOOP;
    }
    else if(STATE == STATE_LOOPING_DEMO_RELOAD_LOOP){
      if(motorController_right.doneCommand && motorController_left.doneCommand){
        if(serialDebug)Serial.println("STATE CHANGE TO STATE_LOOPING_DEMO_MASTER");
        STATE = STATE_LOOPING_DEMO_MASTER;
      }
    }










   
}
