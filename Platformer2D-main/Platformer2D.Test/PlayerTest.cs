using Moq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace Platformer2D.Test;

public class PlayerTest
{
    // Mock level object
    private ILevel mockLevel;
    // Mock sound effect object
    private ISoundEffect mockSound;
    // Mock animation object
    private IAnimation mockAnimation;
    // Mock sprite object
    private IAnimationPlayer mockSprite;
    // Mock content object
    private StubContentManager mockContent;
    
    /**
    * Setup mock objects before every test
    */
    [SetUp]
    public void Mocks()
    {
        mockLevel = Mock.Of<ILevel>();
        mockSound = Mock.Of<ISoundEffect>();
        mockAnimation = Mock.Of<IAnimation>();
        mockSprite = Mock.Of<IAnimationPlayer>();
        mockContent = Mock.Of<StubContentManager>();
        
        // Return mock content object in level mock
        Mock.Get<ILevel>(mockLevel).Setup(l => l.Content).Returns(mockContent);
        // Setup GetCollision to return a floor.
        Mock.Get<ILevel>(mockLevel).Setup(
            l => l.GetCollision(It.IsAny<int>(), It.IsAny<int>())
        ).Returns((int x, int y) =>
        {
            var result = y >= 14 ? TileCollision.Impassable : TileCollision.Passable;
            System.Console.WriteLine("GetCollision: {0}, {1} = {2}",x,y,result);
            return result;
        });
        // TODO: This is just the normal GetBounds and should probably not be a mock.
        Mock.Get<ILevel>(mockLevel).Setup(
            l => l.GetBounds(It.IsAny<int>(), It.IsAny<int>())
            ).Returns((int x, int y) => new Rectangle(x * Tile.Width, y * Tile.Height, Tile.Width, Tile.Height));
        Mock.Get<IAnimationPlayer>(mockSprite).Setup(
            sp => sp.Origin
        ).Returns(new Vector2(20,0));
        // Set the sprite size to 64x64
        Mock.Get<IAnimation>(mockAnimation).Setup(an => an.FrameHeight).Returns(64);
        Mock.Get<IAnimation>(mockAnimation).Setup(an => an.FrameWidth).Returns(64);
        
    }
    
    /**
    * Player test action with no input or with keyboard input
    */
    [TestCase(0.1f, Keys.Space, ExpectedResult = false, TestName = "GetInput_HoldSpace_CauseJump")]
    [TestCase(0.1f, ExpectedResult = true, TestName = "GetInput_DoNothing_StayOnGround")]
    public bool GetInput(float gameTime, params Keys[] keysArray)
    {
        // Start the player with coords right above our floor
        var player = new Player(mockLevel, new Vector2(47,357),
            this.mockAnimation,
            this.mockAnimation,
            this.mockAnimation,
            this.mockAnimation,
            this.mockAnimation,
            this.mockSprite,
            this.mockSound,
            this.mockSound,
            this.mockSound
        );
        
        KeyboardState keyboardState = new KeyboardState(keysArray);
        var gt = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(gameTime));
        // Let the player recognize its environment.
        player.Update(gt, new KeyboardState(), new GamePadState(), new AccelerometerState(), DisplayOrientation.Default);
        
        // Run update with input
        player.Update(gt, keyboardState, new GamePadState(), new AccelerometerState(), DisplayOrientation.Default);
        return player.IsOnGround;
    }
    
    /**
    * Player test on killed method
    */
    [Test]
    public void KillPlayer_PlayerIsDead()
    {
        // Start the player with coords right above our floor
        var player = new Player(mockLevel, new Vector2(47,357),
            this.mockAnimation,
            this.mockAnimation,
            this.mockAnimation,
            this.mockAnimation,
            this.mockAnimation,
            this.mockSprite,
            this.mockSound,
            this.mockSound,
            this.mockSound
        );
        
        player.OnKilled(null);
        
        Assert.False(player.IsAlive, "Player should be dead");
    }
}