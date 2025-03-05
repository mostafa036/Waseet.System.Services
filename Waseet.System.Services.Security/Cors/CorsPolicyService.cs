using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Security.Cors
{
    public class CorsPolicyService
    {
        public void AddDefaultPolicy(CorsPolicyBuilder builder)
        {
            builder
                .AllowAnyOrigin()      // Modify as per your needs (e.g., AllowSpecificOrigin)
                .AllowAnyMethod()
                .AllowAnyHeader();
        }

        public void AddSpecificPolicy(CorsPolicyBuilder builder, string[] allowedOrigins)
        {
            builder
                .WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); // Use this if you allow cookies or tokens
        }


    }
}
