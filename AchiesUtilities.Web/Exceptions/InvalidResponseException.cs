using System.Runtime.Serialization;
using AchiesUtilities.Web.Extensions;

namespace AchiesUtilities.Web.Exceptions;

public class InvalidResponseException : Exception
{
    public HttpResponseMessage? HttpResponseMessage { get; }
    public HttpContent? ResponseContent { get; }
    public string? Response => _response ??= ResponseContent?.ReadAsStringSync();
    private string? _response;

    public InvalidResponseException(HttpContent? responseContent)
    {
        ResponseContent = responseContent;
    }

    public InvalidResponseException(HttpContent? responseContent, string message) : base(message)
    {
        ResponseContent = responseContent;
    }

    public InvalidResponseException(HttpContent? responseContent, string message, Exception inner) : base(message,
        inner)
    {
        ResponseContent = responseContent;
    }


    public InvalidResponseException(HttpResponseMessage? responseMessage)
    {
        ResponseContent = responseMessage?.Content;
        HttpResponseMessage = responseMessage;
    }

    public InvalidResponseException(HttpResponseMessage? responseMessage, string message) : base(message)
    {
        ResponseContent = responseMessage?.Content;
        HttpResponseMessage = responseMessage;
    }

    public InvalidResponseException(HttpResponseMessage? responseMessage, string message, Exception inner) : base(message,
        inner)
    {
        ResponseContent = responseMessage?.Content;
        HttpResponseMessage = responseMessage;
    }

    protected InvalidResponseException(
        SerializationInfo info,
        StreamingContext context) : base(info, context){}
}