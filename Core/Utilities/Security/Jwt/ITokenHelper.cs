using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Jwt
{
    // önlem olarak interface ile veriliyor test amaclı kullanım için veya başka bir teknik için oluşturmak iyiir.
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}

// Giriş esnasında giriş doğruysa "CreateToken" içerisine girer ve ilgili kullanıcı için veri tabanına gider ve 
// bu kullanıcının cleim buluşturur ve orda bir adet jwt token üretir ve onları geri döndürür.
