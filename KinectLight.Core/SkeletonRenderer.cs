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
    public class SkeletonRenderer
    {
        KinectSensor sensor = null;
        IObservable<Skeleton> skeletonStream = null;
        Matrix _skeletonTransform = Matrix.Identity;

        public SkeletonRenderer()
        {
            sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (sensor != null)
            {
                sensor.SkeletonStream.Enable();
                sensor.Start();

                skeletonStream = Observable.FromEventPattern<SkeletonFrameReadyEventArgs>(eh => sensor.SkeletonFrameReady += eh, eh => sensor.SkeletonFrameReady -= eh)
                        .Select(frameReady =>
                        {
                            using (var frame = frameReady.EventArgs.OpenSkeletonFrame())
                            {
                                var res = new Skeleton[frame.SkeletonArrayLength];
                                frame.CopySkeletonDataTo(res);
                                return res[0];
                            }
                        });

                var apa = skeletonStream.Select(s =>  new Vector3(s.Joints[JointType.HandLeft].Position.X, s.Joints[JointType.HandLeft].Position.Y, s.Joints[JointType.HandLeft].Position.Z));
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


        public void Render(RenderTarget target)
        {

        }

        internal bool HitTest(ThingBase thing)
        {
            return false;
            return Vector3.Distance(thing.Position, thing.Position) < 20;
        }
    }
}
