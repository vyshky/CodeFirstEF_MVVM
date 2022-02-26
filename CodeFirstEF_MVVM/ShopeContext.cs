using System;
using System.Data.Entity;
using System.Linq;

namespace CodeFirstEF_MVVM
{
    public class ShopeContext : DbContext
    {
        public ShopeContext()
            : base("name=ShopContext")
        {
        }
        public virtual DbSet<Product> Products { get; set; }
    }

}