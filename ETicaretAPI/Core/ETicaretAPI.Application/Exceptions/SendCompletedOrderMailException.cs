using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class SendCompletedOrderMailException : Exception
    {
        public SendCompletedOrderMailException() :base("Tamamlanan sipariş mail gönderimi sırasında beklenmeyen bir hata oluştu.")
        {
        }

        public SendCompletedOrderMailException(string? message) : base(message)
        {
        }

        public SendCompletedOrderMailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
