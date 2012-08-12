using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace KinectLight.Core
{
    public class ThingBase: IDisposable
    {
        Vector3 position, velocity;

        public virtual void Render() { 
        
        }


        public virtual void Initialize() { 
        
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
