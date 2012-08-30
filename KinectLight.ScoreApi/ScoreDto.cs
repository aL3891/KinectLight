using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectLight.ScoreApi
{
    public class ScoreDto : BaseDto
    {
        public string user { get; set; }
        public string game { get; set; }
        public string points { get; set; }
    }
}
