using Microsoft.Kinect;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;

namespace KinectLight.Core
{
    public class SkeletonRenderer
    {
        KinectSensor sensor = null;
        IObservable<Skeleton> skeletonStream = null;

        public SkeletonRenderer()
        {
            sensor = KinectSensor.KinectSensors.FirstOrDefault();

            if (sensor != null)
            {
                sensor.SkeletonStream.Enable();
                sensor.Start();
                sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;

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
            }
            else {
                Observable.Generate(new Skeleton(), s => true, s => {
                    //s.Joints[JointType.HandLeft] = new Joint() { Position = new SkeletonPoint { }, JointType= JointType.HandLeft };
                    return s;
                }, s => s, s => TimeSpan.FromMilliseconds(33));
            }

        }

        void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Render(RenderTarget target)
        {

        }
    }
}
