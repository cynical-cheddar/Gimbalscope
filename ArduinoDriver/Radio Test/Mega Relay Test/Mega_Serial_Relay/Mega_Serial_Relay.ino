//  ==== CODE FOR MEGA TO RELAY SERIAL FROM UNO TO PC =====//

void setup() {
  delay(1000);
  Serial.begin(9600);
  Serial3.begin(9600);
  Serial.println("Begin");
}

void loop() {
  String  message;
  while(Serial3.available() > 0){
    delay(5);
    digitalWrite(LED_BUILTIN, HIGH);
    if (Serial3.available() >0) {
      char c = Serial3.read();  //gets one byte from serial buffer
      message += c; //makes the string readString
    } 
  }
  if(message != "")Serial.println(message);
  else Serial.println(".");
  

}
