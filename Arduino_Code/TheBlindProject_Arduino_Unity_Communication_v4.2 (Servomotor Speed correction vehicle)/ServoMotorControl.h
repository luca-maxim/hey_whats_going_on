#ifndef SERVO_MOTOR_CONTROL
#define SERVO_MOTOR_CONTROL

#include "Arduino.h"
// Include Wire Library for I2C Communications
#include <Wire.h>
// Include Adafruit PWM Library
#include <Adafruit_PWMServoDriver.h>

#define SERVO_MOTOR_MOVEMENTSPEED_IN_MS_PER_DEGREE 2.5
#define SERVO_MOTOR_VEHICLE_MOVEMENTSPEED_IN_MS_PER_DEGREE 5.0

#define SERVO_MOTOR_FREQUENCY 50
#define SERVO_MOTOR_DRIVER_BOARD_1_ADDR 0x40
extern Adafruit_PWMServoDriver servoMotor_pwmDriverBoard_1;

// Define Motor Outputs on PCA9685 board
enum servoNames 
{
  Servo_ObstacleLeft        = 0,
  Servo_ObstacleRight       = 1,
  Servo_ExitDirectionLeft   = 2,
  Servo_ExitDirectionRight  = 3,
  Servo_ResonForStop        = 4,
  Servo_Vehicle             = 5
};

const int servoMotor_motorCount = 6;
const int servoMotor_addr [servoMotor_motorCount] = {0, 1, 2, 3, 4, 5};
const int servoMotor_minPulseWidth [servoMotor_motorCount] = {94, 103, 118, 122, 94, 102};
const int servoMotor_maxPulseWidth [servoMotor_motorCount] = {494, 509, 518, 522, 505, 499};

extern int servoMotor_currentPositions [servoMotor_motorCount];
extern int servoMotor_targetPositions [servoMotor_motorCount];

extern unsigned long servoMotor_previousMillis;
extern unsigned long servoMotor_currentMillis;

void servoMotor_setup();
void servoMotor_loop ();
void servoMotor_setPosition(int motorSelection, int positionInDegree);
void servoMotor_moveToPosition(int motorSelection, int positionInDegree);

#endif