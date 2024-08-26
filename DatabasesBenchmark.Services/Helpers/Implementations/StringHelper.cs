using DatabasesBenchmark.Services.Helpers.Interfaces;

namespace DatabasesBenchmark.Services.Helpers.Implementations
{
    public class StringHelper : IStringHelper
    {
        /// <summary>
        /// Generates a random string of the specified length using a predefined set of characters.
        /// </summary>
        /// <param name="length">The length of the random string to generate. Defaults to 
        public string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
