/*
Specs:
Motor type: 2-phase 4-core
Drive Voltage: 4-9V / 100-500mA
Thread spindle length: approx. 90 mm.
Slider: approx. 80 mm.
Motor diameter: approx. 15 mm.
Thread spindle diameter: approx. 3 mm.
Pitch of threaded spindle: 0.5 mm.
Step angle: 18°.
Phase resistance: 15.5 ohms
Total size: approx. 15 x 105 mm 
Blue A +
Black A-
Red B+
Yellow B-

Step angle: 18°/step
steps per revolution = 20;
Step lenght: 0.25 mm
Rated speed: 25mm per second
Rated speed max: 3000 rpm
*/

#ifndef LINEAR_MOTOR_CONTROL
#define LINEAR_MOTOR_CONTROL

#include "Arduino.h"
#include <Stepper.h>
#include <ezButton.h>

#define LINEAR_MOTOR_A_1B_PIN 8
#define LINEAR_MOTOR_A_1A_PIN 9
#define LINEAR_MOTOR_B_1B_PIN 10
#define LINEAR_MOTOR_B_1A_PIN 11
#define LINEAR_MOTOR_LIMIT_SWITCH_MIN_PIN A0
#define LINEAR_MOTOR_LIMIT_SWITCH_MAX_PIN A1

#define LINEAR_MOTOR_FORWARDS (1)
#define LINEAR_MOTOR_BACKWARDS (-1)
#define LINEAR_MOTOR_MOVEMENTSPEED_MS_PER_STEP 25

const int linearMotor_stepsPerRevolution = 20;
const float linearMotor_totalTravelLenghtInMillimeter = 70;
const float linearMotor_stepsLengthInMillimeter = 0.025;
const int linearMotor_stepsPerPercentage = (linearMotor_totalTravelLenghtInMillimeter/100)/linearMotor_stepsLengthInMillimeter;

extern Stepper myLinearMotor;
extern ezButton linearMotor_limitSwitchMin;
extern ezButton linearMotor_limitSwitchMax;

extern unsigned int linearMotor_currentPosition_inStepps; //measured from startpoint(linearMotor_limitSwitchMin) = 0 Stepps
extern unsigned int linearMotor_targetPosition_inStepps;
extern unsigned long previousMillis;
extern unsigned long currentMillis;

void linearMotor_setup();
void linearMotor_loop();
void linearMotor_setTargetPosition(int positionInPercentage);

#endif