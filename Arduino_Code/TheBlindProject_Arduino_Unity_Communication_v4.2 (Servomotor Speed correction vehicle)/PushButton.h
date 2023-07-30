#ifndef PUSH_BUTTON
#define PUSH_BUTTON

#include "Arduino.h"
#include <ezButton.h>

#define BUTTON_PIN_OBSTACLE_LEFT          2
#define BUTTON_PIN_OBSTACLE_RIGHT         3
#define BUTTON_PIN_EXIT_DIRECTION_LEFT    4
#define BUTTON_PIN_EXIT_DIRECTION_RIGHT   5
#define BUTTON_PIN_REASON_FOR_STOP        6
#define BUTTON_PIN_PROGRESS_BAR           7

enum arduinoButtons
{
  NoUserInput             = 0,
  ObstacleLeft            = 1,
  ObstacleRight           = 2,
  ExitDirectionLeft       = 3,
  ExitDirectionRight      = 4,
  ReasonForCurrentStop    = 5,
  DestinationSlider       = 6
};

extern ezButton arduinoButtonsArray [];

extern arduinoButtons latestRegisteredButtonPress;

void pushButton_setup();
void pushButton_loop();

#endif