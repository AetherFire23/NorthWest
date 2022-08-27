using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Inventory.Models
{
    public class ItemModel
    {
        //[Key]
        public Guid Id { get; set; }
        public Guid Owner { get; set; }
        public int ResourceId { get; set; }
        public string Name { get; set; }
    }
}