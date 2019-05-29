/**
   Detect addition / removal of four RC522 RFID readers over SPI

   Based on the examples from https://github.com/miguelbalboa/rfid

   Pin layout used:
   --------------------------
   Signal      Leonardo Pin
   --------------------------
   SPI SS 1    10
   SPI SS 2    8
   SPI SS 3    7
   SPI SS 4    6
   SPI SCK     ICSP-3
   SPI MOSI    ICSP-4
   SPI MISO    ICSP-1
   RST/Reset   RESET/ICSP-5

*/

#include <SPI.h>
#include <MFRC522.h>
#include "Adafruit_NeoPixel.h"

#define SS_1_PIN        10         // Configurable, take an unused pin, only HIGH/LOW required
#define SS_2_PIN        8          // Configurable, take an unused pin, only HIGH/LOW required
#define SS_3_PIN        7          // Configurable, take an unused pin, only HIGH/LOW required
#define SS_4_PIN        6          // Configurable, take an unused pin, only HIGH/LOW required
#define RST_PIN         9          // Pin 9 is same as ICSP pin 5
#define LED_COUNT       200
#define LED_PIN         2

#define NR_OF_READERS   4

byte ssPins[] = {SS_1_PIN, SS_2_PIN, SS_3_PIN, SS_4_PIN};

MFRC522 mfrc522[NR_OF_READERS];   // Create MFRC522 instance.

String uids[] = {"0", "0", "0", "0"}; // Array to store most recently read values

Adafruit_NeoPixel strip = Adafruit_NeoPixel(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800);



/**
   Initialize.
*/
void setup() {

  strip.begin();   // Initialize as  OUTPUT
  strip.clear();    // Initialize all pixels to 'off'
  //strip.show();  // turn neopixels off
  strip.show();

  Serial.begin(9600); // Initialize serial communications with the PC
  while (!Serial);    // Do nothing if no serial port is opened (added for Arduinos based on ATMEGA32U4)

  SPI.begin();        // Init SPI bus

  for (uint8_t reader = 0; reader < NR_OF_READERS; reader++) {
    mfrc522[reader].PCD_Init(ssPins[reader], RST_PIN); // Init each MFRC522 card
    Serial.print(F("Reader "));
    Serial.print(reader);
    Serial.print(F(": "));
    mfrc522[reader].PCD_DumpVersionToSerial();
  }
}

/**
   Main loop.
*/
void loop() {
  for (int i = 0; i < LED_COUNT; i++) {
    strip.setPixelColor(i, 255, 255, 255);
  }
  strip.show();
  
  for (uint8_t reader = 0; reader < NR_OF_READERS; reader++) {

    // Store if new tag presentz
    bool newTag = mfrc522[reader].PICC_IsNewCardPresent();

    // Store if new tag successfully read
    // If tag succesfully read, this value consistently flip-flops between true and false (unclear why)
    // But only false if tag not read
    // Therefore, if both false, no tag is present
    bool tagReadOnce = mfrc522[reader].PICC_ReadCardSerial();
    bool tagReadTwice = mfrc522[reader].PICC_ReadCardSerial();

    // New tag successfully read
    if (newTag && tagReadOnce) {

      // Save uid in array of current tags
      String uid = uidToDecimalString(mfrc522[reader].uid.uidByte, mfrc522[reader].uid.size);

      // If previously stored value is not same as currently read uid (when switching tags quickly)
      // Update array of current tags
      // Print to serial
      if (uids[reader] != uid) {
        uids[reader] = uid;
        printUpdate(reader);
      }

    } else if (!tagReadOnce && !tagReadTwice) {
      // If tag not read twice, there is tag present
      // Erase last read tag in array of current tags
      if (uids[reader] != "0") {
        uids[reader] = "0";
        printUpdate(reader);
      }
    }
  }
}

/**
   Helper routine to turn uid into string of decimals
*/
String uidToDecimalString(byte *buffer, byte bufferSize) {
  String uid = "";
  for (byte i = 0; i < bufferSize; i++) {
    uid += buffer[i];
  }
  return uid;
}

/**
   Helper routine to print the updated uid
*/
void printUpdate(int reader) {
  // Print (reader, updated id)
  String updateMsg = "(";
  updateMsg += reader;
  updateMsg += " ";
  updateMsg +=  uids[reader];
  updateMsg += ")";
  Serial.println(updateMsg);
}
