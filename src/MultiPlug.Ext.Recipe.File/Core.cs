using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPlug.Extension.Core.Exchange;
using Newtonsoft.Json.Linq;

namespace MultiPlug.Ext.Recipe.File
{
    internal class Core
    {
        private static Core m_Instance = null;

        public event EventHandler<Extension.Core.Exchange.Recipe> Loaded;

        public List<string> ExtensionNames { get; internal set; } = new List<string>();
        public string FilePath { get; set; }
        public string LastRead { get; internal set; }
        public string LastWrite{ get; internal set; }

        public static Core Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Core();
                }
                return m_Instance;
            }
        }

        private Core()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "base.json");
            LastRead = "Never";
        }

        private Extension.Core.Exchange.Recipe LoadFile()
        {
            Extension.Core.Exchange.Recipe Result;

            var json = System.IO.File.ReadAllText(FilePath);

            Result = new Extension.Core.Exchange.Recipe { Json = json };

            return Result;
        }

        internal void Push(string theAssembly, string theJson)
        {
            try
            {
                RecipeItem item = new RecipeItem { Assembly = theAssembly, Properties = JObject.Parse(theJson) };
                RecipeCollection Collection = new RecipeCollection { Extensions = new RecipeItem[] { item }};
                var Recipe = new Extension.Core.Exchange.Recipe();
                Recipe.ToJson(Collection);
                Loaded?.Invoke(this, Recipe);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
            }
        }

        internal string Load( string theAssembly )
        {
            Extension.Core.Exchange.Recipe config = LoadFile();
            RecipeCollection Object = Extension.Core.Exchange.Recipe.ToObject(config.Json);

            var Search = Object.Extensions.FirstOrDefault(e => e.Assembly.Equals(theAssembly, StringComparison.OrdinalIgnoreCase));

            return (Search != null) ? Search.Properties.ToString().Trim() : string.Empty;
        }

        internal void Load()
        {
            Extension.Core.Exchange.Recipe config = null;
            var nownow = DateTime.Now;

            RecipeCollection Object = null;

            try
            {
                config = LoadFile();
                Object = Extension.Core.Exchange.Recipe.ToObject(config.Json);
            }
            catch
            {
                config = new Extension.Core.Exchange.Recipe { Json = string.Empty };
                // config.Extensions = new KeyValuesJson[0];

                LastRead = "Read Error! at " + nownow.ToString("d/M/yyyy") + " at " + nownow.ToString("h:mm:ss");
                Loaded?.Invoke(this, config);
            }

            ExtensionNames = (Object != null) ? Object.Extensions.Select(e => e.Assembly).ToList() : new List<string>();
            LastRead = nownow.ToString("d/M/yyyy") + " at " + nownow.ToString("h:mm:ss");

            Loaded?.Invoke(this, config);
        }

        internal void Save(Extension.Core.Exchange.Recipe theRecipe)
        {
            var json = theRecipe.Json;

            using (Stream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                }
            }

            var nownow = DateTime.Now;
            LastWrite
                 = nownow.ToString("d/M/yyyy") + " at " + nownow.ToString("h:mm:ss");
        }
    }
}
