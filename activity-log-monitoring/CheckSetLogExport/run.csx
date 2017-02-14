#r "Newtonsoft.Json"
#load "..\Shared\AuthenticationHelpers.csx"
#load "..\Shared\LoggingHandler.csx"
#load "..\Shared\AzureLogProfiles.csx"


using System;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Azure.Management.ResourceManager;
using System.Configuration;
using Newtonsoft.Json;

static string _logprofname = "logprofile01";

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");

    var tenantId = ConfigurationManager.AppSettings["TENANT_ID"];
    var clientId = ConfigurationManager.AppSettings["CLIENT_ID"];
    var secret = ConfigurationManager.AppSettings["CLIENT_SECRET"];

    var logstore = ConfigurationManager.AppSettings["LOG_STORAGE"];
    var logeh = ConfigurationManager.AppSettings["LOG_EVENTHUB"];
    
   RunSubLogs(tenantId, clientId, secret, logstore, logeh, myQueueItem, log).Wait();
}

public static async Task RunSubLogs(string tenantId, string clientId, string secret, string logstore, string logeh, string subId, TraceWriter log)
{
    string token = await AuthenticationHelpers.AcquireTokenBySPN(tenantId, clientId, secret);
    log.Info($"token: {token}");

    using (var client = new HttpClient(new LoggingHandler(new HttpClientHandler(), log)))
    {
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        client.BaseAddress = new Uri("https://management.azure.com/");

        string logpname = await GetLogProfiles(client, subId, logstore, logeh, log);
        if(logpname.Length>0)
            await SetLogProfiles(client, subId, logstore, logeh, logpname, log);
    }
}

static async Task SetLogProfiles(HttpClient client, string subId, string logstore, string logeh,string logpname, TraceWriter log)
{
    string newjson = @"{""properties"": { ""storageAccountId"":""";
        newjson += logstore + @""",""serviceBusRuleId"":""";
        newjson += logeh + @""", ""locations"": [""global"",""eastus"",""westus"",""eastus2""], ""categories"": [ ""Write"", ""Delete"",""Action""],";
        newjson += @"""retentionPolicy"": {""enabled"": true, ""days"": 1 } } }";

    using (var response = await client.PutAsync(
        "/subscriptions/" + subId + "/providers/Microsoft.Insights/logprofiles/" + logpname + "?api-version=2016-03-01",
        new StringContent(newjson,System.Text.Encoding.UTF8, 
                                    "application/json")))
    {
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsAsync<dynamic>();
        log.Info($"{json}");

    }
}

static async Task<string> GetLogProfiles(HttpClient client, string subId, string logstore, string logeh, TraceWriter log)
{
    using (var response = await client.GetAsync(
        "/subscriptions/" + subId + "/providers/Microsoft.Insights/logprofiles?api-version=2016-03-01"))
    {
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsAsync<dynamic>();
        AzureLogProfiles logprofiles = JsonConvert.DeserializeObject<AzureLogProfiles>($"{json}");
        if(logprofiles.value.Count == 0)
            return _logprofname;

        var store = logprofiles.value[0].properties.storageAccountId;
        var ehub = logprofiles.value[0].properties.serviceBusRuleId;
        var logpname = logprofiles.value[0].name;

        if(store != logstore || ehub != logeh)
        {
            return logpname;

        }else{
            return "";
        }
    }
}

