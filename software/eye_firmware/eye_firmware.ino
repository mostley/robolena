#define FREQ_DISPLAY 1000 // Hz
#define FREQ_TIMEUPDATE 2 // Hz
#define SYSCLOCK 8000000 // Hz
#define PRESCALER0 64
#define T0_COMPARE SYSCLOCK / (FREQ_DISPLAY * PRESCALER0)

#define PRESCALER1 64
#define T1_COMPARE SYSCLOCK / (FREQ_TIMEUPDATE * PRESCALER1)
#define row_rate refresh_rate/8
#define pixel_rate row_rate/8

#include "pindefs.h"

volatile int cols[]={ // PC0,PD4,PB6!,PB3,PD5,PB4,PC2,PC3
  MTX_COL1,MTX_COL2,MTX_COL3,MTX_COL4,MTX_COL5,MTX_COL6,MTX_COL7,MTX_COL8}; // ON=LOW
volatile int rows[]={ // PB2,PC1,PD7,PB5,PD2,PD6,PD3,PB7!
  MTX_ROW1,MTX_ROW2,MTX_ROW3,MTX_ROW4,MTX_ROW5,MTX_ROW6,MTX_ROW7,MTX_ROW8}; // ON=HIGH

volatile boolean updatenow = false;
boolean buttonState = LOW;
unsigned long buttonMillis = 0;
boolean buttonHandled = true;

volatile char disp[8]={
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
};

char testdisp[8]={
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
  0B11111111,
};

void setup() {
  for(int i=0;i<8;i++) {
    disp[i] = testdisp[i];

    // Set normal pins as outputs
    pinMode(cols[i],OUTPUT);
    pinMode(rows[i],OUTPUT);
    digitalWrite(cols[i], HIGH); //col off
    digitalWrite(rows[i], LOW); // row off

    // Set A6/A7 (XTAL pins) separately
    if(cols[i] == A6) {
      DDRB |= B01000000;
      PORTB |= B01000000;
    }
    else if(cols[i] == A7) {
      DDRB |= B10000000;
      PORTB |= B10000000;
    }
    if(rows[i] == A6) {
      DDRB |= B01000000;
      PORTB &=~ B01000000;
    }
    else if(rows[i] == A7) {
      DDRB |= B10000000;
      PORTB &=~ B10000000;
    }
  }

  // Set up pins 0 and 1 to set time
  pinMode(PIN_BUTTON, INPUT_PULLUP);


  TCCR2A = 0;
  TCCR2B = 0;
  TCNT2  = 0;
  // set compare match register to match frequency
  OCR2A = T0_COMPARE;// = (sysclock)/(freq*prescaler)-1  (must be <256)
  // turn on CTC mode
  TCCR2A |= (1 << WGM21);
  // Set prescaler
  TCCR2B |= (1 << CS21);
  // enable timer compare interrupt
  TIMSK2 |= (1 << OCIE2A);

  if(digitalRead(PIN_BUTTON) == LOW) {
    while(true);
  }

  TCCR1A = 0;// set entire TCCR1A register to 0
  TCCR1B = 0;// same for TCCR1B
  TCNT1  = 0;// initialize counter value to 0
  // set compare match register for 2hz increments
  OCR1A = T1_COMPARE;// = (sysclock)/(freq*prescaler)-1 (must be <65536)
  // turn on CTC mode
  TCCR1A |= (1 << WGM12);
  // Set CS12 and CS10 bits for prescaler
  TCCR1B |= (1 << CS11) | (1 << CS10);
  // enable timer compare interrupt
  TIMSK1 |= (1 << OCIE1A);

  //  //  Serial.begin(115200);
}


void loop() {
  if(updatenow) {
    prepareDisplay();
    updatenow = false;
  }

  if(digitalRead(PIN_BUTTON) != buttonState) {
    buttonState = digitalRead(PIN_BUTTON);
    if(buttonState == LOW) { // button was pressed
      buttonMillis = millis();
      buttonHandled = false;
    } else { // button was released
      buttonHandled = true;
      unsigned long buttonDelay = millis() - buttonMillis;
      if(buttonDelay > 100) { // debounce
        if(buttonDelay < 1000) { // simple press
          updatenow = true;

          onButtonPressed(buttonDelay);
        }
      }
    }
  } else {
    if(buttonState == LOW && !buttonHandled) { // button is being pressed
      unsigned long buttonDelay = millis() - buttonMillis;
      if(buttonDelay > 2000) {
        buttonHandled = true;
        updatenow = true;
      }
    }
  }
}

void onButtonPressed(unsigned long delay)
{

}

void prepareDisplay() {
  for(int r=0;r<8;r++) {
    disp[r]=B00000000;
    for(int c=0;c<8;c++) {
      disp[r] |= pictures[0][r] & (B10000000 >> c);
    }
  }
}


