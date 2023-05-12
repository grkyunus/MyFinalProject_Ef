using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Utilities = Araçlar demektir.
namespace Core.Utilities.Results
{
    //Temel voidler için başlangıç 
    public interface IResult
    {
        // yapılan işlemin gerçekleştirini ya da gerçekleşmediğini belirler.
        bool Success { get; }
        // yapılan işlemin döndüsüne göre mesaj iletime için kullanılacak.
        string Message { get; }

    }
}
