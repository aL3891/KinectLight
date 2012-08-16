using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;

namespace KinectLight.Core
{
    public class MainGame : IDisposable
    {

        Factory d2dFactory;
        

        List<ThingBase> things = new List<ThingBase>();
        SkeletonRenderer skeleton = new SkeletonRenderer();

        public MainGame()
        {
            for (int i = 0; i < 10; i++)
            {
                things.Add(new GoodThing() { Position = new Vector3((i * 50)+50,100, 0), Velocity = new Vector3(0,0.1f,0) });
            }
        }

        public void Update(long gameTime)
        {
            foreach (var thing in things)
                thing.Update(gameTime);



        }

        public void Render(RenderTarget d2dRenderTarget)
        {
            foreach (var thing in things)
                thing.Render(d2dRenderTarget);

        }

        public void Dispose()
        {
            foreach (var t in things)
                t.Dispose();
        }

    }
}
