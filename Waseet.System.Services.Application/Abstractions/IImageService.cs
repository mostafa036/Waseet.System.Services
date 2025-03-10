using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string folderPath);
    }

}
