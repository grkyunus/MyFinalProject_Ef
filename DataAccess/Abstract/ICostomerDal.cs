using Core.DataAcces;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICustomerDal : IEntityRepository<Customer>
    {
        // Örneği bu Customer özelliği proje bittikten sonra ekledi tekrardan tüm özelliklerin başlıklarını yazmak yerine
        // IEntityRepository sayesinde kod tekrar yapmadan kod kalabalığı oluşturmadan sistem yürütmeye devam edebiliriz.
    }
}
