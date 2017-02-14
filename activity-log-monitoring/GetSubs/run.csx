using System;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Azure.Management.ResourceManager;
using System.Configuration;

public static void Run(TimerInfo myTimer, ICollector<string> myQueue, TraceWriter log)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");    

    var tenantId = ConfigurationManager.AppSettings["TENANT_ID"];
    var clientId = ConfigurationManager.AppSettings["CLIENT_ID"];
    var secret = ConfigurationManager.AppSettings["CLIENT_SECRET"];

    RunSubs(tenantId, clientId, secret, myQueue, log).Wait();
}


public static async Task RunSubs(string tenantId, string clientId, string secret, ICollector<string> myQueue, TraceWriter log)
{
    var servCreds = await ApplicationTokenProvider.LoginSilentAsync(tenantId, clientId, secret);

    var subClient = new SubscriptionClient(servCreds);
    log.Info("subs:");
    foreach (var s in subClient.Subscriptions.List())
    {
        log.Info(s.DisplayName);
        myQueue.Add(s.SubscriptionId); 
    }
}
