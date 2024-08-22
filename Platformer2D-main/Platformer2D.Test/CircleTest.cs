
using Microsoft.Xna.Framework;

namespace Platformer2D.Test
{
    [TestFixture]
    public class CircleTest
    {
        /**
        * Circle test intersect with circle
        */
        [Test]
        public void Intersects_ReturnsTrue_WhenCircleIntersectsRectangle()
        {
            // Arrange
            Circle circle = new Circle(new Vector2(10, 10), 5);
            Rectangle rectangle = new Rectangle(5, 5, 10, 10);

            // Act
            bool result = circle.Intersects(rectangle);

            // Assert
            Assert.IsTrue(result);
        }

        /**
        * Circle test not intersect with rectangle
        */
        [Test]
        public void Intersects_ReturnsFalse_WhenCircleDoesNotIntersectRectangle()
        {
            // Arrange
            Circle circle = new Circle(new Vector2(10, 10), 5);
            Rectangle rectangle = new Rectangle(20, 20, 10, 10);

            // Act
            bool result = circle.Intersects(rectangle);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
