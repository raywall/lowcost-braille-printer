typedef struct ByteNode {
  byte data;
  struct ByteNode *next;  
  byte type;
} bytenode;

typedef struct Node {
  int order;
  uint8_t data;
  struct Node *next;
} node;

typedef struct Settings {
  byte type;
  int count;
  int init;
} settings;
