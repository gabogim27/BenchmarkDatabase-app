namespace DatabasesBenchmark.Domain.Entities
{
    /// <summary>
    /// Represents a benchmark entity used to store and manage benchmark data in the database.
    /// </summary>
    public class Benchmark
    {
        /// <summary>
        /// Gets or sets the unique identifier for the benchmark entity.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the random string associated with the benchmark entity.
        /// </summary>
        public string RandomString { get; set; }
    }
}
