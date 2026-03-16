using System.Security.Cryptography;

namespace ETicaretAPI.Application.Utilities
{
    public static class OrderCodeGenerator
    {
        public static string GenerateOrderCode()
        {
            string timestamp = DateTime.UtcNow.ToString("yyMMddHHmmssfff");

            byte[] randomBytes = new byte[2];
            RandomNumberGenerator.Fill(randomBytes);

            int randomNumber = BitConverter.ToInt16(randomBytes, 0) % 10000;
            string randomPart = randomNumber.ToString("D4");

            return $"C{timestamp}{randomPart}";
        }
    }
}
