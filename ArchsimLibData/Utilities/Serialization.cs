using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArchsimLib
{
    public static class Serialization
    {

        public static T Deserialize<T>(string json)
        {
            return DeserializeJsonNet<T>(json);
        }
        public static string Serialize<T>(T component)
        {
            return SerializeJsonNet<T>(component);
        }



        private static T DeserializeXml<T>(string xml)
        {
            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)));
        }

        private static string SerializeXml<T>(T component)
        {
            var dcs = new DataContractSerializer(typeof(T));
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                dcs.WriteObject(mem, component);
                mem.Position = 0;
                return reader.ReadToEnd();
            }
        }

        private static T DeserializeBin<T>(MemoryStream binData)
        {
            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(binData);
        }


        public  static byte[] SerializeBin<T>(T component)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream1, component);
            stream1.Position = 0;
                
                using (MemoryStream ms = new MemoryStream())
            {
                stream1.CopyTo(ms);
                return ms.ToArray();
            }
        }


        private static T DeserializeJson<T>(string json)
        {
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)));
        }

        private static string SerializeJson<T>(T component)
        {
            var dcs = new DataContractJsonSerializer(typeof(T));
            using (var mem = new MemoryStream())
            using (var reader = new StreamReader(mem))
            {
                dcs.WriteObject(mem, component);
                mem.Position = 0;
                return reader.ReadToEnd();
            }
        }


        private static T DeserializeJsonNet<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static string SerializeJsonNet<T>(T component)
        {
            var set = new JsonSerializerSettings();
            set.TypeNameHandling = TypeNameHandling.Auto;
            return JsonConvert.SerializeObject(component, Formatting.Indented, set);
        }



    }
}
