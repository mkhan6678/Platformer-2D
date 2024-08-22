using Microsoft.Xna.Framework;

namespace Platformer2D.Test;

public class RectangleExtensionsTest
{
  /**
  * Rectangle test intersect with rectangle
  */
  [TestCase(new int[] {0,0,6,6}, new int[] {5,5,3,4}, ExpectedResult = new int[] {-1,-1},
    TestName = "GetIntersectionDepth_OverlappingRects{p} = [-1,-1]")]
  [TestCase(new int[] {5,5,5,5}, new int[] {0,0,6,6}, ExpectedResult = new int[] {1,1},
    TestName = "GetIntersectionDepth_OverlappingRects{p} = [1,1]")]
  [TestCase(new int[] {0,0,5,5}, new int[] {0,5,5,5}, ExpectedResult = new int[] {0, 0},
    TestName = "GetIntersectionDepth_TouchingRects{p} = [0,0]")]
  [TestCase(new int[] {0,0,7,2}, new int[] {0,0,7,2}, ExpectedResult = new int[] {-7, -2},
    TestName = "GetIntersectionDepth_FullOverlap{p} = [-7,-2]")]
  public int[] GetIntersectionDepth_IntersectingRectangles_ReturnsIntersection(int[] rect1, int[] rect2)
  {
    Rectangle a = new Rectangle(rect1[0], rect1[1], rect1[2], rect1[3]);
    Rectangle b = new Rectangle(rect2[0], rect2[1], rect2[2], rect2[3]);
    var result = a.GetIntersectionDepth(b);
    System.Console.WriteLine("Intersection: {0}", result);
    return new int[] { (int)result.X, (int)result.Y };
  }

  /**
  * Rectangle test get bottom center
  */
  [TestCase(new int[] {0,0,6,6}, ExpectedResult = new float[] {3,6},
    TestName = "GetBottomCenter_EvenWidth{p} = [3,6]")]
  [TestCase(new int[] {0,1,3,5}, ExpectedResult = new float[] {1.5f,6},
    TestName = "GetBottomCenter_OddWidth{p} = [1.5,5]")]
  public float[] GetBottomCenter_Rect(int[] rect)
  {
    Rectangle a = new Rectangle(rect[0], rect[1], rect[2], rect[3]);
    var result = a.GetBottomCenter();
    System.Console.WriteLine("Bottom Center: {0}", result);
    return new float[] { result.X, result.Y };
  }
}