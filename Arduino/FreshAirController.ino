/**
 * FreshAirController
 * The microphone controller for the Fresh Air project.
 * 
 * @author Nathan Campos <nathan@innoveworkshop.com>
 */

// Pin configurations.
const int frontPin = A3;
const int backPin  = A2;
const int leftPin  = A1;
const int rightPin = A0;

// Constants.
const uint16_t peakMax = 665;
const uint16_t peakMin = 80;

// Global variables.
uint16_t front = 0;
uint16_t back  = 0;
uint16_t left  = 0;
uint16_t right = 0;

/**
 * Sets up the board.
 */
void setup() {
  Serial.begin(9600);
}

/**
 * Microcontroller loop.
 */
void loop() {
  // Acquire audio peaks.
  acquireAudio();

  // Send control signals.
  sendControl();
}

/**
 * Sends the control signals.
 */
void sendControl() {
  char buf[20];

  sprintf(buf, "%u,%u,%u,%u", front, back, left, right);
  Serial.println(buf);
}

/**
 * Acquires the audio peak voltages from the ADCs.
 */
void acquireAudio() {
  front = map(analogRead(frontPin), peakMin, peakMax, 0, 100);
  if (front > 100)
    front = 0;

  back = map(analogRead(backPin), peakMin, peakMax, 0, 100);
  if (back > 100)
    back = 0;
    
  left = map(analogRead(leftPin), peakMin, peakMax, 0, 100);
  if (left > 100)
    left = 0;
    
  right = map(analogRead(rightPin), peakMin, peakMax, 0, 100);
  if (right > 100)
    right = 0;
}

/**
 * Prints debug messages.
 */
void debugPrint() {
  Serial.print("Front: ");
  Serial.println(front);
  Serial.print("Back:  ");
  Serial.println(back);
  Serial.print("Left:  ");
  Serial.println(left);
  Serial.print("Right: ");
  Serial.println(right);
  Serial.println();
}
