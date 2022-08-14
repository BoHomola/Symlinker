using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using SymLinker.DataEntities;

namespace SymLinker.Core
{
    public class AliasLoader : IDataLoader<Aliases>
    {
        private readonly string FILE_NAME = "Aliases.xml";
        public Aliases LoadData()
        {
            if (!CheckFileExistence())
                return new Aliases();


            XmlSerializer reader = new XmlSerializer(typeof(Aliases));
            StreamReader file = new StreamReader(FILE_NAME);
            Aliases? aliases = (Aliases?)reader.Deserialize(file);
            file.Close();
            if (aliases == null)
                return new Aliases();
            return aliases;

        }

        public void SaveData(Aliases aliases)
        {
            var writer = new XmlSerializer(typeof(Aliases));
            var wfile = new StreamWriter(FILE_NAME);
            writer.Serialize(wfile, aliases);
            wfile.Close();
        }

        private bool CheckFileExistence()
        {
            if (File.Exists(FILE_NAME)) return true;
            return false;
        }
    }
}
