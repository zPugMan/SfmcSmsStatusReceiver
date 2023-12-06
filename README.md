Azure function responsible for listening and processing messages posted to an Azure Service Bus. The resulting message is lightly transformed and uploaded to a data extension within Salesforce Marketing cloud (SFMC).

# Environment
The following environment settings are expected to be defined.

|Environment Variable |Required?  |Description   |
|:--------------------|:--------------------|:--------------------|
|receipt_SERVICEBUS|Yes|Connection string to the service bus|
|receipt_QUEUE|Yes|Name of the queue to listen against|
|sfmcAuth|Yes|Authentication endpoint for SFMC. <br>This is unique to your assigned MID and can be retrieved when secrets are issued.|
|sfmcAuthClientId|Yes|SFMC issued client_id|
|sfmcAuthClientSecret|Yes|SFMC issued client_secret|
|sfmcDataExtensionKey|Yes|SFMC assigned external key for the data extension|
|sfmcDataExtensionPKey|Yes|Primary key defined for the data extension|

# Getting Started
To run the function locally in VSCode
* Open repo in VSCode
* Set `local.settings.json` per your individual settings
* Change directory to the `SfmcSmsStatusReceiver` project folder versus the root solution folder
* Execute: `func start`