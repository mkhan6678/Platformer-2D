using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Moq;

namespace Platformer2D.Test;

public class LevelTest
{
    // mock content object
    private readonly Mock<StubContentManager> mockContent;

    /**
    * Level test initialization with stub content manager
    */
    public LevelTest()
    {
        mockContent = new Mock<StubContentManager>();
    }

    /**
    * Setup mock content load Texture2D before every test
    */
    [SetUp]
    public void BeforeEach()
    {
        mockContent.Setup(cm =>
            cm.Load<Texture2D>(It.IsAny<string>())).Returns(StubTexture2D.GetInstance());
    }

    /**
    * Level test load content from stream
    */
    [Test]
    public void Level_ExpectLoadLevelContentFromStreamSuccess_WhenInstantiate()
    {
        string levelString = ".~:-1#ABCDGX";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(levelString));
        var l = new Level(null, fileStream, 1, mockContent.Object);
        l.Dispose();
    }

    /**
    * Level test exception when content lines length are inconsistent
    */
    [Test]
    public void Level_ExpectException_WhenContentLineLengthInConsistent()
    {
        string levelString = ".~:-1#ABCDGX\n.";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(levelString));
        var thrown = Assert.Throws<Exception>(() =>
        {
            var l = new Level(null, fileStream, 1, mockContent.Object);
            l.Dispose();
        });
        Assert.That(thrown?.Message, Is.EqualTo("The length of line 2 is different from all preceeding lines."));
    }

    /**
    * Level test exception when content is invalid in various ways
    */
    [TestCase(".~:-1#ABCDGX*", ExpectedResult = "Unsupported tile type character '*' at position 12, 0.",
        TestName = "Level_ExpectException_WhenContentHasUnsupportedTileCharacter")]
    [TestCase(".~:-#ABCDGX", ExpectedResult = "A level must have a starting point.",
        TestName = "Level_ExpectException_WhenContentHasNoPlayer")]
    [TestCase(".~:-1#ABCDG", ExpectedResult = "A level must have an exit.",
        TestName = "Level_ExpectException_WhenContentHasNoExit")]
    [TestCase(".~:-1#ABCD1GX", ExpectedResult = "A level may only have one starting point.",
        TestName = "Level_ExpectException_WhenContentHasMoreThanOnePlayer")]
    [TestCase(".~:-1#ABCDXGX", ExpectedResult = "A level may only have one exit.",
        TestName = "Level_ExpectException_WhenContentHasMoreThanOneExit")]
    public string Level_ExpectNotSupportedException_WhenContentHasIsInvalid(string levelString)
    {
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(levelString));
        var thrown = Assert.Throws<NotSupportedException>(() =>
        {
            var l = new Level(null, fileStream, 1, mockContent.Object);
            l.Dispose();
        });
        return thrown?.Message + "";
    }

    /**
    * Level test collition type when loading content tile
    */
    [TestCase(2, 2, 1, ":::\n~~~\n###\nAGX\n.-1",
        TestName = "Level_ExpectImpassableTile_When#CharacterLoaded")]
    [TestCase(1, 1, 2, ":::\n~~~\n###\nAGX\n.-1",
        TestName = "Level_ExpectPlatformTile_When~CharacterLoaded")]
    [TestCase(0, 0, 0, ":::\n~~~\n###\nAGX\n.-1",
        TestName = "Level_ExpectPassableTile_When:CharacterLoaded")]
    [TestCase(-1, 0, 1, "1X:",
        TestName = "Level_ExpectImpassableTile_WhenEscapingPastTheLevelLeftEnds")]
    [TestCase(3, 0, 1, "1X:",
        TestName = "Level_ExpectImpassableTile_WhenEscapingPastTheLevelRightEnds")]
    [TestCase(0, -1, 0, "1X:",
        TestName = "Level_ExpectPassableTile_WhenFallingThroughTheBottom")]
    [TestCase(0, 1, 0, "1X:",
        TestName = "Level_ExpectPassableTile_WhenJumpingPastTheLevelTop")]
    public void Level_ExpectCorrectCollisionType_WhenLoadingTile(int x, int y, int collisionTypeValue,
        string levelString)
    {
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(levelString));
        var l = new Level(null, fileStream, 1, mockContent.Object);
        var tileCollision = l.GetCollision(x, y);
        Assert.That(tileCollision, Is.EqualTo((TileCollision)collisionTypeValue));
    }
    
    /**
    * Level test update method
    */
    [TestCase(false, false, new int[] { 0, 0, 0, 0 }, 2.1,
        ExpectedResult = 0,
        TestName = "Level_Update_PauseWhileThePlayerIsDead")]
    [TestCase(true, true, new int[] { 0, 0, 0, 0 }, 2.1,
        ExpectedResult = 0,
        TestName = "Level_Update_PausedWhileThePlayerRunOutOfTime")]
    [TestCase(true, true, new int[] { 780, 16, 1, 1 }, 1.9,
        ExpectedResult = 30,
        TestName = "Level_Update_WhileThePlayerReachExit")]
    [TestCase(true, false, new int[] { 0, 65, 0, 0 }, 1.9,
        ExpectedResult = 0,
        TestName = "Level_Update_PlayerDeadFallingOffTheBottomOfTheLevel")]
    public int TestUpdate(bool isAlive, bool isOnGround, int[] rec, double minutes)
    {
        string levelString = "1GAGBGCGDGGGGGGGGGGX\n####################";
        var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(levelString));
        var rectangle = new Rectangle(rec[0], rec[1], rec[2], rec[3]);
        var kb = new KeyboardState();
        var gp = new GamePadState();
        var ac = new AccelerometerState();
        var dp = new DisplayOrientation();
        var gt = new GameTime(TimeSpan.Zero, TimeSpan.FromMinutes(minutes));
        var mockPlayer = new Mock<IPlayer>();

        mockPlayer.Setup(p => p.Update(
            It.IsAny<GameTime>(),
            It.IsAny<KeyboardState>(),
            It.IsAny<GamePadState>(),
            It.IsAny<AccelerometerState>(),
            It.IsAny<DisplayOrientation>()));
        mockPlayer.Setup(p => p.IsAlive).Returns(isAlive);
        mockPlayer.Setup(p => p.IsOnGround).Returns(isOnGround);
        mockPlayer.Setup(p => p.BoundingRectangle).Returns(rectangle);

        var l = new Level(null, fileStream, 1, mockContent.Object, mockPlayer.Object);
        l.Update(gt, kb, gp, ac, dp);
        l.Update(gt, kb, gp, ac, dp);
        return l.Score;
    }
}