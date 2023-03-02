
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_L3GD20_U.h>

Adafruit_L3GD20_Unified gyro = Adafruit_L3GD20_Unified(20);

double GyroErrorX = 0;
double GyroErrorY = 0;
double GyroErrorZ = 0;

double X = 0;
double Y = 0;
double Z = 0;

double lastTime = 0;

void displaySensorDetails(void)
{
  Serial.println("displaySensorDetails");
  sensor_t sensor;
  Serial.println("sensor_t sensor");
  gyro.getSensor(&sensor);
  Serial.println("gyro.getSensor(&sensor)");
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

void setup(void) 
{
  Serial.begin(9600);
  Serial.println("Gyroscope Test"); Serial.println("");
  
  /* Enable auto-ranging */
  gyro.enableAutoRange(true);
  Serial.println("gyro.enableAutoRange(true)"); Serial.println("");
  /* Initialise the sensor */
  if(!gyro.begin())
  {
    /* There was a problem detecting the L3GD20 ... check your connections */
    
    while(1){
      Serial.println("Ooops, no L3GD20 detected ... Check your wiring!");
      delay(1000);
    }
  }
  
  /* Display some basic information on this sensor */
  displaySensorDetails();
  calculate_IMU_error();
}

void loop(void) 
{
  
  /* Get a new sensor event */ 
  sensors_event_t event; 
  gyro.getEvent(&event);
 
  /* Display the results (speed is measured in rad/s) */
  /*Serial.print("X: "); Serial.print(event.gyro.x); Serial.print("  ");
  Serial.print("Y: "); Serial.print(event.gyro.y); Serial.print("  ");
  Serial.print("Z: "); Serial.print(event.gyro.z); Serial.print("  ");
  Serial.println("rad/s ");
  */
  
  double dX = event.gyro.x - GyroErrorX;
  double dY = event.gyro.y - GyroErrorY;
  double dZ = event.gyro.z - GyroErrorZ;

  X += dX * ((micros() - lastTime)/1000000);
  Y += dY * ((micros() - lastTime)/1000000);
  Z += dZ * ((micros() - lastTime)/1000000);
  lastTime = micros();

  Serial.print("X: "); Serial.print(X*57.2958); Serial.print("  ");
  Serial.print("Y: "); Serial.print(Y*57.2958); Serial.print("  ");
  Serial.print("Z: "); Serial.print(Z*57.2958); Serial.print("  ");
  Serial.println("");
}

void calculate_IMU_error() {
  Serial.println("Calculate imu error");
  double GyroX = 0;
  double GyroY = 0;
  double GyroZ = 0;
  int c = 0;
  int samples = 10000;

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
  Serial.println("sampled");
  //Divide the sum by 200 to get the error value
  GyroErrorX = GyroErrorX / samples;
  GyroErrorY = GyroErrorY / samples;
  GyroErrorZ = GyroErrorZ / samples;
  // Print the error values on the Serial Monitor

  Serial.print("GyroErrorX: ");
  Serial.println(GyroErrorX);
  Serial.print("GyroErrorY: ");
  Serial.println(GyroErrorY);
  Serial.print("GyroErrorZ: ");
  Serial.println(GyroErrorZ);
}
