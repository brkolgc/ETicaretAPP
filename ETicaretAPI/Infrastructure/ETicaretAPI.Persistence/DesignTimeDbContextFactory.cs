using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence
{
    //dotnet CLI üzerinden migration basabilmek için bu DesignTime'ın oluşturulması gerekiyor.
    //çünkü DbContext constructor'ında bizden DbContextOptions beklediği için ve CLI üzerinden yapılan işlemlerde constructor'a ilgili parametre gönderilemediğinden dolayı burada default olarak belirtiyoruz
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETicaretAPIDbContext>
    {
        public ETicaretAPIDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ETicaretAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configurations.ConnectionString);

            return new(dbContextOptionsBuilder.Options);
        }
    }
}
