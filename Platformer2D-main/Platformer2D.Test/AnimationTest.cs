namespace Platformer2D.Test;

public class AnimationTest
{
    /**
    * Animation constructor test
    */
    [Test]
    public void Animation_TestConstructor()
    {
        var texture = StubTexture2D.GetInstance();
        var animation = new Animation(texture, 0.5f, true);
        Assert.That(animation.IsLooping, Is.True);
        Assert.That(animation.FrameHeight == 0, Is.True);
        Assert.That(animation.FrameWidth == 0, Is.True);
        Assert.That(Math.Abs(animation.FrameTime - 0.5) < 0.01, Is.True);
    }
}