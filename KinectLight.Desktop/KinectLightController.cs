using KinectLight.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace KinectLight.Desktop
{
    public class KinectLightController : ApiController
    {

        [AcceptVerbs("PUT")]
        public string ChangePlayer(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                MainGame.Instance.Player = name;
                return "Done";
            }
            else
                return "Player name must not be blank";
        }
    }
}
