

// Brushless motor controller class.
// Interfaces with physical brushless esc to allow for smooth changing of the brushless motors
#include <Servo.h>

class BrushlessMotorController_c {

    
  public:

    Servo brushlessMotor; 
    bool serialDebug = true;
    bool doneCommand = true;
    unsigned long previousTime = micros();
    unsigned long currentTime = micros();

    float PWM_Change_Per_Second = 50;

    float PWM_Target = 0;
    float PWM_Current = 70;
    float PWM_Last = 0;
    float PWM_interpolation_time = 1.0f;
    double commandTime = 0;
    BrushlessMotorController_c() {
        
    } 

    void ForceWriteMicroSeconds(int ms){
      brushlessMotor.writeMicroseconds(ms);
    }

    void ForceWritePwm(int pwm){
      brushlessMotor.write(pwm);
      PWM_Current = pwm;
    }



    void ReceiveCommand(float targetPWM, float interp_time){
      PWM_Target = targetPWM;
      PWM_Last = PWM_Current;
      PWM_interpolation_time = interp_time;
      Serial.println(" ReceiveCommand");
      Serial.println(PWM_Target);
      Serial.println(PWM_Last);
      Serial.println(PWM_Current);
      commandTime = micros();
      doneCommand = false;
    }
    
    void SetUpMotor(int pwmPin){
      brushlessMotor.attach(pwmPin);
      if(serialDebug)Serial.println(" Brushless Motor set up");
      if(serialDebug)Serial.println(pwmPin);
    }


    

    
    void UpdateLoop() {
        // calculate new degrees
        currentTime = micros();
        if(!doneCommand){
          if(currentTime - commandTime > PWM_interpolation_time* 1.3 * 1000000) doneCommand = true;
          double timeDifference = (float)(currentTime - previousTime)/1000000;

          // calculate how much we need to change the pwm in this time difference
          PWM_Change_Per_Second = (PWM_Target - PWM_Last) / PWM_interpolation_time;

          PWM_Current += PWM_Change_Per_Second * timeDifference;

          // error checking safety
          if(PWM_Current > 180 && PWM_Target > 180)PWM_Current = 180;
          else if (PWM_Current > 180 && PWM_Target < 180)PWM_Current = PWM_Target;
          
          else if(PWM_Current < 50 && PWM_Target < 50) PWM_Current = 50;
          else if(PWM_Current < 50 && PWM_Target > 50) PWM_Current = PWM_Target;
          if(abs(PWM_Current - PWM_Target)< 1){
            Serial.println("Done");
            PWM_Current = PWM_Target;
            doneCommand = true;
          }
          brushlessMotor.write(PWM_Current);
          
        }
        previousTime = micros();
    }
};
