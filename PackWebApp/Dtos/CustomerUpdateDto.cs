using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PackWebApp.Dtos
{
    public class CustomerUpdateDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [Range(0, 200)]
        public int Age { get; set; }
        
    }
}
