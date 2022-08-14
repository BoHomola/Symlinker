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
    public class SettingsLoader : IDataLoader<Settings>
    {
        private readonly string FILE_NAME = "settings.xml";
        public Settings LoadData()
        {
            if (!CheckFileExistence())
                return new Settings();


            XmlSerializer reader = new XmlSerializer(typeof(Settings));
            StreamReader file = new StreamReader(FILE_NAME);
            Settings? settings = (Settings?)reader.Deserialize(file);
            file.Close();
            if (settings == null)
                return new Settings();
            return settings;

        }

        public void SaveData(Settings settings)
        {
            var writer = new XmlSerializer(typeof(Settings));
            var wfile = new StreamWriter(FILE_NAME);
            writer.Serialize(wfile, settings);
            wfile.Close();
        }

        private bool CheckFileExistence()
        {
            if (File.Exists(FILE_NAME)) return true;
            return false;
        }
    }
}
