#ifndef ARDUINO_UNITY_SERIAL_COMMUNICATION
#define ARDUINO_UNITY_SERIAL_COMMUNICATION

#include "Arduino.h"
#include <SoftwareSerial.h>
#include <SerialCommand.h>



//declarations of enumerations
enum obstacleTyps 
{
  NoObstacle                  = 0,    //Values: 0 = no obstacle
  MovingSameDirection         = 1,    //Values: 1 = Moving object that goes in the same direction
  MovingOppositeDirection     = 2,    //Values: 2 = Moving object that goes in the opposite direction
  OtherObstacle               = 3     //Values: 3 = e.g. a stationary object like a tree or a parking car
};

enum exitDirections
{
  NoValue     = 0,    //Values: 0 = car is driving
  ExitLeft    = 1,
  ExitRight   = 2
};

enum stoppingReasons
{
  NoStop              = 0,
  TrafficLight        = 1,
  TrafficJam          = 2,
  StopSign            = 3,
  PedestrianCrossing  = 4
};



//daclaration of variables that define the tactile-dispaly board state
extern obstacleTyps obstaclesAroundCar [2];
extern stoppingReasons reasonForCurrentStop;
extern int upcommingCurveInDegree;
extern int durationUntilArrivalInPercentage;
extern bool arrivalVerification;
extern bool carIsParking;
extern int relationCarAndDestinationPositionInDegree;
extern int relationCarAndNorthCompassPointInDegree;

//declaration of variables for the communication between the arduino and unity
extern SerialCommand sCmd;


//declaration of handler-function for the unity-commands
void pingHandler (const char *command);

void obstaclesAroundCarHandler();
void exitDirectionHandler();
void reasonForCurrentStopHandler();
void upcommingCurveHandler();
void durationUntilArrivalHandler();
void arrivalVerificationHandler();
void relationCarAndDestinationPositionHandler();

void getButtonStatesHandler();


#endif