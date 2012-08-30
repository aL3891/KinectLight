﻿using System;
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

namespace KinectLight.Desktop
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var form = new RenderForm("KinectLight");

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

            MainGame game = new MainGame() { Height = form.ClientSize.Height, Width = form.ClientSize.Width };
            GameTime gameTime = new GameTime();

            RenderLoop.Run(form, () =>
            {
                gameTime.StartFrame();
                game.Update(gameTime);
                dc.BeginDraw();
                dc.Clear(Colors.Black);
                game.Render(dc);
                var res = dc.EndDraw();
                swapChain.Present(0, PresentFlags.None);
                Thread.Sleep(1);
            });

            game.Dispose();
            dc.Dispose();
            surface.Dispose();
            d2dFactory.Dispose();
            device.Dispose();
            swapChain.Dispose();
        }
    }
}
