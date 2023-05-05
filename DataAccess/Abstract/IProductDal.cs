using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    // IProductDal == ("I")= interface || Product = Product tablosunun || Dal = Data Access Layer demek yani
    // product entitiy dal yazdığını belirtir. product veri erişim merkezi olduğunu belirtir.
    // product ile ilgili veri tabanında yapacağım operasyonları içeren interface kendisi.
    public interface IProductDal
    {
        List<Product> GetAll();
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);

        List<Product> GetAllCategory(int categoryId);


    }
}
