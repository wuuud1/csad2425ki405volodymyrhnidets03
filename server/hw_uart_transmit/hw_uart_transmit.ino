#define RXD1 16
#define TXD1 17
#define BAUD_RATE 115200

bool promptPrinted = false; // Variable to track if the prompt  printed


void setup() {
  // Start communication on Serial Monitor (USB)
  Serial.begin(BAUD_RATE);

  // Start communication on UART1 (TX=17, RX=16)
  Serial1.begin(BAUD_RATE, SERIAL_8N1, RXD1, TXD1);

  Serial.println("ESP32 ready to communicate via UART1!");
}

void loop() {
  // Only print "Enter number:" once, when the prompt hasn't been printed yet
  if (!promptPrinted) {
    Serial1.print("Enter number: "); // No newline, stays on the same line
    promptPrinted = true; // Mark the prompt as printed
  }

  // Check if data is available on UART1
  if (Serial1.available() > 0) {
    // Read incoming data
    String input = Serial1.readStringUntil('\n');
    Serial.println("Data received via UART1.");

    // Convert the string to integer
    int receivedValue = input.toInt();
    
    // Multiply the received value by 2
    int result = receivedValue * 2;

    // Send result back via UART1
    Serial1.println(); // Move to a new line after user input
    Serial1.println("Received value: " + String(receivedValue));
    Serial1.println("Multiplied by 2: " + String(result));

    // Print the value directly to UART1 (for the external terminal)
    
    // Also print to USB serial for debugging
    Serial.println("Data sent via UART1.");

    // Reset prompt so it can be printed again
    promptPrinted = false; // Reset to allow re-prompting for new input
  }
}
