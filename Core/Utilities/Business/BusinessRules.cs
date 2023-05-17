using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        // params kullanıldığında istenildiği kadar IResult parametresi gönderilebilir.
        // Array haline getiriyor ve "," ile istenildiği kadar gönderilebilir.
        public static IResult Run(params IResult[] logics)
        {
            // parametre ile başarısız olanı business gönderiliyor işlem başarısız ifadesi ile.
            // logics kuralları ifader eder ve alt tarafta kuralları gez ve kurala uymayanı dön.
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic;
                }
            }

            return null;

        }

    }
}
