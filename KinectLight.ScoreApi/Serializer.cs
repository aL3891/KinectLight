using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace KinectLight.ScoreApi
{
    public static class Serializer
    {
        public static string Serialize<T>(T dto)
        {
            using (MemoryStream stream1 = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(stream1, dto);
                stream1.Position = 0;
                StreamReader sr = new StreamReader(stream1);
                return sr.ReadToEnd();
            }
        }

        public static T[] Deserialize<T>(string s)
        {
            using (MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T[]));
                //ser.WriteObject(stream1, s);
                
                stream1.Position = 0;
                return (T[])ser.ReadObject(stream1);
            }
        }
    }
}
