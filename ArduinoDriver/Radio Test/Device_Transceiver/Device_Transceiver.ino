// rf69 demo tx rx.pde
// -*- mode: C++ -*-
// Example sketch showing how to create a simple messageing client
// with the RH_RF69 class. RH_RF69 class does not provide for addressing or
// reliability, so you should only use RH_RF69  if you do not need the higher
// level messaging abilities.
// It is designed to work with the other example rf69_server.
// Demonstrates the use of AES encryption, setting the frequency and modem 
// configuration

#include <SPI.h>
#include <RH_RF69.h>

/************ Radio Setup ***************/

// Change to 434.0 or other frequency, must match RX's freq!
#define RF69_FREQ 915.0



  #define RFM69_INT     2  // 
  #define RFM69_CS      7  //
  #define RFM69_RST     3  // "A"
  #define LED           13



bool pc_serial = true;
// Singleton instance of the radio driver
RH_RF69 rf69(RFM69_CS, RFM69_INT);

int16_t packetnum = 0;  // packet counter, we increment per xmission

void setup() 
{
  pinMode(12, OUTPUT);
  digitalWrite(12, LOW);
  delay(2000);
  if(pc_serial)Serial.begin(9600);
  Serial1.begin(9600);
  while (!Serial1) { delay(10); } // wait until Serial1 console is open, remove if not tethered to computer
  //if(pc_serial) while (!Serial) { delay(10); } // wait until Serial1 console is open, remove if not tethered to computer
  delay(200);
  if(pc_serial) Serial.println("Begin");
  

  pinMode(LED, OUTPUT);     
  pinMode(RFM69_RST, OUTPUT);
  digitalWrite(RFM69_RST, LOW);

  if(pc_serial) Serial.println("Feather RFM69 RX Test!");
  if(pc_serial) Serial.println();

  // manual reset
  digitalWrite(RFM69_RST, HIGH);
  delay(100);
  digitalWrite(RFM69_RST, LOW);
  delay(100);
  
  
  if (!rf69.init()) {
    if(pc_serial) Serial.println("RFM69 radio init failed");
    while (1);
  }
  
  if(pc_serial)Serial.println("RFM69 radio init OK!");
  
  // Defaults after init are 434.0MHz, modulation GFSK_Rb250Fd250, +13dbM (for low power module)
  // No encryption
  if (!rf69.setFrequency(RF69_FREQ)) {
    if(pc_serial) Serial.println("setFrequency failed");
  }

  // If you are using a high power RF69 eg RFM69HW, you *must* set a Tx power with the
  // ishighpowermodule flag set like this:
  rf69.setTxPower(20, true);  // range from 14-20 for power, 2nd arg must be true for 69HCW

  // The encryption key has to be the same as the one in the server
  uint8_t key[] = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};
  rf69.setEncryptionKey(key);
  
  pinMode(LED, OUTPUT);

  if(pc_serial) {Serial.print("RFM69 radio @");  Serial.print((int)RF69_FREQ);  Serial.println(" MHz");}
  
}

// send telemetry packet without timeout
void SendTelemetryPacket(String telemetryPacket){
  if(pc_serial){Serial.print("Sending "); Serial.println(telemetryPacket);}
  char telemetryPacketArray[31];
  telemetryPacket.toCharArray(telemetryPacketArray, 30);
  // Send a message!
  rf69.send((uint8_t *)telemetryPacketArray, strlen(telemetryPacketArray));
}

void loop() {
 delay(10);

 // receive commands here
 // ============ RX ================ //
 if (rf69.available()) {
    
    //Serial.println("y");
    // Should be a message for us now   
    uint8_t buf[RH_RF69_MAX_MESSAGE_LEN];
    uint8_t len = sizeof(buf);
    if (rf69.recv(buf, &len)) {
      if (!len) return;
      buf[len] = 0;
      // print results to mega
      Serial1.println((char*)buf);
      if(pc_serial)Serial.println((char*)buf);
      String myMessage = (char*)buf;
      if(myMessage[0] == '3'){
        // emergency stop
        if(pc_serial)Serial.println("emergency stop");
        digitalWrite(12, HIGH);
        
      }
      else if(myMessage[0] == '4'){
        if(pc_serial)Serial.println("resume");
        digitalWrite(12, LOW);
      }
      else {
        // Send a reply!
        uint8_t data[] = "acknowledged";
        rf69.send(data, sizeof(data));
        rf69.waitPacketSent();
        Blink(LED, 40, 3); //blink LED 3 times, 40ms between blinks
      }
    } else {
      //Serial1.println("Receive failed");
    }
  }

  // ================= TX ==============//

  // read Serial1 for telemetry data.
  // Send telemetry data (without acknowledgement) to dongle over radio
  // if serial has valid telemetry data (starts with # character):
    // telemetry in #xxx,yyy,zzz format - euler rotations as integer
    // SendTelemetryPacket(String telemetryPacket)
  
  if(Serial1.available()){
   
    String  message;
    while(Serial1.available() > 0){
      delay(5);
      digitalWrite(LED_BUILTIN, HIGH);
      if (Serial1.available() >0) {
        char c = Serial1.read();  //gets one byte from serial buffer
        message += c; //makes the string readString
      }
    }
    // telemetry format in euler angles #xxx,yyy,zzz
    if(message != ""){
      if(message[0] == '#') {
        SendTelemetryPacket(message);
        //Serial.println("Valid message" + message);
      }
      else{
        //Serial.println("Invalid message " + message);
      }
    }
    }
    
    
  
}


void Blink(byte PIN, byte DELAY_MS, byte loops) {
  for (byte i=0; i<loops; i++)  {
    digitalWrite(PIN,HIGH);
    delay(DELAY_MS);
    digitalWrite(PIN,LOW);
    delay(DELAY_MS);
  }
}
