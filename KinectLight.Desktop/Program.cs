using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using KinectLight.Core;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using SharpDX.Windows;

namespace KinectLight.Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var form = new RenderForm("SharpDX - MiniCube Direct3D11 Sample");

            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            SharpDX.Direct3D10.Device1 device;
            SwapChain swapChain;
            SharpDX.Direct3D10.Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, SharpDX.Direct3D10.FeatureLevel.Level_10_0, out device, out swapChain);

            var d2dFactory = new SharpDX.Direct2D1.Factory();
            //var surface = Surface.FromSwapChain(swapChain, 0);

            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            Surface surface = backBuffer.QueryInterface<Surface>();

            var d2dRenderTarget = new RenderTarget(d2dFactory, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            MainGame game = new MainGame();

            Stopwatch gameTime = new Stopwatch();
            gameTime.Start();

            RenderLoop.Run(form, () =>
            {
                var currentTime = gameTime.ElapsedMilliseconds;
                game.Update(currentTime);
                d2dRenderTarget.BeginDraw();
                d2dRenderTarget.Clear(Colors.Black);
                game.Render(d2dRenderTarget);
                var res  = d2dRenderTarget.EndDraw();
                swapChain.Present(0, PresentFlags.None);
            });

            game.Dispose();
            d2dRenderTarget.Dispose();
            surface.Dispose();
            d2dFactory.Dispose();
            device.Dispose();
            swapChain.Dispose();
        }
    }
}
