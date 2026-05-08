int inicioPagina();
int marcarColuna();
int adiantarColuna(int cols);
int voltarColuna(int cols);
int adiantarFolha(int t);
int retrocederFolha(int t);
int imprimirLinha(node *ln);
int baixarEixoZ(int altura);
int subirEixoZ(int altura);
node* inverter(node *ref);

#include "Config.h"

// ------------------------------------------------------------------------
// @Rotinas de impressão
// ------------------------------------------------------------------------
/* 
 *  Envia a cabeça de impressão para o inicio da folha - 162
 */
int inicioPagina() {
  if (digitalRead(PRT_ENDSTOP_INI) != 0) 
  {   
    digitalWrite(DIR_MOTOR_X, HIGH); 

    while (digitalRead(PRT_ENDSTOP_INI) != 0) {
      digitalWrite(STP_MOTOR_X, HIGH);
      delayMicroseconds(40);
      digitalWrite(STP_MOTOR_X, LOW);
      delayMicroseconds(40);
    }
  }

  delay(10);
  return 1;
}

/*
 * Envia a cabeça de impressão para uma determinada posição a frente
 */
int adiantarColuna(int cols) {
  digitalWrite(DIR_MOTOR_X, LOW);

  for (int n = 0; n < (cols * MM_NEMA); n++) {
    digitalWrite(STP_MOTOR_X, HIGH);
    delayMicroseconds(40);
    digitalWrite(STP_MOTOR_X, LOW);
    delayMicroseconds(40);
  }

  delay(10);
  return 1;
}

/*
 * Envia a cabeça de impressão para uma determinada posição para tras
 */
int voltarColuna(int cols) {
  digitalWrite(DIR_MOTOR_X, HIGH);
  
  for (int n = 0; n < (cols * MM_NEMA); n++) {
    digitalWrite(STP_MOTOR_X, HIGH);
    delayMicroseconds(40);
    digitalWrite(STP_MOTOR_X, LOW);
    delayMicroseconds(40);

    if (digitalRead(PRT_ENDSTOP_INI) == 0)
      break;
  }

  delay(10);
  return 1;
}

/* 
 * Adianta ao folha tomando como base o tamanho da 
 * coluna que forma a palavra em braille
 */
int adiantarFolha(int t) {
  digitalWrite(DIR_MOTOR_Y, LOW);
  
  if (t == 0) {
    inicioPagina();
    adiantarColuna(20);
    
    while (digitalRead(PRT_TCRT_INI) == 1)
    {
      digitalWrite(STP_MOTOR_Y, HIGH);
      delayMicroseconds(60);
      digitalWrite(STP_MOTOR_Y, LOW);
      delayMicroseconds(60);
    }

    for (int n = 0; n < (200 * MM_BYJ); n++)
    {
      digitalWrite(STP_MOTOR_Y, HIGH);
      delayMicroseconds(60);
      digitalWrite(STP_MOTOR_Y, LOW);
      delayMicroseconds(60);
    }

    inicioPagina();
    delay(10);
  }
  else 
  {
    for (int n = 0; n < (t * MM_BYJ); n++)
    {
      digitalWrite(STP_MOTOR_Y, HIGH);
      delayMicroseconds(60);
      digitalWrite(STP_MOTOR_Y, LOW);
      delayMicroseconds(60);
    }
    
    delay(10);
  }

  return 1;
}

/* 
 * Retrocede a folha tomando como base o tamanho da 
 * coluna que forma a palavra em braille
 */
int retrocederFolha(int t) {
  digitalWrite(DIR_MOTOR_Y, HIGH);

  for (int n = 0; n < (t * MM_BYJ); n++)
  {
    digitalWrite(STP_MOTOR_Y, HIGH);
    delayMicroseconds(60);
    digitalWrite(STP_MOTOR_Y, LOW);
    delayMicroseconds(60);
  }

  delay(10);
  return 1;
}

/* 
 * Efetua a marcação da coluna para formar relevo na folha
 */
int marcarColuna() {
  for (int i = 0; i < 2; i++) {
    digitalWrite(INDUTOR_PRESS, i % 2 == 0 ? 1 : 0);
    delay(250);
  }
  
  delay(10);
  return 1;
}

/* 
 * Ajustar eixo Z para baixo
 */
int baixarEixoZ(int altura) {
  digitalWrite(DIR_MOTOR_Z, HIGH);

  for (int n = 0; n < (altura * MM_BYJ); n++)
  {
    digitalWrite(STP_MOTOR_Z, HIGH);
    delayMicroseconds(60);
    digitalWrite(STP_MOTOR_Z, LOW);
    delayMicroseconds(60);
  }

  delay(10);
  return 1;
}

/* 
 * Ajustar eixo Z para baixo
 */
int subirEixoZ(int altura) {
  digitalWrite(DIR_MOTOR_Z, LOW);

  for (int n = 0; n < (altura * MM_BYJ); n++)
  {
    digitalWrite(STP_MOTOR_Z, HIGH);
    delayMicroseconds(60);
    digitalWrite(STP_MOTOR_Z, LOW);
    delayMicroseconds(60);
  }

  delay(10);
  return 1;
}

/*
 * Gera a impressão de uma linha de texto
 */
int imprimirLinha(node *ln, int reverso)  {
  node *tmp = ln;

  if (reverso == 0) inicioPagina();
  reverso == 0 ? inicioPagina() : voltarColuna(1);
  
  while (tmp != NULL) {
    if (tmp->data == 1) {
      marcarColuna();

      reverso == 0 ? adiantarColuna(1) : voltarColuna(1);
      tmp = tmp->next;
    } else {
      int colunas = 0;

      while (tmp->data == 0 & tmp != NULL) {
        colunas += 1;
        tmp = tmp->next;
      }
      
      reverso == 0 ? adiantarColuna(colunas) : voltarColuna(colunas);
    }

    delay(10);
  }
    
  adiantarFolha(ALTURA_LINHA);  
  return 1;
}
