using DatabasesBenchmark.Services.Helpers.Interfaces;

namespace DatabasesBenchmark.Services.Helpers.Implementations
{
    public class StringHelper : IStringHelper
    {
        public string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
