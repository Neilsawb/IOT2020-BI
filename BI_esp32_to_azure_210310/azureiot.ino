

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
  Map_Add(properties, "type", "dht");
  Map_Add(properties, "Deviceid", "C4:4F:33:64:8B:01");
  Map_Add(properties, "School", "NACKADEMIN");
  Map_Add(properties, "Student", "Neil Sawbridge");
  Map_Add(properties, "Epoch", epochTime);
  Map_Add(properties, "Date", date_out);
  Map_Add(properties, "Time", time_out);
  

  if(IoTHubClient_LL_SendEventAsync(deviceClient, message, sendCallback, NULL) == IOTHUB_CLIENT_OK) {
    messagePending = true;
  }

  IoTHubMessage_Destroy(message);
}
