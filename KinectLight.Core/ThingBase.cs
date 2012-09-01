using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace KinectLight.Core
{
    public class ThingBase : IDisposable
    {


        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        Brush Fill = null, Stroke = null;
        bool initialized = false;
        Bitmap texture = null;

        public virtual void Render()
        {

        }


        public virtual void Initialize()
        {

        }

        public void Dispose()
        {

        }

        internal void Update(GameTime gameTime)
        {
            Position = Position + (Velocity * (float)(gameTime.FrameDeltaTime /1000));
        }


        internal void InitializeResources(RenderTarget d2dRenderTarget)
        {
            if (!initialized)
            {
                Fill = new SolidColorBrush(d2dRenderTarget, Colors.Green);
                Stroke = new SolidColorBrush(d2dRenderTarget, Colors.Azure);
                initialized = true;
                texture = MainGame.LoadFromFile(d2dRenderTarget, "c:\apa.jpg");
            }
        }

        internal void Render(RenderTarget d2dRenderTarget)
        {
            
            d2dRenderTarget.Transform = Matrix.Translation(Position);
            d2dRenderTarget.DrawBitmap(texture,1, BitmapInterpolationMode.Linear);
            
        }
    }
}
