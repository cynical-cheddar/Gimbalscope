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

#define EMERGENCY_PIN 6

#define RFM69_INT     3  // 
#define RFM69_CS      4  //
#define RFM69_RST     2  // "A"
#define LED           13



/* Teensy 3.x w/wing
#define RFM69_RST     9   // "A"
#define RFM69_CS      10   // "B"
#define RFM69_IRQ     4    // "C"
#define RFM69_IRQN    digitalPinToInterrupt(RFM69_IRQ )
*/
 
/* WICED Feather w/wing 
#define RFM69_RST     PA4     // "A"
#define RFM69_CS      PB4     // "B"
#define RFM69_IRQ     PA15    // "C"
#define RFM69_IRQN    RFM69_IRQ
*/

// Singleton instance of the radio driver
RH_RF69 rf69(RFM69_CS, RFM69_INT);

int16_t packetnum = 0;  // packet counter, we increment per xmission

void setup() 
{
  Serial.begin(9600);
  //while (!Serial) { delay(1); } // wait until serial console is open, remove if not tethered to computer
  pinMode(EMERGENCY_PIN, INPUT_PULLUP);
  pinMode(LED, OUTPUT);     
  pinMode(RFM69_RST, OUTPUT);
  digitalWrite(RFM69_RST, LOW);

  Serial.println("Feather RFM69 TX Test!");
  Serial.println();

  // manual reset
  digitalWrite(RFM69_RST, HIGH);
  delay(10);
  digitalWrite(RFM69_RST, LOW);
  delay(10);
  
  if (!rf69.init()) {
    Serial.println("RFM69 radio init failed");
    while (1);
  }
  Serial.println("RFM69 radio init OK!");
  // Defaults after init are 434.0MHz, modulation GFSK_Rb250Fd250, +13dbM (for low power module)
  // No encryption
  if (!rf69.setFrequency(RF69_FREQ)) {
    Serial.println("setFrequency failed");
  }

  // If you are using a high power RF69 eg RFM69HW, you *must* set a Tx power with the
  // ishighpowermodule flag set like this:
  rf69.setTxPower(20, true);  // range from 14-20 for power, 2nd arg must be true for 69HCW

  // The encryption key has to be the same as the one in the server
  uint8_t key[] = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};
  rf69.setEncryptionKey(key);
  
  pinMode(LED, OUTPUT);

  Serial.print("RFM69 radio @");  Serial.print((int)RF69_FREQ);  Serial.println(" MHz");
}
void Blink(byte PIN, byte DELAY_MS, byte loops) {
  for (byte i=0; i<loops; i++)  {
    digitalWrite(PIN,HIGH);
    delay(DELAY_MS);
    digitalWrite(PIN,LOW);
    delay(DELAY_MS);
  }
}
void SendCommandPacket(String radioPacket, int timeout){
  Serial.print("Sending "); Serial.println(radioPacket);
  char radiopacketArray[31];
  radioPacket.toCharArray(radiopacketArray, 30);
  // Send a message!
  rf69.send((uint8_t *)radiopacketArray, strlen(radiopacketArray));
  rf69.waitPacketSent();

  // Now wait for a reply
  uint8_t buf[RH_RF69_MAX_MESSAGE_LEN];
  uint8_t len = sizeof(buf);

  if (rf69.waitAvailableTimeout(timeout))  { 
    // Should be a reply message for us now   
    if (rf69.recv(buf, &len)) {
      Serial.print("acknowledgement : ");
      Serial.println((char*)buf);
      Blink(LED, 50, 3); //blink LED 3 times, 50ms between blinks
    } else {
      Serial.println("Receive failed");
    }
  } else {
    Serial.println("No reply, is another RFM69 listening?");
  }
}

int lastButtonValue = 0;

void loop() {
  delay(10);  



  // EMERGENCY STOP:

  int buttonValue = digitalRead(EMERGENCY_PIN);
  if(lastButtonValue != buttonValue){
     if (buttonValue == LOW){
        Serial.println("button 0");
        // restore operation code
        buttonValue = 0;
        SendCommandPacket("400,000", 500);
     } else {
        Serial.println("button 1");
        //EMERGENCY STOP CODE
        buttonValue = 1;
        SendCommandPacket("300,000", 500);
     }
     lastButtonValue = buttonValue;
     delay(1000);
  }
      // =========== TX ==============//
      // read serial from pc for a command
        // if we have a valid command, send it via radio - wait for acknowledgement
          // SendCommandPacket(command string, timeout)
      String  message;
      while(Serial.available() > 0){
        delay(5);
        digitalWrite(LED_BUILTIN, HIGH);
        if (Serial.available() >0) {
          
          char c = Serial.read();  //gets one byte from serial buffer
          //Serial.print(c);
          message += c; //makes the string readString
        } 
      }
      if(message != ""){
        SendCommandPacket(message, 500);
      }
      
    
      // =========== RX ============= //
      // check attached transceiver for incoming telemetry
      // telemetry should start with a # character
      // for now, let's not bother doing acknowledgements with telemetry
      if(rf69.available()){
        uint8_t buf[RH_RF69_MAX_MESSAGE_LEN];
        uint8_t len = sizeof(buf);
        if (rf69.recv(buf, &len)) {
        if (!len) return;
        buf[len] = 0;
        if(buf[0] == '#')Serial.println((char*)buf);
      
      
    }
  }
}
