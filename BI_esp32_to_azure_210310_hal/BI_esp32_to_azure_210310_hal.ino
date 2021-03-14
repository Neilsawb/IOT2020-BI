#include <WiFi.h>   
#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>
#include <DHT.h>
#include <ArduinoJson.h>

char *ssid = "Alfred";
char *pass = "nobel2019";
char *conn = "HostName=NS-iot2020hub.azure-devices.net;DeviceId=NS:4F:33:64:8B:01;SharedAccessKey=bzPUnOW/amFUTwr9uqYOqzQ3DOBzobbi+ImSzYHUwjI=";

bool messagePending = false;
unsigned long prevMillis = 0;
time_t epochTime;
int hallValue = 0;
char *state = "undefined";
char *prevState = "undefined";
char date_out[32];
char time_out[32];
int stateCode = -1;

IOTHUB_CLIENT_LL_HANDLE deviceClient;

void setup() {
  initSerial();
  initWifi();
  initEpochTime();
  initDevice();
  delay(2000);
}

void loop() {
  unsigned long currentMillis = millis();
  hallValue = hallRead();
  Serial.println(hallValue);
  delay(2000);
  if (hallValue < 0) {
    state = "closed";
    stateCode = 0;
  }
  if (hallValue > 15) {
    state = "open";
    stateCode = 1;
  }
  
  if(!messagePending) {
    if (state != prevState) {
      prevState = state;
      char payload[256];
      char epochTimeBuf[12];
      printLocalTime();
      StaticJsonBuffer<sizeof(payload)> buf;
      JsonObject &root = buf.createObject();
      root["HallValue"] = hallValue;
      root["State"] = state;
      root["Statecode"] = stateCode;
      root.printTo(payload, sizeof(payload));

      sendMessage(payload, itoa(epochTime, epochTimeBuf, 10)); 
    }
  }
  IoTHubClient_LL_DoWork(deviceClient);
  delay(10);
}
