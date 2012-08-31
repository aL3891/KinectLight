using Microsoft.Kinect;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using SharpDX;

namespace KinectLight.Core
{
    public class SkeletonRenderer : IDisposable
    {
        KinectSensor sensor = null;

        Matrix _skeletonTransform = Matrix.Identity;
        Vector3 LeftPos = Vector3.Zero, RightPos = Vector3.Zero;
        IDisposable skeletonStream;
        Brush Fill = null, Stroke = null;
        bool initialized = false;

        public SkeletonRenderer()
        {
            sensor = KinectSensor.KinectSensors.FirstOrDefault();

            LeftPos.X = 100;
            LeftPos.Y = 200;

            RightPos.X = 300;
            RightPos.Y = 200;

            if (sensor != null)
            {
                sensor.SkeletonStream.Enable();
                sensor.Start();

                var stream = Observable.FromEventPattern<SkeletonFrameReadyEventArgs>(eh => sensor.SkeletonFrameReady += eh, eh => sensor.SkeletonFrameReady -= eh)
                        .Select(frameReady =>
                        {
                            using (var frame = frameReady.EventArgs.OpenSkeletonFrame())
                            {
                                var res = new Skeleton[frame.SkeletonArrayLength];
                                frame.CopySkeletonDataTo(res);
                                return res[0];
                            }
                        });

                skeletonStream = stream.Subscribe(s =>
                {
                    var left = sensor.MapSkeletonPointToDepth(s.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
                    var right = sensor.MapSkeletonPointToDepth(s.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
                });
            }
            else
            {
                Observable.Generate(new Skeleton(), s => true, s =>
                {
                    //s.Joints[JointType.HandLeft] = new Joint() { Position = new SkeletonPoint { }, JointType= JointType.HandLeft };
                    return s;
                }, s => s, s => TimeSpan.FromMilliseconds(33));
            }

        }

        internal void InitializeResources(RenderTarget d2dRenderTarget)
        {
            if (!initialized)
            {
                Fill = new SolidColorBrush(d2dRenderTarget, Colors.Purple);
                Stroke = new SolidColorBrush(d2dRenderTarget, Colors.Plum);
                initialized = true;
            }
        }

        public void Render(RenderTarget target)
        {
            InitializeResources(target);
            target.Transform = Matrix.Identity;
            
            target.FillEllipse(new Ellipse(new DrawingPointF(LeftPos.X,LeftPos.Y), 20, 20), Fill);
            target.DrawEllipse(new Ellipse(new DrawingPointF(LeftPos.X, LeftPos.Y), 20, 20), Stroke);


            target.FillEllipse(new Ellipse(new DrawingPointF(RightPos.X, RightPos.Y), 20, 20), Fill);
            target.DrawEllipse(new Ellipse(new DrawingPointF(RightPos.X, RightPos.Y), 20, 20), Stroke);
        }

        internal bool HitTest(ThingBase thing)
        {
            return Vector3.Distance(thing.Position, LeftPos) < 45 || Vector3.Distance(thing.Position, RightPos) < 45;
        }

        public void Dispose()
        {
            if (sensor != null)
            {
                sensor.Stop();

                skeletonStream.Dispose();

                sensor.Dispose();
            }
        }
    }
}
