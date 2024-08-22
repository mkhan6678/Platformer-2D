using Moq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D.Test;

public class EnemyTest
{   
    /**
    * Enemy test movement direction when collide with a cliff or timeout
    */
    [TestCase(0, 0.5f, TestName = "Enemy_ExpectChangeDirection_WhenWaitTimeIsOut")]
    [TestCase(2, 0.1f, TestName = "Enemy_ExpectMoveInCurrentDirection_WhenNotRunIntoACliff")]
    public void Enemy_Update(int collisionTypeValue, float gameTime)
    {
        var mockLevel = Mock.Of<ILevel>();
        var mock = Mock.Get<ILevel>(mockLevel);
        mock.Setup(
            l => l.GetCollision(It.IsAny<int>(), It.IsAny<int>())
        ).Returns((TileCollision)collisionTypeValue);
        var enemy = new Enemy(mockLevel, new Vector2(), "MonsterA",
            Mock.Of<IAnimation>(),
            Mock.Of<IAnimation>(),
            Mock.Of<IAnimationPlayer>()
        );

        var gt = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(gameTime));
        enemy.Update(gt);
        // run update second time to update waitTime
        enemy.Update(gt);
    }

    /**
    * Enemy test bounding rectangle is in world space
    */
    [Test]
    public void Enemy_GetBouncingRectangle_ExpectedBoundsTheEnemyInWorldSpace()
    {
        var mockRunAnimation = Mock.Of<IAnimation>();
        var mock = Mock.Get(mockRunAnimation);
        mock.Setup(ra => ra.FrameHeight).Returns(10);
        mock.Setup(ra => ra.FrameWidth).Returns(15);

        var mockIdleAnimation = Mock.Of<IAnimation>();
        mock = Mock.Get(mockIdleAnimation);
        mock.Setup(ia => ia.FrameHeight).Returns(10);
        mock.Setup(ia => ia.FrameWidth).Returns(5);

        var enemy = new Enemy(Mock.Of<ILevel>(),
            new Vector2(10, 10), "C",
            mockRunAnimation,
            mockIdleAnimation,
            Mock.Of<IAnimationPlayer>()
        );
        var boundingRectangle = enemy.BoundingRectangle;
        var expectedBoundingRectangle = new Rectangle(12, 13, 1, 7);
        var equal = boundingRectangle == expectedBoundingRectangle;
        Assert.That(equal, Is.True);
    }
    
    /**
    * Enemy test constructor
    */
    [Test]
    public void Enemy_TestConstructor()
    {
        var mockContent = new Mock<StubContentManager>();
        mockContent.Setup(cm =>
            cm.Load<Texture2D>(It.IsAny<string>())).Returns(StubTexture2D.GetInstance());
        var enemy = new Enemy(new Level(mockContent.Object), new Vector2(5,10), "MonsterA");
        var expectedBoundingRectangle = new Rectangle(5, 10, 0, 0);
        var equal = enemy.BoundingRectangle == expectedBoundingRectangle;
        Assert.That(equal, Is.True);
    }
}