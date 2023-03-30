#ifndef Encoder_h
#define Encoder_h

#include "Arduino.h"

#define MAX_NUM_ENCODERS 2

class Encoders{
  public:  
    Encoders(byte pinA, byte pinB);
    static void interruptEncoder1(){
      if(Encoders::_instances[0] != NULL)
      Encoders::_instances[0]->encoderCount();
    }
    static void interruptEncoder2(){
      if(Encoders::_instances[1] != NULL)
      Encoders::_instances[1]->encoderCount();
    }

    
    void encoderCount();
    short getEncoderCount();
    void setEncoderCount(short);
    short getEncoderErrorCount();
    static Encoders *_instances[MAX_NUM_ENCODERS];
    
  private:
    static uint8_t _whichEncoder;
    uint8_t _encoderPINA;
    uint8_t _encoderPINB;
    volatile short _encoderCount = 0;
    volatile short _lastEncoded = 0;
    volatile short _encoderErrors = 0;
};

#endif
