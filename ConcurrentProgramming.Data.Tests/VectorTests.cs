namespace ConcurrentProgramming.Data.Tests
{
    /// <summary>
    /// Vector tests.
    /// </summary>
    public class VectorTests
    {
        private readonly Vector v1 = new(1, 0.02);
        private readonly Vector v2 = new(-0.1, 5);

        /// <summary>
        /// Tests addition.
        /// </summary>
        [Fact]
        public void AddTests()
        {
            Assert.Equal(new Vector(0.9, 5.02), v1 + v2);
        }

        /// <summary>
        /// Tests subtraction.
        /// </summary>
        [Fact]
        public void SubtractTests()
        {
            Assert.Equal(new Vector(1.1, -4.98), v1 - v2);
        }

        /// <summary>
        /// Tests multiplication.
        /// </summary>
        [Fact]
        public void MultiplyTests()
        {
            Assert.Equal(new Vector(2, 0.04), v1 * 2);
            Assert.Equal(new Vector(0.1, -5), v2 * -1);
        }
    }
}
