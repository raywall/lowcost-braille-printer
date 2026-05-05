#ifndef PINS_H
  #define PINS_H

  #include "Boards.h"

  #if MB(CNC_V3)
    // CNC Shield V3
    // Portas de conexão do motor do eixo X (Cabeça de impressão)
    #define STP_MOTOR_X 2
    #define DIR_MOTOR_X 5
    #define ENB_MOTOR_X 8
    #define LIMITE_AX_X 9

    // Portas de conexão do motor do eixo Y (Movimentação da folha)
    #define STP_MOTOR_Y 3
    #define DIR_MOTOR_Y 6
    #define ENB_MOTOR_Y 8
    #define LIMITE_AX_Y 10

    // Portas de conexão do motor do eixo Z (Pressão sob a folha)
    #define STP_MOTOR_Z 4
    #define DIR_MOTOR_Z 7
    #define ENB_MOTOR_Z 8
    #define LIMITE_AX_Z 11

    // Portas adicionais da CNC Shield V3 - DIGITAIS
    #define SPINDLE_ENABLE 12
    #define SPINDLE_DIRECTION 13
    #define INDUTOR_PRESS 10

    // Portas adicionais da CNC Shield V3 - ANALOGICAS
    #define RESET_ABORT A0
    #define FEED_HOLD A1
    #define CYCLE_START_RESUME A2
    #define COOLANT_ENABLE A3

    // Portas de leitura dos Endstops
    #define PRT_ENDSTOP_INI A4

    // Portas de leitura do TCRT5000
    #define PRT_ENDSTOP_INI -1

    // Portas de leitura do TCRT5000
    #define PRT_TCRT_INI -1

    // Buzzer
    #define BUZZER_PIN 52
  #endif

  #if MB(RAMPS_14)
    // Ramps 1.4
    // Portas de conexão do motor do eixo X (Cabeça de impressão)
    #define STP_MOTOR_X A0
    #define DIR_MOTOR_X A1
    #define ENB_MOTOR_X 38
    #define LIMITE_AX_X -1

    // Portas de conexão do motor do eixo Y (Movimentação da folha)
    #define STP_MOTOR_Y 60
    #define DIR_MOTOR_Y 61
    #define ENB_MOTOR_Y A2
    #define LIMITE_AX_Y -1

    // Portas de conexão do motor do eixo Z (Pressão sob a folha)
    #define STP_MOTOR_Z 46
    #define DIR_MOTOR_Z 48
    #define ENB_MOTOR_Z A8
    #define LIMITE_AX_Z -1

    // Portas adicionais da CNC Shield V3 - DIGITAIS
    #define SPINDLE_ENABLE    -1
    #define SPINDLE_DIRECTION -1
    #define INDUTOR_PRESS     10

    // Portas adicionais da CNC Shield V3 - ANALOGICAS
    #define RESET_ABORT -1
    #define FEED_HOLD -1
    #define CYCLE_START_RESUME -1
    #define COOLANT_ENABLE -1

    // Portas de leitura dos Endstops
    #define PRT_ENDSTOP_INI 3

    // Portas de leitura do TCRT5000
    #define PRT_TCRT_INI 14

    // Buzzer
    #define BUZZER_PIN 18
  #endif
#endif
