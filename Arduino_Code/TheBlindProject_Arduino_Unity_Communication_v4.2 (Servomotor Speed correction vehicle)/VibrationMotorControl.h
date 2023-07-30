/*
Specs:
Rated voltage: DC 5 V; 
Operating voltage: DC 3 - 5.3 V; 
Rated speed: 9000 rpm minimum; 
Rated current: up to 60 mA; 
Starting current: up to 90 mA; 
Starting voltage: DC 3.7 V; 
Insulation resistance: 10 Mohm;
*/

#ifndef VIBRATION_MOTOR_CONTROL
#define VIBRATION_MOTOR_CONTROL

#include "Arduino.h"

const int vibrationMotor_motorCount = 1;
const int vibrationMotor_Pins [vibrationMotor_motorCount] = {7};

void vibrationMotor_setup();

//motorSelection = 0 would be the first motor in the array vibrationMotor_Pins. 
//With motorSelection = (vibrationMotor_motorCount + 1) all motors would vibrate simultaneously.
void vibrationMotor_control(int motorSelection, int onDuration, int offDuration, int numberOfRepetiotions);

#endif