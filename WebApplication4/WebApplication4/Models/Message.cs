using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class MessageModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Topic { get; set; }

        [Required]
        [StringLength(2000)]
        public string Text { get; set; }
    }
}
