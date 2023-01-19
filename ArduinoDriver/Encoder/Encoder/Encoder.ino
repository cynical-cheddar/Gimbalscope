
   
//#include <Wire.h>
//#include <Encoder.h>
#include <Servo.h>
// Connect to the two encoder outputs!
#define ENCODER_A   2
#define ENCODER_B   3

#define PWM_PIN   11

// These let us convert ticks-to-RPM
#define GEARING     100
#define ENCODERMULT 12



volatile float RPM = 0;
volatile uint32_t lastA = 0;
volatile bool motordir = true;

volatile int rotations = 0;
volatile int cumPos = 0;

volatile float currentDegrees = 0;

float targetRotation = 360.0f;

int pos = 0;


int currentPWM = 90;


Servo esc; 

//Encoder myEnc(ENCODER_A, ENCODER_B);


long positionLeft  = -999;

void interruptA() {
  if(digitalRead(ENCODER_B) == LOW){
    motordir = false;
  }
  else motordir = true;

 // Serial.println(cumPos);

  if(!motordir){
    cumPos += 1;
    rotations += 1;
  }
  else {
    cumPos -= 1;
    rotations -= 1;
  }
  
  

  
}

void setup() {
  Serial.begin(9600);           // set up Serial library at 9600 bps
  pinMode(LED_BUILTIN, OUTPUT);
 // digitalWrite(LED_BUILTIN, LOW);
  pinMode(ENCODER_A, INPUT_PULLUP);
  pinMode(ENCODER_B, INPUT_PULLUP);
 // digitalWrite(ENCODER_A, HIGH);       // turn on pull-up resistor
  attachInterrupt(digitalPinToInterrupt(ENCODER_A), interruptA, RISING );
  esc.attach(11);
  ArmSystem();
  delay(1000);
  //Serial.println("Begun");

}

int i = 0;
bool up = true;



void ArmSystem (){

  //Serial.println("ARM");

  for (pos = 70; pos <= 110; pos += 1) { // goes from 0 degrees to 180 degrees
    // in steps of 1 degree
    esc.write(pos);              // tell servo to go to position in variable 'pos'
    delay(120);                       // waits 15 ms for the servo to reach the position
  }
  
  delay(100);
  
  for (pos = 110; pos >= 70; pos -= 1) { // goes from 0 degrees to 180 degrees
    // in steps of 1 degree
    esc.write(pos);              // tell servo to go to position in variable 'pos'
    delay(120);                       // waits 15 ms for the servo to reach the position
  }
  delay(120);

  esc.write(90);
  currentPWM = 90;
  //Serial.println("Done arming");
}

void SweepLowToHigh(){
    for (pos = 70; pos <= 110; pos += 1) { 
        // in steps of 1 degree
        SetPWM(pos);           
        delay(10);                       
   }
}
void SweepHighToLow(){
    for (pos = 110; pos >= 70; pos -= 1) { // goes from 0 degrees to 180 degrees
        // in steps of 1 degree
        SetPWM(pos);               // tell servo to go to position in variable 'pos'
        delay(10);                       // waits 15 ms for the servo to reach the position
  }
}


void SetPWM(int desiredPWM){
    esc.write(desiredPWM);
    currentPWM = desiredPWM;
}

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
    if(Serial.available()){
      SerialDecoder();
    }

    float deg = ((float)cumPos / (GEARING*ENCODERMULT))* 360;
    currentDegrees = deg;

  
    //Serial.println("serialAvailable");
    digitalWrite(LED_BUILTIN, LOW);
   
   // go to position 90 degrees
   float currentRotation = currentDegrees;
   

   // get current rotation in degrees.
    //Serial.println(currentDegrees);
    if(abs(targetRotation-currentDegrees) < 100){
        SetPWM(90);
    }
    // if target is larger than current, pwm >90
    // if current pwm < 90, do sweep function from 80 to 100, then set pwm to desired >100
    // else set pwm to desired
   else if(targetRotation > currentDegrees){
        if(currentPWM <= 90){
            SweepLowToHigh();
        }
        else{
            SetPWM(105);
        }
   }
    // if target is smaller than current, pwm < 90
    // if current pwm > 90, do sweep function from 100 to 80, then set pwm to desired <80
   else if(targetRotation < currentDegrees){
        if(currentPWM >= 90){
            SweepHighToLow();
        }
        else{
            SetPWM(75);
        }
   }
   
}
