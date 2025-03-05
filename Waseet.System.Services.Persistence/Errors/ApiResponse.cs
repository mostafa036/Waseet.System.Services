using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Persistence.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMassageForStatusCode(statusCode);
        }

        private string GetDefaultMassageForStatusCode(int statusCode) => statusCode switch
        {
            200 => "Ok, Request is Successful",
            201 => "Created, the resource is made",
            204 => "No Content, Nothing to Show",
            400 => "A Bad Request, You have made",
            401 => "Authorized, You are Not",
            403 => "Forbidden, you have no access",
            404 => "resource found, It was not.",
            500 => "Internal Error, Server is Broken",
            502 => "Bad Gateway, Something went wrong with the upstream server",
            503 => "Service unavailable",
            _ => "Some unknown Error occurred"
        };

    }
}
