using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

//SOLID
// Open Closed Principle
namespace ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            ProductManager productManager = new ProductManager(new EfProductDal());

            foreach (var product in productManager.GetAll())
            {
                Console.WriteLine(product.ProductName);
            }

        }
    }
}