void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  long duration = pulseIn(6, HIGH);
  Serial.println(duration);

}
