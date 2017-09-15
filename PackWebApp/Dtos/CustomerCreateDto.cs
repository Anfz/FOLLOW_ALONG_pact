using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackWebApp.Dtos
{
    public class CustomerCreateDto
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public int Age { get; set; }
        
    }
}
