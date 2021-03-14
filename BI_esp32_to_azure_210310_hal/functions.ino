void initSerial() {
  Serial.begin(115200);
  delay(2000);
  Serial.println("Serial initialized.");
}

void printLocalTime(){
  struct tm timeinfo;
  if(!getLocalTime(&timeinfo)){
    Serial.println("Failed to obtain time");
    return;
  }
  
  strftime (date_out,80,"%Y%m%d",&timeinfo); 
  strftime (time_out,80,"%H:%M:%S",&timeinfo); 
  
}

void initWifi() {
  WiFi.begin(ssid, pass);

  while(WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println(".");
  }

  Serial.println("WiFi initialized.");
  Serial.println(WiFi.macAddress());
}

void initEpochTime() {
  configTime(3600, 0, "pool.ntp.org", "time.nist.gov");

  while(true) {
    epochTime = time(NULL);

    if(epochTime == 28800) {
      delay(2000);
    } else {
      break;
    }
  }

  Serial.printf("Epochtime initialized. Current Time: %lu \n", epochTime);
}

void initDevice() {
  deviceClient = IoTHubClient_LL_CreateFromConnectionString(conn, MQTT_Protocol);
}
