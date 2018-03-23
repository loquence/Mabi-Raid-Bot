using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Raid_Bot
{
    public class SaveRaid
    {
        static string raidPath = "C://Users//ryand//source//repos//Raid Bot//Raid Bot//db//SavedRaids.xml";
        public static void SerializeRaid(RaidTimer obj)
        {
            List<RaidTimer> objects = new List<RaidTimer>();
            if (File.Exists(raidPath))
            {
                Console.WriteLine("It exists");
                objects = DeSerializeObj();
            }
            if(obj == null) { return; }
            objects.Add(obj);
            try
            {
                XmlSerializer writer = new XmlSerializer(objects.GetType());
                


                System.IO.FileStream file = System.IO.File.Create(raidPath);

                writer.Serialize(file, objects);
                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to write to db: {0}", e.ToString());
            }
        }

        public static List<RaidTimer> DeSerializeObj()
        {
            List<RaidTimer> objects = new List<RaidTimer>();
            XmlSerializer reader = new XmlSerializer(objects.GetType());
            try
            {
                TextReader textReader = new StreamReader(@"C://Users//ryand//source//repos//Raid Bot//Raid Bot//db//SavedRaids.xml");
                objects = (List<RaidTimer>)reader.Deserialize(textReader);
                textReader.Close();
                return objects;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read from db : {0}", e.ToString());
                return null;
            }
        }
    }
}
