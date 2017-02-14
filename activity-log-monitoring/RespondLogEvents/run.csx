#r "Newtonsoft.Json"
#load "..\Shared\AzureEventRecords.csx"

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public static void Run(string myEventHubMessage, TraceWriter log)
{
    log.Info(myEventHubMessage);
    var jset = new JsonSerializerSettings();
    ITraceWriter jsonmem = new MemoryTraceWriter();
    jset.TraceWriter = jsonmem;
    jset.NullValueHandling = NullValueHandling.Ignore;
    try
    {
        var aerec = JsonConvert.DeserializeObject<List<AzureEventRecord>>(myEventHubMessage, jset);
        foreach(var v in aerec)
        {
            // your logic here
            log.Info(v.logevent.operationName);
        }
    }
    catch(Exception e)
    {
        log.Info(jsonmem.ToString());

    }
}

