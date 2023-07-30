#include "VibrationMotorControl.h"

void vibrationMotor_setup(){
  for(int i = 0; i < vibrationMotor_motorCount; i++){
    pinMode(vibrationMotor_Pins[i], OUTPUT);
  }
}

void vibrationMotor_control(int motorSelection, int onDuration, int offDuration, int numberOfRepetiotions){
  if(motorSelection == (vibrationMotor_motorCount+1)){
    for(int rep = 0; rep < numberOfRepetiotions; rep++){
      for(int motor = 0; motor < vibrationMotor_motorCount; motor++){
        digitalWrite(vibrationMotor_Pins[motor], HIGH);
      }
      delay(onDuration);

      for(int motor = 0; motor < vibrationMotor_motorCount; motor++){
        digitalWrite(vibrationMotor_Pins[motor], HIGH);
      }
      delay(offDuration);
    }
  }
  else{
    for(int rep = 0; rep < numberOfRepetiotions; rep++){
      digitalWrite(vibrationMotor_Pins[motorSelection], HIGH);
      delay(onDuration);
      digitalWrite(vibrationMotor_Pins[motorSelection], LOW);
      delay(offDuration);
    }
  }
}