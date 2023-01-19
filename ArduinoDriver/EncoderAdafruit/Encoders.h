#define ENCODER_LEFT_A   2
#define ENCODER_LEFT_B   3

#define ENCODER_RIGHT_A   18
#define ENCODER_RIGHT_B   19

#define ENCODER_MID_A   20
#define ENCODER_MID_B   21 


volatile long cumPos_left = 0;
volatile long cumPos_right = 0;
volatile long cumPos_mid = 0;

volatile byte state_left; // used to store the prior and current state

volatile int posChange_left = 0;
volatile int posChange_right = 0;
volatile int posChange_mid = 0;


volatile bool motordir_left = true;
volatile bool motordir_right = true;
volatile bool motordir_mid = true;

volatile float leftMotorDegrees = 0.0f;
volatile float rightMotorDegrees = 0.0f;
volatile float midMotorDegrees = 0.0f;


void interruptA_LEFT() {
  // ai0 is activated if DigitalPin nr 2 is going from LOW to HIGH
  // Check pin 3 to determine the direction
  if(digitalRead(ENCODER_LEFT_B)==LOW) {
    cumPos_left++;
  }else{
    cumPos_left--;
  }
  Serial.println(cumPos_left);
}
   
void interruptB_LEFT() {
  // ai0 is activated if DigitalPin nr 3 is going from LOW to HIGH
  // Check with pin 2 to determine the direction
  if(digitalRead(ENCODER_LEFT_A)==LOW) {
    cumPos_left--;
  }else{
    cumPos_left++;
  }
  Serial.println(cumPos_left);
}








void interruptA_RIGHT() {
  if(digitalRead(ENCODER_RIGHT_B) == LOW){
    motordir_right = false;
  }
  else motordir_right = true;

 // Serial.println(cumPos);

  if(!motordir_right){
    cumPos_right += 1;
    posChange_right += 1;
  }
  else {
    cumPos_right -= 1;
    posChange_right -= 1;
  }
}

void interruptA_MID() {
  if(digitalRead(ENCODER_MID_B) == LOW){
    motordir_mid = false;
  }
  else motordir_mid = true;

 // Serial.println(cumPos);

  if(!motordir_mid){
    cumPos_mid += 1;
    posChange_mid += 1;
  }
  else {
    cumPos_mid -= 1;
    posChange_mid -= 1;
  }
}


void SetupEncoders(){

  // currently testing without code block
  if(false){
    // set up left motor encoders
    pinMode(ENCODER_LEFT_A, INPUT_PULLUP);
    pinMode(ENCODER_LEFT_B, INPUT_PULLUP);

   // setupEncoderLeft();
    attachInterrupt(digitalPinToInterrupt(ENCODER_LEFT_A), interruptA_LEFT, RISING );
    attachInterrupt(digitalPinToInterrupt(ENCODER_LEFT_B), interruptB_LEFT, RISING );

    pinMode(ENCODER_RIGHT_A, INPUT_PULLUP);
    pinMode(ENCODER_RIGHT_B, INPUT_PULLUP);
    attachInterrupt(digitalPinToInterrupt(ENCODER_RIGHT_A), interruptA_RIGHT, RISING );

    pinMode(ENCODER_MID_A, INPUT_PULLUP);
    pinMode(ENCODER_MID_B, INPUT_PULLUP);
    attachInterrupt(digitalPinToInterrupt(ENCODER_MID_A), interruptA_MID, RISING );

  }
}
