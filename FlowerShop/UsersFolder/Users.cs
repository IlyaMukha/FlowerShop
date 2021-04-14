using FlowerShop.BagFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.UsersFolder
{
  public  class Users
    {
        public int Id { get; set; }
        public string Uname { get; set; }
        public byte[] Image { get; set; }
        public string UPhone { get; set; }
        public string UPassword { get; set; }
        
        public ICollection<Bag> Bags2 { get; set; }

        public Users()
        {
            Bags2 = new List<Bag>();
        }

    }
}
