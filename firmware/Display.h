#include "DebugDisplay.h";

void iniciar(int viewer) 
{
  byteArrayTest = malloc(sizeof(bytenode));
  byteArrayTest->next = NULL;
  
  Config.count = 0;
  Config.init = 0;

  linha = 50;
  
  restartpointers();

  if (viewer == 1)
  {
    tft.setRotation(1);
    tft.fillScreen(BLACK);
    tft.setTextColor(WHITE);
    tft.setTextSize(1.9);
  
    tft.setCursor(0, 20);
    tft.println("Braille Printer Connection Debug");
  }
}

void printline(node *ln, int i) 
{
  tft.setCursor(0, i);
  node *tmp = ln->next;
            
  while (tmp != NULL) {
    tft.print(tmp->data == 1 ? "#" : " "); 
    tmp = tmp->next;    
  }

  tft.println("");
}
