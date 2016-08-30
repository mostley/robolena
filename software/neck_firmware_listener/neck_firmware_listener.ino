#include <Servo.h>


#define AXIS0   (2)
#define AXIS1   (3)
#define AXIS2   (4)
#define AXIS3   (5)
#define AXIS4   (6)
#define AXIS5   (7)

Servo servos[6];
int scale[6];
float currentAngle[6];
float targetAngle[6];
float speed = 1.0/500.0;

void setup() {
  Serial.begin(57600);

  servos[0].attach(AXIS0);
  servos[1].attach(AXIS1);
  servos[2].attach(AXIS2);
  servos[3].attach(AXIS3);
  servos[4].attach(AXIS4);
  servos[5].attach(AXIS5);
  scale[0] = -1;
  scale[1] = 1;
  scale[2] = -1;
  scale[3] = 1;
  scale[4] = -1;
  scale[5] = 1;

  currentAngle[0] = 0;
  currentAngle[1] = 0;
  currentAngle[2] = 0;
  currentAngle[3] = 0;
  currentAngle[4] = 0;
  currentAngle[5] = 0;

  targetAngle[0] = 0;
  targetAngle[1] = 0;
  targetAngle[2] = 0;
  targetAngle[3] = 0;
  targetAngle[4] = 0;
  targetAngle[5] = 0;
}

void setAngle(int index, float angle) {
  /*Serial.print("Servo ");
  Serial.print(index);
  Serial.print(" now at ");
  Serial.print(angle);
  Serial.println("\n");*/
  currentAngle[index] = angle;
  
  angle = (angle * scale[index]) * (500.0f/90.0f) + 1500.0f;
  servos[index].writeMicroseconds(angle);
}

void setTarget(int index, float angle) {
  if (angle >= -90 && angle <= 90) {
    targetAngle[index] = angle;
  } else {
    Serial.print("angle ");
    Serial.print(angle);
    Serial.println(" outside of bounds");
  }
}

void loop() {

  if (Serial.available() > 0) {
    setTarget(0, Serial.parseFloat());
    setTarget(1, Serial.parseFloat());
    setTarget(2, Serial.parseFloat());
    setTarget(3, Serial.parseFloat());
    setTarget(4, Serial.parseFloat());
    setTarget(5, Serial.parseFloat());
  }

  for (int i=0; i<6; i++) {
    float delta = targetAngle[i] - currentAngle[i];
    
    if (abs(delta) > 0.1 || abs(delta) < -0.1) {
    
      /*Serial.print("Servo ");
      Serial.print(i);
      Serial.print(" now with delta ");
      Serial.print(delta );
      Serial.print(" and speed ");
      Serial.print(speed);
      Serial.print(" and ");
      Serial.print(targetAngle[i]);
      Serial.print(" - ");
      Serial.print(currentAngle[i]);
      Serial.println("\n");*/
    
      setAngle(i, currentAngle[i] + (delta * speed));
    } else {
      //Serial.print("no change for Servo ");
      //Serial.println(i);
    }
  }
}
