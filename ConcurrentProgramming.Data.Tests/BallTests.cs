using System.ComponentModel;

namespace ConcurrentProgramming.Data.Tests
{
    /// <summary>
    /// The standard ball tests.
    /// </summary>
    public class BallTests
    {
        private readonly Ball ball = new(new Vector(0, 0), new Vector(0, 0));

        /// <summary>
        /// Tests the position.
        /// </summary>
        [Fact]
        public void TestPosition()
        {
            Assert.Equal(new Vector(0, 0), ball.Position);

            ball.Position = new Vector(1, 1);

            Assert.Equal(new Vector(1, 1), ball.Position);
        }

        /// <summary>
        /// Tests the velocity.
        /// </summary>
        [Fact]
        public void TestVelocity()
        {
            Assert.Equal(new Vector(0, 0), ball.Velocity);

            ball.Velocity = new Vector(1, 1);

            Assert.Equal(new Vector(1, 1), ball.Velocity);
        }

        /// <summary>
        /// Tests the radius.
        /// </summary>
        [Fact]
        public void TestRadius()
        {
            Assert.Equal(5, ball.Radius);
        }

        /// <summary>
        /// Tests the property changed event.
        /// </summary>
        [Fact]
        public void TestPropertyChangedEvent()
        {
            var ball = new Ball(new Vector(1, 1), new Vector(1, 1));
            var fired = false;

            void good(object? sender, PropertyChangedEventArgs e) => fired = true;
            void bad(object? sender, PropertyChangedEventArgs e) => Assert.Fail();

            ball.PropertyChanged += good;

            ball.Position = new Vector(0, 0);
            if (!fired) { Assert.Fail(); }
            fired = false;

            ball.Velocity = new Vector(0, 0);
            if (!fired) { Assert.Fail(); }
            fired = false;

            ball.PropertyChanged -= good;


            ball.PropertyChanged += bad;

            ball.Position = new Vector(0, 0);
            ball.Velocity = new Vector(0, 0);

            ball.PropertyChanged -= bad;
        }
    }
}
