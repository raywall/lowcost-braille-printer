# BraillePrinter
Firmware, modelos 3D e servidor local de impressão da impressora braille de baixo custo baseada no Arduino Mega.

## Estrutura

- `firmware/`: firmware Arduino original.
- `driver/`: Braille Print Server local via IPP.
- `Modelos3D.zip`: modelos 3D da impressora.

Esta é a versão original da firmware para a impressora braille de baixo custo desenvolvida em C, para atuar com microcontroladores 
ATMega 2560, construída para no meu TCC de conclusão de curso em engenharia de computação da UNIFEV em 2018.

Aqui, além da primeira versão do código que foi construído e apresentado no TCC, também estão os modelos 3D que projetei para a construção da impressora 
e a lista de componentes que permitirão a você reproduzir a impressora e implementá-la.

Peço apenas que todas as referências ao desenvolvedor do projeto sejam mantidas, e espero sinceramente que este projeto possa beneficiar
aqueles que realmente precisam.

https://user-images.githubusercontent.com/6422997/128648269-b0d921a4-b340-4ca5-ad9f-da523fdeb597.mp4


### Braille

O conceito do Braille relativamente simples. 
Nele cada letra é representada por uma matriz 3x2 contendo 6 pontos chamada de cela, e a combinação dos pontos indica a letra escrita.

Abaixo está representado o nosso alfabeto:

![image](https://user-images.githubusercontent.com/6422997/128647874-25bbdfbb-9f30-462e-8e6e-a0b8b82c0081.png)

Também é possível utilizar a musicografia braille para representar partituras musicais, como demonstrado abaixo:

![image](https://user-images.githubusercontent.com/6422997/128647889-ba8ac0b3-38e0-4063-a13f-383494d46544.png)

Obs. É possível converter partituras musicais criadas no Musibraille (UFRJ) e imprimir com esta impressora


### Comunicação e interpretação do Braille pela impressora

Com base nestes modelos, a impressora funciona de forma bastante simples e rudimentar, recebendo através de comunicação serial uma mensagem
contendo o identiicação do tipo de conteúdo e o conteúdo da página a ser impressa.

Os quatro bits mais significativos identificam o conteúdo a ser impresso, os quatro bits subsequentes foram reservados para implementações futuras, e a partir
do nono bit é representado o conteúdo a ser impresso.

![image](https://user-images.githubusercontent.com/6422997/128647972-46af991b-2b76-4abd-8304-9021ecffe4e6.png)

O conteúdo da mensagem é composto por conjuntos de 8 bits que representam a cela a ser impressa.
Os dois bits mais significativos deste grupo indica se a letra é maiusculas ou minusculas, enquanto os seis bits subsequentes são responsáveis pela representação
da cela braille, sendo um bit em auto (1) equivalente a pressão e o seu oposto (0) indicando um espacamento.

![image](https://user-images.githubusercontent.com/6422997/128648045-4b9b40da-892e-48ce-ba97-7cad3ad893b6.png)


### Montando a impressora

![image](https://user-images.githubusercontent.com/6422997/128648093-e0c562b6-e7b7-42e7-8349-af41ac90ef87.png)

Para reproduzir este projeto você precisa de:
- 01 Arduino Mega 2560
- 01 RAMPS 1.4
- 03 drivers Pololu A4988
- 01 sensor ótico reflexivo TCRT5000
- 01 solenoide TAU-0826
- 01 fim de curso (endstop)

Utilizando nomenclaturas conhecidas na geometria espacial, a impressora possui movimentação em três eixos distintos: X, Y e Z
- O eixo Y é responsável pelo avanço e retrocesso do papel
- O eixo X possibilita a movimentação da cabeça de impressão sobre a folha
- E o eixo Z é responsavel por estabelecer a pressão sobre a página, de forma que possibilite a marcação dos pontos que compõem a cela Braille

![image](https://user-images.githubusercontent.com/6422997/128647148-81d3888b-7c8d-4ec9-880a-0d8ba93ca295.png)

A movimentação da cabeça de impresão é feita por: 
- 01 motor Nema 17
- 01 polia GT2 de 20 dentes
- 01 rolamento 624zz de 4mm
- 04 rolamentos lineares LM8UU de 8mm
- 02 guias de aço lineares (a mesma usada em impressoras matriciais antigas como a Epson LX300)

![image](https://user-images.githubusercontent.com/6422997/128647307-a3764e78-cad5-4247-a010-5c4d2e6f5018.png)

O motor e o rolamento 6244zz são entrelaçados por uma correia de 6mm que é conectada a base da cabeça de impressão, possibilitando a movimentação da
mesma bilateralmente sobre o eixo X

![image](https://user-images.githubusercontent.com/6422997/128647517-93bb48c5-11b6-4c39-a4c7-ce972201d2da.png)

Você pode utilizar bracadeiras de naillon para prender a correia a base da cabeça de impressão

![image](https://user-images.githubusercontent.com/6422997/128647549-9a7f3d9e-f7fc-4905-b0b0-b5333ffb6ed4.png)

A pressão exercida sobre o papel é responsabilidade do solenoide.
Ele fica posicionado na cabeça de impressão e recebe comandos do Arduino para realizar a pressão sobre o papel, enquanto o sensor óptico identifica
a ausencia/presença da folha de papel na impressora.

![image](https://user-images.githubusercontent.com/6422997/128647589-09d90f74-c872-4343-bf4d-3cc58cdeff36.png)

Um terceiro eixo, posicionado rente a folha e com borrachas com alta aderencia possibilitam puxar ou retroceder a folha no eixo Y sempre que necessário

![image](https://user-images.githubusercontent.com/6422997/128647656-a9f7a5f6-c261-4779-98bd-9bb91bfe3fde.png)

Para garantir a leveza e menor esforço dos movimentos do eixo, do lado direito utilizamos um rolamento 608ZZ

![image](https://user-images.githubusercontent.com/6422997/128647697-94e8e6dd-b658-40c7-b110-6af0983e8696.png)

Enquanto do lado esquerdo um motor de passo 288YJ-48 de 5V garante o movimento preciso da folha

![image](https://user-images.githubusercontent.com/6422997/128647755-61d23b29-54f0-4c75-84e0-1d054c81657b.png)

Nos modelos 3D da impressora você encontrará também um suporte para o endstop, ele é responsável por informar a impressora que a cabeça de impressão
chegou ao limite da folha

![image](https://user-images.githubusercontent.com/6422997/128647789-9cd4dbc1-8704-4477-bafc-79f36f8959c4.png)

E também irá perceber que todos os modelos podem ser impressos em uma impressora 3D comum.
Foi justamente pensando nisso que a base da impressora foi desenhada de forma modular, permitindo que sejam montadas e conectadas umas as outras.
Também encontrará no projeto esquemas de adaptação dos conectores dos motores e da montagem de toda parte eletronica.

Talvez demore um pouco para responder, mas qualquer dúvida ficarei feliz em ajudar!
