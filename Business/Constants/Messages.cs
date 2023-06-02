using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    // static verilirse new gerek yoktur.
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün İsmi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductListed = "Ürünler listelendi";
        public static string ProductCountOfCategoryError = "Bir kategoride ürün eklenebilirliğin sınırına geldiniz (20)";
        public static string ProductNameAlreadyExists = "Bu isimde başka bir ürün bulunmaktadır";
        public static string CategoryLimitExceded = "Kategori limitine ulaşıldı bu sebeple ekleme yapılamaz";
        public static string AuthorizationDenied = "Yetkiniz yok";
        public static string UserRegistered = "Kayıt oldu";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError="Parola hatası";
        public static string SuccessfulLogin="Başarılı giriş";
        public static string UserAlreadyExists= "Kullanıcı mevcut";
        public static string AccessTokenCreated="Token oluşturuldu";
    }
}
