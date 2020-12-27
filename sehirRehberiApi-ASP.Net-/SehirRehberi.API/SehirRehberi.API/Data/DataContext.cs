using Microsoft.EntityFrameworkCore;
using SehirRehberi.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.API.Data
{
    //Veritabanıyla projeyi ilişkilendirmek için DbContextten kalıtım alır.(Entity Framework kısmı.)
    public class DataContext:DbContext
    {
        //Hangi veritabanına bağlanacağını anlayabilmesi için parametreyi kullanırız.(base=DbContext)
        public DataContext(DbContextOptions<DataContext>options):base(options)
        {
                
        }

        //Modeldeki Value sınıfını Values tablosuyla eşleştirir.
        public DbSet<Value> Values { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
