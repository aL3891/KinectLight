using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;
using KinectLight.ScoreApi;
using System.Runtime.InteropServices;


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
        private string _player;
        public string Player
        {
            get { return _player; }
            set
            {
                if (!string.IsNullOrEmpty(_player))
                {
                    _scoreApi.PostScoreAsync<ScoreDto>(new ScoreDto { points = (Score).ToString() }, Player, "kinectgame");

                }

                if (!string.IsNullOrEmpty(value))
                {
                    PlayerStartTime = DateTime.Now;
                    // Assumes that new game is indicated by a new player being set.
                    _scoreApi.PostScoreAsync<ScoreDto>(new ScoreDto { points = Score.ToString() }, Player, "kinectgame");
                    Score = 0;
                    _player = value;

                    playerActive = true;
                }
                else
                {
                    PlayerStartTime = DateTime.MinValue;
                    playerActive = false;
                }

                _player = value;
            }
        }
        public int Score { get; set; }
        public IScoreApi _scoreApi;
        DateTime PlayerStartTime = DateTime.MinValue;
        bool playerActive = false;

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
            _scoreApi = new ScoreClient();
        }

        public void InitializeGlobalResources(RenderTarget target)
        {
            if (!resourcesInitialized)
            {
                SceneColorBrush = new SolidColorBrush(target, Colors.Black);
                resourcesInitialized = true;
            }

        }

        public void Update(GameTime gameTime)
        {


            for (int i = 0; i < _things.Count; i++)
            {
                _things[i].Update(gameTime);
                if (Player != "" && _skeleton.HitTest(_things[i]))
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

            if (gameTime.WorldTime % 1000 > 1 && _things.Count() < 10)
                _things.Add(new GoodThing() { Position = new Vector3((float)(r.NextDouble() * Width), 0, 0), Velocity = new Vector3(0, 20 + (float)(r.NextDouble() * 10), 0) });

            if (Player != "")
            {
                var playtime = DateTime.Now - PlayerStartTime;
                if (playtime.TotalSeconds > 60)
                {
                    Player = "";
                }
            }
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

            if (Player != "")
                target.DrawText((60d - (DateTime.Now - PlayerStartTime).TotalSeconds).ToString("00.0"), TextFormat, new RectangleF((float)(Width / 2) - 20, 15, 800, 70), SceneColorBrush);

        }

        public void Dispose()
        {
            foreach (var t in _things)
                t.Dispose();

            _skeleton.Dispose();
        }

        /// <summary>
        /// Loads a Direct2D Bitmap from a file using System.Drawing.Image.FromFile(...)
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="file">The file.</param>
        /// <returns>A D2D1 Bitmap</returns>
        public static Bitmap LoadFromFile(RenderTarget renderTarget, string file)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(renderTarget, size, tempStream, stride, bitmapProperties);
                }
            }
        }

    }
}
