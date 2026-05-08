#include <stdlib.h>
#include <string.h>
#include <stdio.h>

#define keyPad 0
#define lineSize 28;

void restartpointers();
void insertspace();
void insertcell(byte data);

#include "DataFunctions.h"
#include "Display.h"
#include "Functions.h"

void setup() 
{
  if (INT_PLOTAR_TFT == 1) {
    Serial.begin(115200);
    tft.begin(0x9325);
  }
  else 
    inicializar();

  iniciar(INT_PLOTAR_TFT);
  Serial.println("START");
}

void loop() 
{
  if (Serial.available() > 0)
  {
    read();
    delay(10);
    
    if (print() == 0) {
      Serial.println();
      Serial.println("END");
      
      if (INT_PLOTAR_TFT == 0)
        inicioPagina();
    }

    restartpointers();
    free(byteArrayTest);
  }
}

int read() {
    while (!Serial.available());
    
    iniciar(INT_PLOTAR_TFT);

    bool endMessage = false;
    unsigned long lastByteAt = millis();
    
    while (!endMessage && (millis() - lastByteAt < 1000)) {
      while (Serial.available()) {
        byte dataByte = Serial.read();
        lastByteAt = millis();
      
        bytenode *newNode  = (bytenode *) malloc(sizeof(bytenode));
        newNode->data  = dataByte;
        newNode->next  = NULL;
     
        if (byteArrayTest->next == NULL)
          byteArrayTest->next = newNode;
        else
        {
          bytenode *tmp = byteArrayTest->next;
    
          while (tmp->next != NULL)
            tmp = tmp->next;

          tmp->next = newNode;
        }

        Serial.println(dataByte, HEX);

        if (dataByte == 0xFF)
          endMessage = true;

        delay(1);
      }
    }
    
    return 1;
}

int print() {
  int comando = 0;
  byteArrayTest = byteArrayTest->next;
  
  while (byteArrayTest != NULL)
  {
    switch (byteArrayTest->data)
    {
      case 0xE0:
        comando = 1;
        break;
        
      case 0xC1:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Marcar folha");
          marcarColuna();
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;
      
      case 0xC2:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Adiantando coluna");
          adiantarColuna(10);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC3:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Voltando coluna");
          voltarColuna(10);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC4:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Avancando folha");
          adiantarFolha(ALTURA_LINHA);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC5:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Retrocedendo folha");
          retrocederFolha(ALTURA_LINHA);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC6:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Subindo eixo");
          subirEixoZ(ALTURA_PRESSAO / 2);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC7:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Baixando eixo");
          baixarEixoZ(ALTURA_PRESSAO / 2);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC8:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Inicio da pagina");
          inicioPagina();
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;

      case 0xC9:
        if (comando == 1 && Config.init == 0) {
          debugMessage("Localizar pagina e posicionar");
          adiantarFolha(0);
        } else if (Config.init == 1) {
          insertcell(byteArrayTest->data);
        }
        break;
        
      case 0xF0:
      case 0x90:
        if (Config.init == 0 && comando == 0) {
          Config.type = byteArrayTest->data;
          Config.init = 1;

          debugMessage("Iniciando processo de impressao!");
          
          if (INT_PLOTAR_TFT == 0) {
            adiantarFolha(0);
            inicioPagina();
          }
        }
        break;

      case 0xFF:
      case 0xC0:
        if (Config.init == 1)
        {
          if (INT_PLOTAR_TFT == 1) 
          {
            printline(l1, linha);
            printline(l2, linha + 10);
            printline(l3, linha + 20);
          }
          else 
          {
            imprimirLinha(l1->next, 0);
            //imprimirLinha(inverter(l2->next), 1);
            imprimirLinha(l2->next, 0);
            imprimirLinha(l3->next, 0);
            
            adiantarFolha(ALTURA_LINHA * 2);
          }

          restartpointers();
          linha += 40;
        }
        break;

      case 0x00:
        if (Config.init == 1)
          insertspace(2);
        break;

      default:
        if (Config.init == 1)
          insertcell(byteArrayTest->data);
        break;
    }

    byteArrayTest = byteArrayTest->next;
  }

  return comando;
}

void restartpointers() {
  free(l1);
  l1 = malloc(sizeof(node));
  l1->next = NULL;

  free(l2);
  l2 = malloc(sizeof(node));
  l2->next = NULL;

  free(l3);
  l3 = malloc(sizeof(node));
  l3->next = NULL;
}

void insertspace(int tamanho) {
  insert(l1, 0, tamanho);
  insert(l2, 0, tamanho);
  insert(l3, 0, tamanho);
}

void insertcell(byte data) {
  for (int b = 5; b >= 0; b--) 
  {
    switch (b) {
      case 2:
      case 5:
        insert(l1, getbit(data, b), 1);
        break;

      case 4:
      case 1:
        insert(l2, getbit(data, b), 1);  
        break;

      case 3:
      case 0:
        insert(l3, getbit(data, b), 1);
        if (b == 0) insertspace(1);
        break;
      }       
    }
}
