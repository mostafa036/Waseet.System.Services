using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waseet.System.Services.Application.ImagePredictionServices;

namespace Waseet.System.Services.Application.Abstractions
{
    public interface IImagePredictionService
    {
        Task<ImagePrediction> PredictAsync(byte[] imageData);

    }
}
