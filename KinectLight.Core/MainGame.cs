using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace KinectLight.Core
{
    public class MainGame : IDisposable
    {
        Stopwatch gameTime = new Stopwatch();
        DeviceContext device = null;


        public void Run() {
            
            while (true)
            {
                var currentTime = gameTime.ElapsedMilliseconds;
        
                Update(currentTime);
                Render();
            }

        }

        public void Update(long gameTime) { 
        
        }

        public void Render() { 
        
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
