using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace WeaponsGame.Game
{
    public enum BlueprintState
    {
        Droppable=0,
        Purchaseable,
        Researched,
    }

    public struct Blueprint
    {
       public string name;
        
       public string type;

       public int tier;

       public int cost;

       public BlueprintState state;
        
        public static Blueprint LoadFromFile(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blueprint));
            Blueprint result;
            using (System.IO.TextReader textReader = new System.IO.StreamReader(path))
            {
                object obj = xmlSerializer.Deserialize(textReader);
                result = (Blueprint)obj;
            }
            return result;
        }
        /*
        public void SaveToFile()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blueprint));
            Blueprint result;
            using (System.IO.TextWriter textReader = new System.IO.StreamWriter("tempb.xml"))
            {
                xmlSerializer.Serialize(textReader, this);
            }
        }
         */
    }
}
