using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60) //
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>(); // Redis eklemek için burda değişikliğe gerek yoktur.Redis klasör olurturmak ve coremodule eklemek.
        }

        public override void Intercept(IInvocation invocation)
        {
            //invocation(metot) || ReflectedType.FullName ( namespace ismini alır.) || Method.Name (metot ismini alır.)
            //Northwind.Business.IProductService.GetAll();
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList(); // Metodun parametrelerini listeye çevir.
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})"; // varsa parametleri ekler yoksa null atar. || join işlemi ile her parametre arasına "," eklemek için. 
            if (_cacheManager.IsAdd(key))// cache olup olmadığını kontrol eder.
            {
                invocation.ReturnValue = _cacheManager.Get(key); // Eğer varsa metodu çalıştırmadan geri dön. cachedeki veriyi getir.
                return;
            }
            invocation.Proceed(); // Eğer yoksa çalış veri tabanından veriyi getir.
            _cacheManager.Add(key, invocation.ReturnValue, _duration); // ve burda cache veri eklenmiş olur.
        }
    }
}

// 
