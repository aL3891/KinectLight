﻿using KinectLight.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace KinectLight.Desktop
{
    public class KinectLightController :ApiController
    {
        
        public string Get(string name) {
            MainGame.Instance.Player = name;
            return "hiarg?";
        }
    }
}