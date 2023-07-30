#include "ArduinoUnitySerialCommunication.h"
#include "StepperMotorControl.h"
#include "VibrationMotorControl.h"
#include "LinearMotorControl.h"
#include "ServoMotorControl.h"
#include "PushButton.h"



//definition of variables that define the tactile-dispaly board state
obstacleTyps obstaclesAroundCar [2] = 
{ 
  NoObstacle,     //Position: [0] = left,
  NoObstacle      //Position: [1] = right
};
exitDirections exitDirection = NoValue;
stoppingReasons reasonForCurrentStop = NoStop;    //= 0 if car is driving
int upcommingCurveInDegree = 0;                   //Set value to 0 after exiting a curve
int durationUntilArrivalInPercentage = 0;
bool arrivalVerification = false;
bool carIsParking = false;
int relationCarAndDestinationPositionInDegree = 0;
int relationCarAndNorthCompassPointInDegree = 0;



//definition of variables for the communication between the arduino and unity
SerialCommand sCmd;



//definition of handler-function for the unity-commands
void pingHandler (const char *command){
    Serial.println("PONG");
    Serial.println(command);
}

void obstaclesAroundCarHandler(){
  int arg;
  arg = atoi(sCmd.next());
  
  obstaclesAroundCar[0] = (obstacleTyps) (arg/10  % 10);
  obstaclesAroundCar[1] = (obstacleTyps) (arg/1   % 10);

  Serial.println("ACK: [" + String(obstaclesAroundCar[0]) + ", " + String(obstaclesAroundCar[1]) + "]");
  
  if(obstaclesAroundCar[0] != NoObstacle){
    servoMotor_setPosition(Servo_ObstacleLeft, 180);
  }
  else{
    servoMotor_setPosition(Servo_ObstacleLeft, 0);
  }
  
  if(obstaclesAroundCar[1] != NoObstacle){
    servoMotor_setPosition(Servo_ObstacleRight, 180);
  }
  else{
    servoMotor_setPosition(Servo_ObstacleRight, 0);
  }
}

void exitDirectionHandler(){
  exitDirection = (exitDirections) atoi(sCmd.next());
  Serial.println("ACK: " + String(exitDirection));

  if(exitDirection == NoValue){
    servoMotor_setPosition(Servo_ExitDirectionLeft, 0);
    servoMotor_setPosition(Servo_ExitDirectionRight, 0);
  }
  if(exitDirection == ExitLeft){
    servoMotor_setPosition(Servo_ExitDirectionLeft, 180);
  }
  if(exitDirection == ExitRight){
    servoMotor_setPosition(Servo_ExitDirectionRight, 180);
  }
}

void reasonForCurrentStopHandler(){
  reasonForCurrentStop = (stoppingReasons) atoi(sCmd.next());
  Serial.println("ACK: " + String(reasonForCurrentStop));

  if(reasonForCurrentStop != NoStop){
    servoMotor_setPosition(Servo_ResonForStop, 180);
  }
  else{
    servoMotor_setPosition(Servo_ResonForStop, 0);
  }
}

void upcommingCurveHandler(){
  upcommingCurveInDegree = atoi(sCmd.next());
  Serial.println("ACK: " + String(upcommingCurveInDegree));
  
  //von Unity:  Linkskurve = 90°; Geradeaus = 0°; Rechtkurve = -90°
  //Ardunio:  Linkskurve = 180°; Geradeaus = 90°; Rechtkurve = 0°
  upcommingCurveInDegree += 90;
  servoMotor_setPosition(Servo_Vehicle, upcommingCurveInDegree);
}

void durationUntilArrivalHandler(){
  durationUntilArrivalInPercentage = atoi(sCmd.next());
  Serial.println("ACK: " + String(durationUntilArrivalInPercentage));

  linearMotor_setTargetPosition(durationUntilArrivalInPercentage);
}

void arrivalVerificationHandler(){
  const char* compareString = "True";
  if(strcmp(sCmd.next(), compareString) == 0){
    arrivalVerification = true;
  }
  else{
    arrivalVerification = false;
  }

  Serial.println("ACK: " + String(arrivalVerification));

  if(arrivalVerification == true){
    
  }
}


void relationCarAndDestinationPositionHandler(){
  relationCarAndDestinationPositionInDegree = atoi(sCmd.next());
  Serial.println("ACK: " + String(relationCarAndDestinationPositionInDegree));

  //von Unity:  Ziel Links = 90°; Geradeaus = 0°; Ziel Rechts = -90°
  //Ardunio:  Linkskurve = 180°; Geradeaus = 90°; Rechtkurve = 0°
  relationCarAndDestinationPositionInDegree += 90;
  servoMotor_setPosition(Servo_Vehicle, relationCarAndDestinationPositionInDegree);
}


void getButtonStatesHandler(){
  if(latestRegisteredButtonPress == NoUserInput){
    Serial.println("ACK");
  }
  else{
    Serial.println(String(latestRegisteredButtonPress));
    latestRegisteredButtonPress = NoUserInput;
  }
}
