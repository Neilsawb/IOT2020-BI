#include <WiFi.h>   //#include <ESP8266WiFi.h>
#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>
#include <DHT.h>
#include <ArduinoJson.h>

#define DHT_PIN 21
#define DHT_TYPE DHT11

char *ssid = "Alfred";
char *pass = "nobel2019";
char *conn = "HostName=NS-iot2020hub.azure-devices.net;DeviceId=C4:4F:33:64:8B:01;SharedAccessKey=+Uxc3Ec/t46gF8DS0vfi3nOAY2JJlkKG6ks5nng8/AY=";

bool messagePending = false;
int interval = 1000 * 15;
unsigned long prevMillis = 0;
time_t epochTime;
float distance;

DHT dht(DHT_PIN,DHT_TYPE);
IOTHUB_CLIENT_LL_HANDLE deviceClient;

void setup() {
  initSerial();
  initWifi();
  initEpochTime();
  initDHT();
  initDevice();
  delay(2000);
}

void loop() {
  unsigned long currentMillis = millis();
  epochTime = time(NULL);
  float temperature = dht.readTemperature();
  float humidity = dht.readHumidity();
  
  if(!messagePending) {
    if((currentMillis - prevMillis) >= interval) {
      prevMillis = currentMillis;

      if(!(std::isnan(temperature)) && !(std::isnan(humidity)) && epochTime > 28800)  {
        Serial.printf("Current Time: %lu. ", epochTime);
        
        char payload[256];
        char epochTimeBuf[12];
        
        StaticJsonBuffer<sizeof(payload)> buf;
        JsonObject &root = buf.createObject();
        root["distance"] = distance;
        root.printTo(payload, sizeof(payload));

        sendMessage(payload, itoa(epochTime, epochTimeBuf, 10));       
      }
    }
  }

  IoTHubClient_LL_DoWork(deviceClient);
  delay(10);
}
