int  MAX_COUNT = 24;
int  statePin  = LOW;
int  count     = 0;
int  count2    = 0;
int  count3    = 0;

byte names[]   = {'c',  'd',  'e',  'f',  'g',  'a',  'b',  'C'};  
int  tones[]   = {1915, 1700, 1519, 1432, 1275, 1136, 1014, 956};

void reproduzir(byte *musica) {
  if (IMP_SOUND == 1)
    for (count = 0; count < strlen(musica); count++) {
      statePin = !statePin;
    
      for (count3 = 0; count3 <= (musica[count * 2] - 48) * 30; count3++) 
      {
        for (count2 = 0; count2 < 8; count2++) 
        {
          if (names[count2] == musica[count*2 + 1]) 
          {
            analogWrite(BUZZER_PIN,500);
            delayMicroseconds(tones[count2]);
          
            analogWrite(BUZZER_PIN, 0);
            delayMicroseconds(tones[count2]);
          } 
        
          if (musica[count * 2 + 1] == 'p') 
          {
            analogWrite(BUZZER_PIN, 0);
            delayMicroseconds(500);
          }
        }
      }
    }
}

void beep (int frequencyInHertz, long timeInMilliseconds)
{
    int x;
    long delayAmount = (long)(1000000/frequencyInHertz);
    long loopTime = (long)((timeInMilliseconds*1000)/(delayAmount*2));
    
    for (x=0;x<loopTime;x++)
    {
        digitalWrite(BUZZER_PIN, HIGH);
        delayMicroseconds(delayAmount);
        
        digitalWrite(BUZZER_PIN, LOW);
        delayMicroseconds(delayAmount);
    }
  
    delay(20);
}

void superMario() {
  int melodia[] = {660, 660, 660, 510, 660, 770, 380};
  int duracaodasnotas[] = {100, 100, 100, 100, 100, 100, 100};
 
  for (int nota = 0; nota < 6; nota++)
  {
    int duracaodanota = duracaodasnotas[nota];
    tone(BUZZER_PIN, melodia[nota], duracaodanota);
    
    int pausadepoisdasnotas[] = {150, 300, 300, 100, 300, 550, 575};
    delay(pausadepoisdasnotas[nota]);
  }
 
  noTone(BUZZER_PIN);
}

