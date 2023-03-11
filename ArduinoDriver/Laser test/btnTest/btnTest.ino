

const int buttonPin = 42;  // the number of the pushbutton pin
const int buttonPinTwo = 43;  // the number of the pushbutton pin
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
void setup() {
  // initialize the LED pin as an output:

  // initialize the pushbutton pin as an input:
  pinMode(buttonPin, INPUT);
  pinMode(buttonPinTwo, INPUT);
  pinMode(A3,OUTPUT);
  Serial.begin(9600);
}



void InputControl(){
    // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);

  buttonStateTwo = digitalRead(buttonPinTwo);

  if(triggerHeld){
    triggerHeldDurationMillis = millis() - triggerHeldStart;
    if(triggerHeldDurationMillis > triggerHoldDurationThreshold && !noTriggerActionUntilRelease){
      if(buttonStateTwo == LOW){
        Serial.println("select target");
      }
      else if(buttonStateTwo == HIGH){
        Serial.println("calibrate");
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
      
    // release
    } else {
      if(noTriggerActionUntilRelease == false){
        Serial.println("cue");
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



void loop() {
  InputControl();
  delay(10);
}
