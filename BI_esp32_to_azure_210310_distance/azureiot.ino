

void initDevice() {
  deviceClient = IoTHubClient_LL_CreateFromConnectionString(conn, MQTT_Protocol);
}

void sendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *userContextCallback) {
  if(IOTHUB_CLIENT_CONFIRMATION_OK == result) {
    Serial.println("Sending message to Azure IoT Hub - succeeded.");
  }
  
  messagePending = false;
}

void sendMessage(char *payload, char *epochTime) {
  
  IOTHUB_MESSAGE_HANDLE message = IoTHubMessage_CreateFromByteArray((const unsigned char *) payload, strlen(payload));

  MAP_HANDLE properties = IoTHubMessage_Properties(message);
  Map_Add(properties, "deviceType", "dist");
  Map_Add(properties, "school", "NACKADEMIN");
  Map_Add(properties, "student", "Neil Sawbridge");
  Map_Add(properties, "latitude", "59.27961");
  Map_Add(properties, "longitude", "17.90018");
  Map_Add(properties, "epochTime", epochTime);

  if(IoTHubClient_LL_SendEventAsync(deviceClient, message, sendCallback, NULL) == IOTHUB_CLIENT_OK) {
    messagePending = true;
  }

  IoTHubMessage_Destroy(message);
}
