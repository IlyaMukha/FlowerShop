using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.UsersFolder
{
   public class Flowers
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public int count { get; set; }
        public BagFolder.Bag Bag2 { get; set; }

        public Flowers()
        {
        }
    }
}
