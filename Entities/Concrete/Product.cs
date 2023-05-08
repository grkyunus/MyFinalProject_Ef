using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        public int ProductId { get; set; }
        public int CategoryId{ get; set; }
        public string ProductName { get; set; }
        public short UnitsInStock { get; set; } // Norlamde belki int verilebilir ancak veri tabanı ile uyumlu olmadılır smallint karşılığı short dur.
        public decimal UnitPrice { get; set; }
    }
}
