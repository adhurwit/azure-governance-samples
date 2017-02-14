public class AzureEventRecord
{
    public AzureEvent logevent { get; set; }
}
public class AzureEvent
{
    public string time { get; set; }
    public string resourceId { get; set; }
    public string operationName { get; set; }
    public string category { get; set; }
    public string resultType { get; set; }
    public string resultSignature { get; set; }
    public int durationMs { get; set; }
    public string callerIpAddress { get; set; }
    public string correlationId { get; set; }
    public AzureIdentity identity { get; set; }
    public string level { get; set; }
    public string location { get; set; }
    public Dictionary<string, string> properties { get; set; }

}
public class AzureIdentity
{
    public AzureAuth authorization { get; set; }
    public Dictionary<string, string> claims { get; set; }
}

public class AzureAuth
{
    public string scope { get; set; }
    public string action { get; set; }
    public Dictionary<string, string> evidence { get; set; }
}
public class AzureProperties
{
    public string statusCode {get; set;}
    public int serviceRequestId { get; set;}
    public string requestBody {get; set; }
}  
