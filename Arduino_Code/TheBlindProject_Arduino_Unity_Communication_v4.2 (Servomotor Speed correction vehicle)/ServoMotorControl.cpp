#include "ServoMotorControl.h"

Adafruit_PWMServoDriver servoMotor_pwmDriverBoard_1 = Adafruit_PWMServoDriver(SERVO_MOTOR_DRIVER_BOARD_1_ADDR);

int servoMotor_currentPositions [servoMotor_motorCount] = {0, 0, 0, 0, 0, 90};
int servoMotor_targetPositions [servoMotor_motorCount] = {0, 0, 0, 0, 0, 90};

unsigned long servoMotor_previousMillis = 0;
unsigned long servoMotor_currentMillis = 0;

unsigned long servoMotor_previousMillis_ServoVehicle = 0;
unsigned long servoMotor_currentMillis_ServoVehicle = 0;


void servoMotor_setup(){
  servoMotor_pwmDriverBoard_1.begin();
  servoMotor_pwmDriverBoard_1.setPWMFreq(SERVO_MOTOR_FREQUENCY);

  servoMotor_moveToPosition(Servo_ObstacleLeft, 0);
  delay(100);
  servoMotor_moveToPosition(Servo_ObstacleRight, 0);
  delay(100);
  servoMotor_moveToPosition(Servo_ExitDirectionLeft, 0);
  delay(100);
  servoMotor_moveToPosition(Servo_ExitDirectionRight, 0);
  delay(100);
  servoMotor_moveToPosition(Servo_ResonForStop, 0);
  delay(100);
  servoMotor_moveToPosition(Servo_Vehicle, 90);
}


void servoMotor_loop ()
{
  servoMotor_currentMillis = millis();
  servoMotor_currentMillis_ServoVehicle = millis();

  if(servoMotor_currentMillis - servoMotor_previousMillis > SERVO_MOTOR_MOVEMENTSPEED_IN_MS_PER_DEGREE){
    for(int motor = 0; (motor < servoMotor_motorCount-1); motor++){
      if(servoMotor_currentPositions[motor] != servoMotor_targetPositions[motor]){

        if(servoMotor_currentPositions[motor] > servoMotor_targetPositions[motor])
          servoMotor_currentPositions[motor]--;

        if(servoMotor_currentPositions[motor] < servoMotor_targetPositions[motor])
          servoMotor_currentPositions[motor]++;

        servoMotor_moveToPosition(motor, servoMotor_currentPositions[motor]);
        servoMotor_previousMillis = servoMotor_currentMillis;
      }
    }
  }

  if(servoMotor_currentMillis_ServoVehicle - servoMotor_previousMillis_ServoVehicle > SERVO_MOTOR_VEHICLE_MOVEMENTSPEED_IN_MS_PER_DEGREE){

    if(servoMotor_currentPositions[Servo_Vehicle] != servoMotor_targetPositions[Servo_Vehicle]){

      if(servoMotor_currentPositions[Servo_Vehicle] > servoMotor_targetPositions[Servo_Vehicle])
        servoMotor_currentPositions[Servo_Vehicle]--;

      if(servoMotor_currentPositions[Servo_Vehicle] < servoMotor_targetPositions[Servo_Vehicle])
        servoMotor_currentPositions[Servo_Vehicle]++;

      servoMotor_moveToPosition(Servo_Vehicle, servoMotor_currentPositions[Servo_Vehicle]);
      servoMotor_previousMillis_ServoVehicle = servoMotor_currentMillis_ServoVehicle;
    }
    
  }
}


void servoMotor_setPosition(int motorSelection, int positionInDegree){
  servoMotor_targetPositions[motorSelection] = positionInDegree;
}


void servoMotor_moveToPosition(int motorSelection, int positionInDegree){
  // Convert to pulse width
  int pulseWidth = map(positionInDegree, 0, 180, servoMotor_minPulseWidth[motorSelection], servoMotor_maxPulseWidth[motorSelection]);
  servoMotor_pwmDriverBoard_1.setPWM(servoMotor_addr[motorSelection], 0, pulseWidth);
}