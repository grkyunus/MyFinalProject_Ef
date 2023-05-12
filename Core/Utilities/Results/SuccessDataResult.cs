using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T> 
    {
        //Tüm veriler için( data ve mesaj)
        public SuccessDataResult(T data, string message):base(data,true,message)
        {
            
        }
        // sadece data ver
        public SuccessDataResult(T data):base(data,true)
        {
            
        }
        // sadece mesaj ver
        public SuccessDataResult(string message): base(default,true,message)
        {
            
        }
        // istersen hiç bir şey verme
        public SuccessDataResult():base(default,true)
        {
            
        }
    }
}
