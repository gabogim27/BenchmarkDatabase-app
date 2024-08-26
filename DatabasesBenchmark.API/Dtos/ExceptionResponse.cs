using System.Net;

namespace DatabasesBenchmark.API.Dtos
{
    public class ExceptionResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Description { get; set; }

        public ExceptionResponse(HttpStatusCode statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
        }
    }
}
