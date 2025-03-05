using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Persistence.Errors
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string Details { get; set; } = string.Empty;

        public ApiExceptionResponse(int statuscode, string message = null, string details = null) : base(statuscode, message)
        {
            Details = details;
        }


    }
}
