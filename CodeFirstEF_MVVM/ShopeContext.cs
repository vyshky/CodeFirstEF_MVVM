using System;
using System.Data.Entity;
using System.Linq;

namespace CodeFirstEF_MVVM
{
    public class ShopContext : DbContext
    {
        public ShopContext()
            : base("name=ShopContext")
        {
        }
        public virtual DbSet<Product> Products { get; set; }
    }
    // enable-migrations консольная комманда
}