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
            sensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);

            LeftPos.X = 400;
            LeftPos.Y = 400;

            RightPos.X = 900;
            RightPos.Y = 400;



            if (sensor != null)
            {

                
                sensor.SkeletonStream.Enable( new TransformSmoothParameters
                                             {
                                                 Smoothing = 0.5f,
                                                 Correction = 0.5f,
                                                 Prediction = 0.5f,
                                                 JitterRadius = 0.1f,
                                                 MaxDeviationRadius = 0.04f
                                             });
                sensor.DepthStream.Enable();
                sensor.Start();

                var stream = Observable.FromEventPattern<SkeletonFrameReadyEventArgs>(eh => sensor.SkeletonFrameReady += eh, eh => sensor.SkeletonFrameReady -= eh)
                        .Select(frameReady =>
                        {
                            using (var frame = frameReady.EventArgs.OpenSkeletonFrame())
                            {
                                if (frame != null)
                                {
                                    var res = new Skeleton[frame.SkeletonArrayLength];
                                    frame.CopySkeletonDataTo(res);
                                    return res.Where(r => r.TrackingState == SkeletonTrackingState.Tracked).OrderBy(r => r.Position.Z).FirstOrDefault();
                                }
                                else
                                    return null;
                            }
                        }).Where(s => s != null);

                skeletonStream = stream.Subscribe(s =>
                {
                    var left = sensor.MapSkeletonPointToDepth(s.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
                    var right = sensor.MapSkeletonPointToDepth(s.Joints[JointType.HandRight].Position, DepthImageFormat.Resolution640x480Fps30);

                    //positional data doesnt seem to go all the way to the edge for some reason
                    LeftPos.X = (float)left.X / 440 * (float)MainGame.Instance.Height;
                    LeftPos.Y = (float)left.Y / 640 * (float)MainGame.Instance.Width;

                    RightPos.X = (float)right.X / 440 * (float)MainGame.Instance.Height;
                    RightPos.Y = (float)right.Y / 640 * (float)MainGame.Instance.Width;
                });
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

            target.FillEllipse(new Ellipse(new DrawingPointF(LeftPos.X, LeftPos.Y), 20, 20), Fill);
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
