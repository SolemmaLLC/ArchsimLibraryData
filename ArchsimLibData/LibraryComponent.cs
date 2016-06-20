using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArchsimLib
{

    public class Units : Attribute
    {
        public Units(string _unit) {
            Unit = _unit;
        }
        public string Unit { get; set; }
    }


    [DataContract]
    public abstract class LibraryComponent
   {


        [DataMember, DefaultValue("No comments")]
        public string Comment { get; set; } = "No comments";

        [DataMember, DefaultValue("No data source")]
        public string DataSource { get; set; } = "No data source";

        [DataMember, DefaultValue("No Category")]
        public string Category { get; set; } = "No Category";

        [DataMember, DefaultValue("No name")]
        public string Name { get; set; } = "No name";


        [OnDeserializing]
        public void OnDeserializing(StreamingContext context)
        {
            foreach (var prop in GetType().GetProperties())
            {
                var att = prop.GetCustomAttribute<DefaultValueAttribute>();
                if (att == null) { continue; }
                prop.SetValue(this, att.Value);
            }
        }

        public string Serialize() { return Serialization.Serialize(this); }

        public static T Deserialize<T>(string context)
        {
            return Serialization.Deserialize<T>(context);
        }


        public override string ToString()
        {
            return  Name;
        }

    }
}
