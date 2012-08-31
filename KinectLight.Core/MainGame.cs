using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;

namespace KinectLight.Core
{
    public class MainGame : IDisposable
    {
        Random r = new Random();
        List<ThingBase> _things = new List<ThingBase>();
        SkeletonRenderer _skeleton = new SkeletonRenderer();
        List<ThingBase> _thingsToRemove = new List<ThingBase>();
        public double Height { get; set; }
        public double Width { get; set; }
        public TextFormat TextFormat { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }
        bool resourcesInitialized = false;
        public string Player { get; set; }
        public int Score { get; set; }

        static MainGame _instance;

        public static MainGame Instance {
            get {

                if (_instance == null)
                    _instance = new MainGame();
                return _instance;
            }
        }

        public MainGame()
        {
            FactoryDWrite = new SharpDX.DirectWrite.Factory();
            TextFormat = new TextFormat(FactoryDWrite, "Arial", 32) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center };
        }

        public void InitializeGlobalResources(RenderTarget target)
        {
            if (!resourcesInitialized)
            {
                SceneColorBrush = new SolidColorBrush(target, Colors.White);
                resourcesInitialized = true;
            }

        }

        public void Update(GameTime gameTime)
        {


            for (int i = 0; i < _things.Count; i++)
            {
                _things[i].Update(gameTime);
                if (_skeleton.HitTest(_things[i]))
                {
                    _thingsToRemove.Add(_things[i]);
                    Score++;
                }
            }

            foreach (var thing in _things.Where(t => t.Position.Y > Height))
                _thingsToRemove.Add(thing);

            for (int i = 0; i < _thingsToRemove.Count; i++)
            {
                _things.Remove(_thingsToRemove[i]);
            }

            _thingsToRemove.Clear();

            if (gameTime.WorldTime % 1000 == 0 && _things.Count() < 10)
                _things.Add(new GoodThing() { Position = new Vector3((float)(r.NextDouble() * Width), 0, 0), Velocity = new Vector3(0, 20 + (float)(r.NextDouble() * 10), 0) });


        }

        public void Render(RenderTarget target)
        {
            InitializeGlobalResources(target);

            foreach (var thing in _things)
                thing.Render(target);

            _skeleton.Render(target);

            target.Transform = Matrix3x2.Identity;
            target.DrawText("Player: " + Player, TextFormat, new RectangleF(5, 5, 800, 70), SceneColorBrush);
            target.DrawText("Score: " + Score, TextFormat, new RectangleF(5, 80, 800, 70), SceneColorBrush);
            
        }

        public void Dispose()
        {
            foreach (var t in _things)
                t.Dispose();

            _skeleton.Dispose();
        }

    }
}
