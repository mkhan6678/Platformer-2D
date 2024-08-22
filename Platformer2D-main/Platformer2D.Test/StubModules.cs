using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer2D.Test
{
    /**
    * Stub content manager for testing purpose only
    */
    public class StubContentManager : ContentManager
    {
        class SP : IServiceProvider
        {
            public object? GetService(Type serviceType) { return new object(); }
        }
        public StubContentManager() : base(new SP()) { }
    }

    /**
    * Stub texture2D for testing purpose only
    */
    public class StubTexture2D : Texture2D
    {
        private static volatile StubTexture2D _instance = null!;
        private StubTexture2D() : this(() => throw new Exception()) { }
        private StubTexture2D(Func<GraphicsDevice> func) : this(func()) { }
        private StubTexture2D(GraphicsDevice g) : base(g, 0, 0) { }

        // pick up from garbage
        ~StubTexture2D() { _instance = this; }
        public static StubTexture2D GetInstance()
        {
            StubTexture2D local;
            while ((local = _instance) == null)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
                try { new StubTexture2D(); } catch {  }
            }
            local.Name = "";
            return local;
        }
    }
}