// Placas disponíveis
#define BOARD_CNC_V3    1
#define BOARD_RAMPS_14  2

// Indica a placa controladora utilizada
#define MOTHERBOARD BOARD_CNC_V3

// Verificar tipo de placa
#define MB(board) (MOTHERBOARD==BOARD_##board)
