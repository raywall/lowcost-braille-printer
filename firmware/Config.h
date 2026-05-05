#include <string.h>

#include "Pins.h"

#define STRING_VERSAO_CONFIG __DATE__ "10/02/2017 1712h" __TIME__ 
#define STRING_CONFIG_AUTOR "(Raywall Malheiros de Souza, Braille printer firmware 1.0.0)"
#define DEBUG   1

// Rotinas de debug
#include "Debug.h"

#define MM_NEMA 159  //212
#define MM_BYJ  87.3 //128
 
// Indica a orientação do texto
// 0 - Texto normal para exibição na tela
// 1 - Texto invertido para impressao */
#define INT_PLOTAR_INVERTIDO 0
#define INT_PLOTAR_TFT       0
#define INT_EXIBIR_LITERAL   0

// Definicoes de procedimentos de impressão
#define IMP_TAMANHO_LINHA 300         // Define a quantidade de caracteres impressos em uma linha
#define IMP_TAMANHO_COLUNA 200        // Define a rotação do motor para mudança de coluna
#define IMP_ESPACO_ENTRE_LINHAS 400   // Define a rotação do motor para mudança de linha
#define IMP_SOUND 1                   // 1 - Ligado , 0 - Desligado

// Configurações de escrita
#define ALTURA_LINHA 20
#define ALTURA_PRESSAO 60

// Reprodução de música
#include "Sounds.h"

typedef struct Item {
  char letra;
  int coordenadas[6];

  struct Item *proximo;
} Lista;

Lista *lista = NULL;

int inicializar() {
  Serial.begin(115200);
  
  pinMode(STP_MOTOR_X, OUTPUT); 
  pinMode(DIR_MOTOR_X, OUTPUT);
  pinMode(LIMITE_AX_X, INPUT);
  
  pinMode(STP_MOTOR_Y, OUTPUT); 
  pinMode(DIR_MOTOR_Y, OUTPUT);
  pinMode(LIMITE_AX_Y, INPUT);
  
  pinMode(STP_MOTOR_Z, OUTPUT); 
  pinMode(DIR_MOTOR_Z, OUTPUT);
  pinMode(LIMITE_AX_Z, INPUT);

  #if MB(CNC_V3)
    pinMode(FEED_HOLD, INPUT);
    pinMode(RESET_ABORT, INPUT);
    pinMode(COOLANT_ENABLE, OUTPUT); 
    pinMode(SPINDLE_ENABLE, OUTPUT);
    pinMode(PRT_ENDSTOP_INI, INPUT);
    pinMode(CYCLE_START_RESUME, INPUT);
    pinMode(SPINDLE_DIRECTION, OUTPUT); 
  #endif

  pinMode(BUZZER_PIN, OUTPUT);
  pinMode(INDUTOR_PRESS, OUTPUT);
  
  pinMode(ENB_MOTOR_X, OUTPUT);
  digitalWrite(ENB_MOTOR_X, LOW);

  pinMode(ENB_MOTOR_Y, OUTPUT);
  digitalWrite(ENB_MOTOR_Y, LOW);

  pinMode(ENB_MOTOR_Z, OUTPUT);
  digitalWrite(ENB_MOTOR_Z, LOW);

  return 1;
}
