public class AzureLogProfiles
{
    public List<LogProfile> value { get; set; }
}
public class LogProfile
{
    public string id { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string location { get; set; }
    public string kind { get; set; }
    public string tags { get; set; }
    public LogProperties properties { get; set; }
}

public class LogProperties
{
	public string storageAccountId { get; set;}
	public string serviceBusRuleId { get; set;}
}
