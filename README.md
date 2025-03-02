# TestTgBot
# Username in telegram: @AspNetTgTestBot
## Stack:
  ### Asp.Net WebApi
  ### Mssql Database 
  #### ![image](https://github.com/user-attachments/assets/fc10957d-fcfb-46e8-b497-6d81ed01bab2)
  #### Tables:
  * Users:
      ![image](https://github.com/user-attachments/assets/416abc19-0904-41d7-863f-a43c9bbbd43e)
  * UserHistories:
      ![image](https://github.com/user-attachments/assets/9f93fbb4-9e13-478e-8891-ce19635462d3)
  * HistoryRecords:
      ![image](https://github.com/user-attachments/assets/9c4ca1b2-78e7-4d21-8d27-165b2c006c60)
  * Cities:
      ![image](https://github.com/user-attachments/assets/708e3930-68c2-4c56-aa51-8eabf2306580)
  ### Dapper
  ### Telegram.Bot
  ### Swagger



## To install:
1. Download project
2. Install required user-secrets:
  * ```dotnet user-secrets set BotToken [your token from @BotFather] --project ApiTgBot```
  * ```dotnet user-secrets set OpenWeatherToken [your token from openweathermap.org] --project ApiTgBot```
  * ```dotnet user-secrets set ConnectionString [connection string to mssql db] --project ApiTgBot```
  * ```dotnet user-secrets set BotWebhookUrl [your url(if you use ngrok there should be link like 'https://***.ngrok-free.app')] --project ApiTgBot```
3. Go to your applicationUrl
