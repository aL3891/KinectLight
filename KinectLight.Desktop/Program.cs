using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using KinectLight.Core;
using System.Diagnostics;
using System.Threading;
using SharpDX.Windows;
using SharpDX.DXGI;
using SharpDX.Direct3D10;
using SharpDX.Direct2D1;
using SharpDX;
using System.Web.Http.SelfHost;
using System.Web.Http;



namespace KinectLight.Desktop
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var form = new RenderForm("KinectLight");
            form.Size = new System.Drawing.Size(1920,1200);

            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            SharpDX.Direct3D10.Device1 device;
            SwapChain swapChain;
            SharpDX.Direct3D10.Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, SharpDX.Direct3D10.FeatureLevel.Level_10_1, out device, out swapChain);

            var d2dFactory = new SharpDX.Direct2D1.Factory();
            var surface = Surface.FromSwapChain(swapChain, 0);

            RenderTarget dc = new RenderTarget(d2dFactory, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            MainGame.Instance.Height = form.ClientSize.Height;
            MainGame.Instance.Width = form.ClientSize.Width;
            GameTime gameTime = new GameTime();



            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{action}/{name}",
                new { id = RouteParameter.Optional });
            

            HttpSelfHostServer server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();




            RenderLoop.Run(form, () =>
            {
                gameTime.StartFrame();
                MainGame.Instance.Update(gameTime);
                dc.BeginDraw();
                dc.Clear(Colors.White);
                MainGame.Instance.Render(dc);
                var res = dc.EndDraw();
                swapChain.Present(1, PresentFlags.None);
                //Thread.Sleep(1);
            });

            server.Dispose();
            MainGame.Instance.Dispose();
            dc.Dispose();
            surface.Dispose();
            d2dFactory.Dispose();
            device.Dispose();
            swapChain.Dispose();
        }
    }
}
