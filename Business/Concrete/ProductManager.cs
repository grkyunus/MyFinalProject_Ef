using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
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
    // Bir entity Manager kendisi hariç başka bir dalı enjekte edemez.
    public class ProductManager : IProductService
    {
        // Dependency chain -- bağımlılık var. alt tarafta yapılan eklemede bağımlılık kopuyor.
        // alt taraftaki durum Loosely coupled denir. az bağımlılık durumu. isimlendirilir.
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal,ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")] // "Get" tüm cache siler ancak || "IProductService.Get" sayesinde sadece product.get hepsini siler.
        public IResult Add(Product product)
        {
            #region bilgi ve eski kodlar

            // business codes
            // yukarıdaki ve aşşağıdaki farklı işlemlerdir.
            // validation 
            // bu kısımda kurallar ProductValidator.cs bulunması gerekmektedir ve oradadır.
            // alt taraftaki kod bloğu buraya yazmak yerine | ValidationTool.cs yazmak daha uygundur bu sayede kod tekrarından kurtuluruz.
            //*****************************
            //var context = new ValidationContext<Product>(product);
            //ProductValidator productValidator = new ProductValidator();
            //var result = productValidator.Validate(context);
            //if (!result.IsValid)
            //{
            //    // throw new ValidationException(result.Errors);  // hoca bu şekilde yazdı ancak alt tarafa çevirmek zorunda kaldım.
            //    throw new FluentValidation.ValidationException(result.Errors);
            //}

            // ValidationTool.Validate(new ProductValidator(), product); // bu kod yukarıdaki [***] sayesinde gerek duyulmuyor.
            //*****************************
            // |11| işlem sayesinde alt taraftaki kod ihtiyaç yoktur daha iyi bir hale getirilmiştir.
            //if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
            //{
            //    if (CheckIfProductNameExists(product.ProductName).Success)
            //    {
            //        _productDal.Add(product);
            //        return new SuccessResult(Messages.ProductAdded);

            //    }
            //}
            //return new ErrorResult();
            //***********************
            #endregion

            // business codes
            // İş kurallarını bu halde düzenlemek daha iyidir. |11|
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());
            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }

        [CacheAspect] // inmemoriycache || key,value
        public IDataResult<List<Product>> GetAll()
        {
            //İş kodları
            // sorgunun amacı saat 22 olduğunda bakıma alındığı ya da servis dışı olduğunu belirtmek için.
            //if (DateTime.Now.Hour == 10)
            //{
            //    return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            //}
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        // Kuralların isimleri uzun olabilir önemli değil yeterki görevini açıklasın.
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId) // ister soldaki gibi istersen
                                                                             // || Product product olabilir ancak kuralları ayır ve özel tanımlamak önemlidir.
        {
            // Select count(*) from products where categoryId = 1 || alt taraftaki kod bunu arka planda oluşturur ve çalıştırır.
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 20)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        //13.gün 3:00:00 tekrar izle sabah.
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
            // bu kod satırının amacı olduda ürünü alamadığında kullanıcı ödeme için gönderdiği parayı geri iade işlemi içindir örneğin. || Detaylı araştırma yapılmalı 
        {
            Add(product);
            if (product.UnitPrice<10)
            {
                throw new Exception("");
            }
            Add(product);
            return null;
        }
    }
}
