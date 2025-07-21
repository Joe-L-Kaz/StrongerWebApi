using System;

namespace Stronger.Domain.Responses;

public class Response
{
    public int StatusCode { get; set; }

    public dynamic? Content { get; set; }

    public ErrorModel? Error { get; set; }

    public class ErrorModel
    {
        int StatusCode { get; set; }
        String Message { get; set; }

        public ErrorModel()
        {
            Message = String.Empty;
        }
    }
}
