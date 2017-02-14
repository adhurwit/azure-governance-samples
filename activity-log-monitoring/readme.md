# Audit Log Monitoring Solution in Azure #

## Overview

This solution is focused on how to set-up a generic monitoring solution for Activity Logs that can be used when  you want to react to events as they happen. 

All operations in Azure generate events which are published as Activity Logs. 


## Ensure that all Activity Logs are exported to Event Hubs

Activity logs are defined on a Subscription level. In Enterprise environments, many Subscriptions are in use and need to be monitored. So the first part of this solution is to ensure that all Subscriptions, current and future, are configured to export to Event Hubs. 


### Create a service principal with tenant-level Reader permission

Follow these instructions to create a service principal. 

Tenant-level RBAC is a new feature. You can learn here how to make use of it. For our purposes we can choose the Reader role. 

### Create an Azure Function to retrieve Subscriptions

Create an Azure Functions app and create a timer Function. The Timer has good granularity. I run it every 5 minutes. 

Set an Azure Queue Storage output. 

When the Function runs it will retrieve all Subscriptions and put them into a Queue. 

See the code in the GetSubs folder

You will need to set App Settings with the Service Principal information. You do this in the Function app settings of the Azure Functions App. 


### Create an Azure Function to check and set the Activity Log export feature

Create a second Function and bind it to the Queue that you used in the first Function. This second Function will grab each Subscription ID from the Queue and ensure that the log profile is set to Export to your Event Hub. 

See the code in the CheckSetLogExport folder

You will have to go into Kudu to access the file system of your Azure Functions. Create a shared directory ("Shared") and put AuthenticationHelpers.csx, LoggingHandler.csx, and AzureLogProfiles.csx into it 


## Stream Analytics job for filtering of events

Now that all your Subscriptions are exporting their Activity Logs to the same Event Hub, create a Stream Analytics job that has the Event Hub as its Input ("ALogs"), a Query that filters the stream, and then another Event Hub for output ("ALogWrites"). 


	SELECT
    arrayElement.ArrayValue as logevent
	INTO
	    [ALogWrites]
	FROM
	    [ALogs] as logrecs
	CROSS APPLY GetArrayElements(logrecs.records) AS arrayElement
	
	WHERE
	    REGEXMATCH(arrayElement.ArrayValue.operationName, '/write$') > 0


## Azure Functions to act on your filtered events

The last step of this process is to create another Azure Function (in the same Functions App as before) and bind it to the filtered stream Event Hub. 

This Function runs when new events come in. In order to do something with the events, I have created classes for the event records so that they can used strongly-typed. 

Put AzureEventRecords.csx into the Shared directory. Deserialization will actually occur automatically for you as the events are passed to your function. 

See the code in the RespondLogEvents folder


## Conclusion 
That's it. You should be able to enable many use cases by adding logic to this last Function and adjusting the way events are filtered in the Stream Analytics job.  




