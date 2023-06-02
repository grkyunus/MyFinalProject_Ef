﻿using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace Business.BusinessAspects.Autofac
{
    // JWT için Yazıldı ||  
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            // Bir metni bizim belirttiğim karaktere göre ayırıp Array içerisine atıyor.
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            // Yetki Kontrolü yapar.
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied); // yetkiniz yok hatası alınıyor.
        }
    }
}
