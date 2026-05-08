#include <Adafruit_GFX.h>
#include <Adafruit_TFTLCD.h>

#define BLACK           0x0000
#define GRAY            0x31A6
#define BLUE            0x001F
#define RED             0xF800
#define GREEN           0x07E0
#define CYAN            0x07FF
#define MAGENTA         0xF81F
#define YELLOW          0xFFE0
#define WHITE           0xFFFF

Adafruit_TFTLCD tft(A3, A2, A1, A0, A4);
