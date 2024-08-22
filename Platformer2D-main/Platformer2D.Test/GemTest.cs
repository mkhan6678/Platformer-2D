using Moq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Platformer2D.Test;

public class GemTest
{   
    // mock level object
    private ILevel mockLevel;
    
    /**
    * Setup mock content and level before every test
    */
    [SetUp]
    public void SetupBefore()
    {
        var mockContent = new Mock<StubContentManager>();
        mockContent.Setup(cm =>
            cm.Load<Texture2D>(It.IsAny<string>())).Returns(StubTexture2D.GetInstance());
        mockLevel = new Level(mockContent.Object);
    }
    
    /**
    * Gem test constructor
    */
    [Test]
    public void Gem_ExpectGemObject_ToBeInstantiatedSuccessfully()
    {
        var position = new Vector2(10, 10);
        var gem = new Gem(mockLevel, position);
        Assert.That(gem.Color, Is.EqualTo(Color.Yellow));
        Assert.That(gem.PointValue, Is.EqualTo(30));
        Assert.That(gem.Position, Is.EqualTo(position));
        Assert.That(gem.BoundingCircle, Is.EqualTo(new Circle(position, Tile.Width / 3.0f)));
    }

    /**
    * Gem test update method
    */
    [Test]
    public void Gem_Update()
    {
        var gem = new Gem(mockLevel, new Vector2(10, 10));
        gem.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(0.5f)));
        Assert.That(gem.Position, Is.EqualTo(new Vector2(10, 10)));
    }
}