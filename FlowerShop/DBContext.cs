using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowerShop.AdminFloder;
using FlowerShop.BagFolder;
using FlowerShop.HistoryFolder;
using FlowerShop.MessegeFolder;
using FlowerShop.UsersFolder;

namespace FlowerShop
{
    class DBContext : DbContext
    {
        public DBContext() : base("DefaultConnection")
        {

        }
        public DbSet<Admin> Admin1 { get; set; }
        public DbSet<Bag> Bags1 { get; set; }
        public DbSet<Flowers> Flowers1 { get; set; }
        public DbSet<Users> Users1 { get; set; }
        public DbSet<Message> Message1 { get; set; }
        public DbSet<Histiory> Histiorie1 { get; set; }
    }
}
