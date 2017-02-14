using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class LoggingHandler : DelegatingHandler
{
    TraceWriter _log;
    
    public LoggingHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }
    
    public LoggingHandler(HttpMessageHandler innerHandler, TraceWriter log)
        : base(innerHandler)
    {
        _log = log;
    }
    

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _log.Info("*********************");
        _log.Info("Request: ");
        _log.Info($"{request.Method} {request.RequestUri}");
        if (request.Content != null)
        {
            _log.Info(await request.Content.ReadAsStringAsync());
        }

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        _log.Info("Response: ");
        _log.Info($"{response.StatusCode} {response.ReasonPhrase}");
        if (response.Content != null)
        {
            _log.Info(await response.Content.ReadAsStringAsync());
        }

        return response;
    }
}
