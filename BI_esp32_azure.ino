#include "ArduinoJson.h"  // Required for Json (using version 6)
#include <Adafruit_Sensor.h> 
#include <DHT.h>  //using dht11 sensor for temperature and humidity.
#include <WiFi.h>
#include <Esp32MQTTClient.h>
#include <DateTime.h>

char* ssid = "Alfred";
char* pword = "nobel2019";


#define DHT_PIN 21 // using pin 21 on a Esp32.
#define DHT_TYPE DHT11
DHT dht(DHT_PIN, DHT_TYPE);

static char* connectionString = "HostName=NeilsIOT2020hub.azure-devices.net;DeviceId=esp32;SharedAccessKey=lvTz7L3Toedl8PQ7j8jUaKFhmNfw2TJvORs1mGKbVqM=";
static bool _connected = false;

void setup() {
  Serial.begin(115200);
  delay(2000);
  
  WiFi.begin(ssid,pword);
  while(WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }

  Serial.println("\nIP Address: ");
  Serial.println(WiFi.localIP());

  iothub();
  dht.begin();
  
}

void loop() {

  if(_connected) {

    //setupDateTime();
    char payload[256];
    float temperature = dht.readTemperature();  // Get temperature.
    float humidity = dht.readHumidity(); // Get humidity.

    DynamicJsonDocument doc(256); // define doc for Json document.
    doc["deviceid"] = "esp32";
    doc["Temperature"] = temperature; 
    doc["Humidity"] = humidity;  // assign both temp and humidity to doc.
    doc["Name"] = "Neil Sawbridge";
    doc["School"] = "NACKADEMIN";
    
  
    serializeJson(doc, payload);  // apply doc values into buf to be send via driver in the next line.

    if (Esp32MQTTClient_SendEvent(payload))
     Serial.print(payload);
  }

  delay(10 * 1000);

}
