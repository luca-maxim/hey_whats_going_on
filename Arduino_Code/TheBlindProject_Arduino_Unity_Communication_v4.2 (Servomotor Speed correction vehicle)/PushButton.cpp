#include "PushButton.h"


arduinoButtons latestRegisteredButtonPress = NoUserInput;


ezButton arduinoButtonsArray [] = {
  ezButton(BUTTON_PIN_OBSTACLE_LEFT),
  ezButton(BUTTON_PIN_OBSTACLE_RIGHT),
  ezButton(BUTTON_PIN_EXIT_DIRECTION_LEFT),
  ezButton(BUTTON_PIN_EXIT_DIRECTION_RIGHT),
  ezButton(BUTTON_PIN_REASON_FOR_STOP),
  ezButton(BUTTON_PIN_PROGRESS_BAR)
};


void pushButton_setup(){
  for(int i = 0; i < (int)(sizeof(arduinoButtonsArray)/sizeof(arduinoButtonsArray[0])); i++){
    arduinoButtonsArray[i].setDebounceTime(50);
  }
}


void pushButton_loop(){
  for(int i = 0; i < (int)(sizeof(arduinoButtonsArray)/sizeof(arduinoButtonsArray[0])); i++){
    arduinoButtonsArray[i].loop();

    if(arduinoButtonsArray[i].isPressed()){
      latestRegisteredButtonPress = arduinoButtons(i+1);
    }
  }
}