using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        // Dependency chain -- bağımlılık var. alt tarafta yapılan eklemede bağımlılık kopuyor.
        // alt taraftaki durum Loosely coupled denir. az bağımlılık durumu. isimlendirilir.
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            // business codes
            // yukarıdaki ve aşşağıdaki farklı işlemlerdir.
            // validation 
            // bu kısımda kurallar ProductValidator.cs bulunması gerekmektedir ve oradadır.
            // alt taraftaki kod bloğu buraya yazmak yerine | ValidationTool.cs yazmak daha uygundur bu sayede kod tekrarından kurtuluruz.

            //var context = new ValidationContext<Product>(product);
            //ProductValidator productValidator = new ProductValidator();
            //var result = productValidator.Validate(context);
            //if (!result.IsValid)
            //{
            //    // throw new ValidationException(result.Errors);  // hoca bu şekilde yazdı ancak alt tarafa çevirmek zorunda kaldım.
            //    throw new FluentValidation.ValidationException(result.Errors);
            //}

            // ValidationTool.Validate(new ProductValidator(), product); // bu kod yukarıdaki [***] sayesinde gerek duyulmuyor.



            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            //İş kodları
            // sorgunun amacı saat 22 olduğunda bakıma alındığı ya da servis dışı olduğunu belirtmek için.
            //if (DateTime.Now.Hour == 10)
            //{
            //    return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            //}
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>( _productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product> (_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>> (_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>( _productDal.GetProductDetails());
        }

    }
}
