using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        //Tüm veriler için( data ve mesaj)
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {

        }
        // sadece data ver
        public ErrorDataResult(T data) : base(data, false)
        {

        }
        // sadece mesaj ver
        public ErrorDataResult(string message) : base(default, false, message)
        {

        }
        // istersen hiç bir şey verme
        public ErrorDataResult() : base(default, false)
        {

        }
    }
}
