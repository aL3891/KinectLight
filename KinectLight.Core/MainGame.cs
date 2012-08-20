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
        Random r = new Random();
        List<ThingBase> _things = new List<ThingBase>();
        SkeletonRenderer _skeleton = new SkeletonRenderer();
        public double Height { get; set; }
        public double Width { get; set; }


        public MainGame()
        {

        }

        public void Update(GameTime gameTime)
        {
            foreach (var thing in _things)
                thing.Update(gameTime);

            foreach (var thing in _things.Where(t => t.Position.Y > Height).ToArray())
            {
                _things.Remove(thing);
            }

            if (_things.Count() < 10)
                _things.Add(new GoodThing() { Position = new Vector3((float)(r.NextDouble() * Width), 0, 0), Velocity = new Vector3(0, 20+(float)(r.NextDouble() * 10), 0) });


        }

        public void Render(RenderTarget target)
        {
            foreach (var thing in _things)
                thing.Render(target);

            _skeleton.Render(target);
        }

        public void Dispose()
        {
            foreach (var t in _things)
                t.Dispose();
        }

    }
}
