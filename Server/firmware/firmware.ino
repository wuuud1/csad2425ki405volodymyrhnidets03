String receivedMessage = "";

void setup() {
  Serial.begin(9600); 
  Serial.println("Waiting for a message...");
}

void loop() {
  if (Serial.available() > 0) {
    receivedMessage = Serial.readString();

    if (receivedMessage.length() > 0) {
      receivedMessage += " Modified";
      Serial.println(receivedMessage);
    }
  }

}

