#include <Servo.h>
#include <Wire.h>

//#include "utility/Adafruit_MS_PWMServoDriver.h"
//Adafruit_MotorShield AFMS = Adafruit_MotorShield(); 
//Adafruit_DCMotor *dcMotor = AFMS.getMotor(1);

#define SensorPin A0
#define MotorPin 6
#define MotorPinTwo 3
#define ServoPin 7
#define ServoPinTwo 10
Servo motor; 
Servo motor2; 
Servo servo; 
Servo servo2; 
String data;
char d1;

int motorSpeed = 0;
int sensorVal = 0;


bool settingUp = true;
void setup() {
  // put your setup code here, to run once:
 
 // AFMS.begin();
  Serial.begin(9600);
  pinMode(6, OUTPUT);
  motor.attach(MotorPin);
  motor2.attach(MotorPinTwo);
  servo.attach(ServoPin);
  servo2.attach(ServoPinTwo);
 // motor.writeMicroseconds(1060);
  
  while(settingUp){
    if(Serial.available()){
        char c = Serial.read();
        if(c == 'b') break;
    }
    
  }
  
 
  delay(100);
  motor.writeMicroseconds(1060);
  motor2.writeMicroseconds(1060);

  delay(5000);
  motor.writeMicroseconds(1700);
  motor2.writeMicroseconds(1700);
  delay(5000);
  motor.writeMicroseconds(1060);
  motor2.writeMicroseconds(1060);

  delay(5000);
  
  
}

int i = 0;
bool down = false;
int pwm = 0;
int currentPwm = 0;

String readstring = "";
String currentReadString = "";

int servoPos1 = 0;
int servoPos2 = 0;
void loop() {
  // put your main code here, to run repeatedly:
  while(Serial.available()){
    if(Serial.available()){
      char c = Serial.read();

      // if the character is not a dash
      if(c != '-'){
        readstring += c;
      }
      else{
        currentReadString = readstring;
        readstring = "";
        break;
      }
     
    }

  }

  // read currentReadString:

 String pwmString = "";
 char pwmDesignation;

 for (int i = 0; i < currentReadString.length(); i++) {
  if(i == 0) pwmDesignation = currentReadString.charAt(i);
  else pwmString += currentReadString.charAt(i);
 }
 
      
 pwm = pwmString.toInt();
 if(pwm >= 0 && pwm < 256){
    currentPwm = pwm;
 }

  
 int servoPos = map(currentPwm, 0, 255, 0, 180);
    
  //  dcMotor->setSpeed(sped);
  //  dcMotor->run(FORWARD);

  motorSpeed = map(currentPwm, 0, 255, 1060, 1700);

  if(pwmDesignation == 'a')motor.writeMicroseconds(motorSpeed);
  else if(pwmDesignation == 'b')motor2.writeMicroseconds(motorSpeed);
  else if(pwmDesignation == 'c'){
    motor.writeMicroseconds(motorSpeed);
    motor2.writeMicroseconds(motorSpeed);
  }
  else if(pwmDesignation == 'd')servo.write(servoPos);
  else if(pwmDesignation == 'e')servo2.write(servoPos);
  else if(pwmDesignation == 'f'){
    servo.write(servoPos);
    servo2.write(servoPos);
  }
  else if(pwmDesignation == 'g'){
    servo.write(servoPos);
    servo2.write(180-servoPos);
  }
  
  
  //  Serial.println(motorSpeed);
    
}
