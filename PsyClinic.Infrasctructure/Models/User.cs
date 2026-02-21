using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsyClinic.Infrasctructure.Models
{
    public class User : IdentityUser
    {
        public required string FederalRegistration { get; set; }
        public required string Name { get; set; }
    }
}
