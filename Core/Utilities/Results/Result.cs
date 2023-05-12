using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Utilities = Araçlar demektir.
namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message) : this(success)
        {
            Message = message;
            // Success = success;  Alt taraftaki constructor da saten bu görev üstlenmektedir.ama bunun yolu :this(...) yöntemidir.
        }

        // Olay şu ki bazen kullanıcıya mesaj vermeden işlem gerçekleştirğini belirtiriz.
        public Result(bool success)
        {
            Success = success;
        }

        // readonly olması sebebi ile constructor ile set edilebilir hale geliyor.
        public bool Success { get; }
        public string Message { get; }
    }
}
