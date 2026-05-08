#include "Types.h";
#include "Structs.h";

settings Config;
bytenode *byteArrayTest;
node *l1, *l2, *l3;
int linha;

int empty(node *cols)
{
  return cols->next == NULL ? 1 : 0;
}

void *mountnode(node *cols, uint8_t element) 
{
  node *newNode  = (node *) malloc(sizeof(node));
  newNode->data  = element;
  newNode->order = empty(cols) == 1 ? 1 : cols->order + 1;
  newNode->next  = NULL;

  return newNode;
}

void insert(node *cols, uint8_t element, int quantity)
{
  for (int i = 0; i < quantity; i++)
  {
    node *newNode   = mountnode(cols, element);
     
    if (empty(cols) == 1)
      cols->next = newNode;
    else
    {
      node *tmp = cols->next;
    
      while (tmp->next != NULL)
        tmp = tmp->next;

      newNode->order = tmp->order + 1;
      tmp->next = newNode;
    }
  }
}

node* inverter(node *ref)
{ 
  node* prev    = NULL;
  node* current = ref;
  node* next    = NULL;
  
  while (current != NULL) {
    next  = current->next;  
    current->next = prev;   
    prev = current;
    current = next;
  }
  
  return prev;
}

uint8_t getbit(byte data, int pos) 
{
  return (uint8_t)((data >> pos) & 0x01);
}

int datatype(byte data) {
  if (getbit(data, 7) == 1 & getbit(data, 6) == 1)
    return BreakLine;

  if (getbit(data, 7) == 0 & getbit(data, 6) == 0)
    return Min;

  if (getbit(data, 7) == 0 & getbit(data, 6) == 1)
    return Max;

  if (getbit(data, 7) == 1 & getbit(data, 6) == 0)
    return Number;
}
