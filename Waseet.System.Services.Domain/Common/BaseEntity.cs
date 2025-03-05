using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Waseet.System.Services.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow; // Auto-set on creation
        public DateTime? UpdatedAt { get; private set; } // Nullable for optional updates
    }
}