/*
Specs:
Operating voltage: 5 V DC; 
Rated speed: 10-15 rpm; 
Rated current: up to 800 mA; 
*/
#ifndef STEPPER_MOTOR_CONTROL
#define STEPPER_MOTOR_CONTROL

#include "Arduino.h"
#include <Stepper.h>
#include <ezButton.h>

#define STEPPER_MOTOR_IN1_PIN A0
#define STEPPER_MOTOR_IN2_PIN A1
#define STEPPER_MOTOR_IN3_PIN A2
#define STEPPER_MOTOR_IN4_PIN A3
#define STEPPER_LIMIT_SWITCH_PIN 12

const int stepperMotor_stepsPerRevolution = 2048;
const int stepperMotor_ratedSpeed = 10;

extern Stepper myStepper;
extern int stepperMotor_currentPosition;
//extern int stepperMotor_steppsToTurn;
extern ezButton stepperMotor_limitSwitch;


void stepperMotor_setup();
void stepperMotor_loop();
void stepperMotor_setPosition(int positionInDegree); // 0° = straight; 90° = right; 180° = backward; 270° = left

#endif