using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.BagFolder
{
  public  class Bag
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string UserPhone { get; set; }
        public string Type { get; set; }
        public  string coutnFlow { get; set; }
        public int count { get; set; }
        public UsersFolder.Users Users2 { get; set; }


        public ICollection<UsersFolder.Flowers> Flowers2 { get; set; }

        public Bag()
        {
            Flowers2 = new List<UsersFolder.Flowers>();
        }
    }
}
