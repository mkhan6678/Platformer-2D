using Microsoft.Xna.Framework;

namespace Platformer2D.Test;

public class TileTest
{
    /**
    * Tile test constructor with default size
    */
    [Test]
    public void Tile_ExpectInitialTileSize_ToBe40W32H()
    {
        var expectedSize = new Vector2(40, 32);
        var actualSize = Tile.Size;
        Assert.That(actualSize, Is.EqualTo(expectedSize));
    }

    /**
    * Tile test constructor with default collision type
    */
    [Test]
    public void Tile_ExpectInitializationWithDifferentCollisionType_ToBeSuccess()
    {
        var tile = new Tile(StubTexture2D.GetInstance(), TileCollision.Impassable);
        Assert.That(tile.Collision, Is.EqualTo(TileCollision.Impassable));
        tile.Collision = TileCollision.Passable;
        Assert.That(tile.Collision, Is.EqualTo(TileCollision.Passable));
        tile.Collision = TileCollision.Platform;
        Assert.That(tile.Collision, Is.EqualTo(TileCollision.Platform));
        
    }
}