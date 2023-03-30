
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_L3GD20_U.h>

Adafruit_L3GD20_Unified gyro = Adafruit_L3GD20_Unified(20);

float GyroErrorX = 0;
float GyroErrorY = 0;
float GyroErrorZ = 0;

float Gyro_X = 0;
float Gyro_Y = 0;
float Gyro_Z = 0;

float lastTime = 0;

void displaySensorDetails()
{
  sensor_t sensor;
  gyro.getSensor(&sensor);
  Serial.println("------------------------------------");
  Serial.print  ("Sensor:       "); Serial.println(sensor.name);
  Serial.print  ("Driver Ver:   "); Serial.println(sensor.version);
  Serial.print  ("Unique ID:    "); Serial.println(sensor.sensor_id);
  Serial.print  ("Max Value:    "); Serial.print(sensor.max_value); Serial.println(" rad/s");
  Serial.print  ("Min Value:    "); Serial.print(sensor.min_value); Serial.println(" rad/s");
  Serial.print  ("Resolution:   "); Serial.print(sensor.resolution); Serial.println(" rad/s");  
  Serial.println("------------------------------------");
  Serial.println("");
  delay(500);
}
void calculate_IMU_error() {

  Gyro_X = 0;
  Gyro_Y = 0;
  Gyro_Z = 0;
  float GyroX = 0;
  float GyroY = 0;
  float GyroZ = 0;
  int c = 0;
  int samples = 300;

  while (c < samples) {
    sensors_event_t event; 
    gyro.getEvent(&event);
    GyroX = event.gyro.x;
    GyroY = event.gyro.y;
    GyroZ = event.gyro.z;
    // Sum all readings
    GyroErrorX = GyroErrorX + (GyroX );
    GyroErrorY = GyroErrorY + (GyroY );
    GyroErrorZ = GyroErrorZ + (GyroZ );
    c++;
  }
  //Divide the sum by samples to get the error value
  GyroErrorX = (GyroErrorX / samples) * 1.0;
  GyroErrorY = (GyroErrorY / samples) * 1.0;
  GyroErrorZ = (GyroErrorZ / samples) *1.0;
  // Print the error values on the Serial Monitor

  Serial.print("GyroErrorX: ");
  Serial.println(GyroErrorX);
  Serial.print("GyroErrorY: ");
  Serial.println(GyroErrorY);
  Serial.print("GyroErrorZ: ");
  Serial.println(GyroErrorZ);
}
void SetupGyroscope() 
{
  /* Enable auto-ranging */
  gyro.enableAutoRange(true);
  
  /* Initialise the sensor */
  if(!gyro.begin())
  {
    Serial.println("Ooops, no L3GD20 detected ... Check wiring!");
    while(1);
  }
  
  /* Display some basic information on this sensor */
  displaySensorDetails();
  calculate_IMU_error();
}

void GyroscopeUpdateLoop() 
{
  
  /* Get a new sensor event */ 
  sensors_event_t event; 
  gyro.getEvent(&event);
  
  float dX = event.gyro.x - GyroErrorX;
  float dY = event.gyro.y - GyroErrorY;
  float dZ = event.gyro.z - GyroErrorZ;

  
  // angles are measured in radians 

  // z is the interesting value for us
  
  if(abs(dX) > 0.05)Gyro_X += dX * ((micros() - lastTime)/1000000);
  if(abs(dY) > 0.25)Gyro_Y += dY * ((micros() - lastTime)/1000000);
  if(abs(dZ) > 0.25)Gyro_Z += dZ * ((micros() - lastTime)/1000000);
  lastTime = micros();

  /*
  Serial.print("X: "); Serial.print(X*57.2958); Serial.print("  ");
  Serial.print("Y: "); Serial.print(Y*57.2958); Serial.print("  ");
  Serial.print("Z: "); Serial.print(Z*57.2958); Serial.print("  ");
  Serial.println("");
  */
}
