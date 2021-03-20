using AnaliseDadosDotNetCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnaliseDadosDotNetCore.Context
{
    class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public AppDbContext()
        {

        }

        public DbSet<Dados> Dados { get; set; }
    }
}
