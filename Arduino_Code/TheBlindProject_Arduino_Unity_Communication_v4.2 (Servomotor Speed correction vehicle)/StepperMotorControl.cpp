#include "StepperMotorControl.h"

Stepper myStepper(stepperMotor_stepsPerRevolution, STEPPER_MOTOR_IN1_PIN, STEPPER_MOTOR_IN3_PIN, STEPPER_MOTOR_IN2_PIN, STEPPER_MOTOR_IN4_PIN); //In4, In2, In3, In1 because our Motor moves revered
ezButton stepperMotor_limitSwitch(STEPPER_LIMIT_SWITCH_PIN);
int stepperMotor_currentPosition = 0;
//int stepperMotor_steppsToTurn = 0;

void stepperMotor_setup(){
  myStepper.setSpeed(stepperMotor_ratedSpeed);
  stepperMotor_limitSwitch.setDebounceTime(50);
  stepperMotor_limitSwitch.loop();

  //Spin Stepper Motor clockwise until LimitSwitch = 0 (activ low)
  while(stepperMotor_limitSwitch.getState() == HIGH){
    myStepper.step(1);
    stepperMotor_limitSwitch.loop();
  }
}

void stepperMotor_loop(){
  stepperMotor_limitSwitch.loop();
  /*
  if(stepperMotor_steppsToTurn != 0){
    if(stepperMotor_steppsToTurn > 0){
      myStepper.step(17);
      stepperMotor_steppsToTurn = stepperMotor_steppsToTurn - 17;
    }
    else{
      myStepper.step(-17);
      stepperMotor_steppsToTurn = stepperMotor_steppsToTurn + 17;
    }
  }
  */
}


void stepperMotor_setPosition(int positionInDegree){
  if(stepperMotor_currentPosition != positionInDegree){

      int degreesToTurn = positionInDegree - stepperMotor_currentPosition;

      //Changes rotation direction in the desired direction
      if(degreesToTurn > 180) degreesToTurn = degreesToTurn - 360;
      if(degreesToTurn < -180) degreesToTurn = degreesToTurn + 360;
      if(stepperMotor_currentPosition == 90 && positionInDegree == 270) degreesToTurn = -180;
      if(stepperMotor_currentPosition == 270 && positionInDegree == 90) degreesToTurn = 180;

      //Values are empirically determined
      int stepsToTurn = (degreesToTurn/3) * 17;

      stepperMotor_currentPosition = positionInDegree;

      myStepper.step(stepsToTurn);
      //stepperMotor_steppsToTurn = stepsToTurn;
  }
  return;
}