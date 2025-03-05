using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waseet.System.Services.Domain.Identity
{
   public class ProfilePhotos
    {
        [Key]
        public int PhotoId { get; set; }
        public string FileName { get; private set; } = string.Empty;
        public string FilePath { get; private set; } = string.Empty;
        public string PhotoType { get; private set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }

        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
