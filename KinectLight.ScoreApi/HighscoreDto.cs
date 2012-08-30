using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectLight.ScoreApi
{
    public class HighscoreDto
    {
        public string user { get; set; }
        public IList<ScoreDto> game_score { get; set; }
    }
}
