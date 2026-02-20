using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PsyClinic.Infrasctructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsyClinic.Infrasctructure.Data
{
    public class AppDbContext(DbContextOptions options) 
        : IdentityDbContext<User>(options);
}
