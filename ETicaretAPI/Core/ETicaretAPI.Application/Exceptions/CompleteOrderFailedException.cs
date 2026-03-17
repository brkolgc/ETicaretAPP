using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class CompleteOrderFailedException : Exception
    {
        public CompleteOrderFailedException(): base("Sipariş tamamlanırken bekleyenmeyen bir hata oluştu.")
        {
        }

        public CompleteOrderFailedException(string? message) : base(message)
        {
        }

        public CompleteOrderFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
