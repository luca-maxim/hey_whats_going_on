#include "LinearMotorControl.h"

Stepper myLinearMotor(linearMotor_stepsPerRevolution, LINEAR_MOTOR_A_1B_PIN, LINEAR_MOTOR_A_1A_PIN, LINEAR_MOTOR_B_1B_PIN, LINEAR_MOTOR_B_1A_PIN);
ezButton linearMotor_limitSwitchMin(LINEAR_MOTOR_LIMIT_SWITCH_MIN_PIN);
ezButton linearMotor_limitSwitchMax(LINEAR_MOTOR_LIMIT_SWITCH_MAX_PIN);
unsigned int linearMotor_currentPosition_inStepps = 0;
unsigned int linearMotor_targetPosition_inStepps = 0;
unsigned long previousMillis = 0;
unsigned long currentMillis = 0;

void linearMotor_setup(){
  //myLinearMotor.setSpeed(linearMotor_ratedSpeed);
  linearMotor_limitSwitchMin.setDebounceTime(50);
  linearMotor_limitSwitchMax.setDebounceTime(50);

  //Spin Stepper clockwise (backwards) until LimitSwitchMin = FALSE (activ low)
  
  while(linearMotor_limitSwitchMin.getState() == HIGH){
    myLinearMotor.step(LINEAR_MOTOR_BACKWARDS);
    
    linearMotor_limitSwitchMin.loop();
    linearMotor_limitSwitchMax.loop();
    delay(25);
  }
}

void linearMotor_loop(){
  linearMotor_limitSwitchMin.loop();
  linearMotor_limitSwitchMax.loop();

  //move current position to target position
  //move forward
  if(linearMotor_targetPosition_inStepps > linearMotor_currentPosition_inStepps){

    //Limit-Switches are activ low
    if(linearMotor_limitSwitchMax.getState() == LOW){

    }
    else{
      currentMillis = millis();

      if(currentMillis - previousMillis > LINEAR_MOTOR_MOVEMENTSPEED_MS_PER_STEP){
        myLinearMotor.step(LINEAR_MOTOR_FORWARDS);
        linearMotor_currentPosition_inStepps++;
        previousMillis = currentMillis;
      }
    }
  }
  //move backward
  else if(linearMotor_targetPosition_inStepps < linearMotor_currentPosition_inStepps){

    //Limit-Switches are activ low
    if(linearMotor_limitSwitchMin.getState() == LOW){

    }
    else{
      currentMillis = millis();

      if(currentMillis - previousMillis > LINEAR_MOTOR_MOVEMENTSPEED_MS_PER_STEP){
        myLinearMotor.step(LINEAR_MOTOR_BACKWARDS);
        linearMotor_currentPosition_inStepps--;
        previousMillis = currentMillis;
      }
    }
  }
}

void linearMotor_setTargetPosition(int positionInPercentage){
  linearMotor_targetPosition_inStepps = positionInPercentage * linearMotor_stepsPerPercentage;
}
