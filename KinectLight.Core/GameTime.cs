using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KinectLight.Core
{
    public class GameTime
    {

        Stopwatch stopWatch = new Stopwatch();
        double lastFrame = 0;


        public GameTime()
        {
            stopWatch.Start();
        }

        public double FrameDeltaTime = 0;

        public void StartFrame() {
            FrameDeltaTime = stopWatch.Elapsed.TotalMilliseconds - lastFrame;
            lastFrame = stopWatch.ElapsedMilliseconds;
        }
    }
}
