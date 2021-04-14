using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.MessegeFolder
{
    public  class Message
    {
        public int Id { get; set; }
        
        public string message1 { get; set; }

        public string tell { get; set; }
    }
}
