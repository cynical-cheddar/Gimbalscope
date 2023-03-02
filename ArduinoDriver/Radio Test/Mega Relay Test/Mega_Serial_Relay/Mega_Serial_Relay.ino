#include "IMU_Handler.h"

//  ==== CODE FOR MEGA TO RELAY SERIAL FROM UNO TO PC =====//
int generate_telemetry_delay_ms = 500;
long target_time = 0;
void setup() {
  delay(100);
  Serial.begin(9600);
  Serial3.begin(9600);
  IMU_Setup();
  while (!Serial3) { delay(10); } // wait until Serial1 console is open, remove if not tethered to computer
  while (!Serial) { delay(10); } // wait until Serial console is open, remove if not tethered to computer
  Serial.println("Begin");
  target_time = millis() + generate_telemetry_delay_ms;
}

void loop() {
  String  message;
  while(Serial3.available() > 0){
    delay(10);
    digitalWrite(LED_BUILTIN, HIGH);
    if (Serial3.available() >0) {
      char c = Serial3.read();  //gets one byte from serial buffer
      message += c; //makes the string readString
    } 
  }
  if(message != "")Serial.println(message);

  if(millis() > target_time){
    target_time = millis() + generate_telemetry_delay_ms;
    // generate random telemetry to send back to computer via Serial3 and radio
    IMU_Loop();
    int x = roll;
    int y = pitch;
    int z = yaw;
    String telemetry = "#" + String(x) + "," + String(y) + "," + String(z) + ",";
    Serial3.println(telemetry);
    Serial.println(telemetry);
  }
  
  

}
