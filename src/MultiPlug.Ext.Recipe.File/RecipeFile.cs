using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MultiPlug.Base.Exchange;
using MultiPlug.Ext.Recipe.File.Models;
using MultiPlug.Extension.Core;
using MultiPlug.Ext.Recipe.File.Properties;
using MultiPlug.Extension.Core.Exchange;
using MultiPlug.Extension.Core.Http;

namespace MultiPlug.Ext.Recipe.File
{
    public class RecipeFile : MultiPlugExtension, IRecipe
    {
        public event EventHandler<Extension.Core.Exchange.Recipe> RecipeUpdate;

        private SettingsModel m_Settings = new SettingsModel { ExtensionNames = new List<string>(), FilePath = "", LastRead = "Never" };
        readonly HttpEndpoint[] m_Dashboards;

        public RecipeFile()
        {
            m_Settings.FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "base.json");
            m_Dashboards = new HttpEndpoint[] { new CSettingsView(m_Settings) };
        }

        public override HttpEndpoint[] HttpEndpoints
        {
            get
            {
                return m_Dashboards;
            }
        }

        public override RazorTemplate[] RazorTemplates
        {
            get
            {
                return new RazorTemplate[]
                {
                    new RazorTemplate("SimpleFileConfiguratorSettingsView", Resources.settings)
                };
            }
        }

        private Extension.Core.Exchange.Recipe LoadFile()
        {
            Extension.Core.Exchange.Recipe Result;

            var json = System.IO.File.ReadAllText(m_Settings.FilePath);

            Result = new Extension.Core.Exchange.Recipe { Extensions = KeyValuesJson.Parse(json) };

            return Result;
        }

        public void RecipeLoad()
        {
            Extension.Core.Exchange.Recipe config = null;
            var nownow = DateTime.Now;

            try
            {
                config = LoadFile();
            }
            catch
            {
                config = new Extension.Core.Exchange.Recipe();
                config.Extensions = new KeyValuesJson[0];

                m_Settings.LastRead = "Read Error! at " + nownow.ToString("d/M/yyyy") + " at " + nownow.ToString("h:mm:ss");
                RecipeUpdate?.Invoke(this, config);
                return;
            }

            m_Settings.ExtensionNames = (config != null) ? config.Extensions.Select(e => e.Key).ToList() : new List<string>();         
            m_Settings.LastRead = nownow.ToString("d/M/yyyy") + " at " + nownow.ToString("h:mm:ss");

            RecipeUpdate?.Invoke(this, config);
        }

        private void SendConfiguration()
        {
            // Unused?
        }

        public void RecipeSave(Extension.Core.Exchange.Recipe theRecipe)
        {
            Extension.Core.Exchange.Recipe LoadedFromFile = null;

            try
            {
                LoadedFromFile = LoadFile();
            }
            catch
            {
                // File not found ?
            }

            var configList = theRecipe.Extensions.ToList();

            if (LoadedFromFile != null)
            {
                LoadedFromFile.Extensions.ToList().ForEach(e =>
                {
                    if (configList.Find(ce => ce.Key == e.Key) == null)
                    {
                        configList.Add(e);
                    }
                });
            }

            var json = KeyValuesJson.Stringify( configList.ToArray() );

            using (Stream stream = new FileStream(m_Settings.FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                }
            }
        }

        public override object Save()
        {
            var result = new List<Payload>(); 

            Type type = m_Settings.GetType();

            List<Pair> Pairs = new List<Pair>();

            foreach (var Prop in type.GetProperties())
            {
                Pairs.Add(new Pair( Prop.Name, Prop.GetValue(m_Settings, null).ToString() ) );
            }

            result.Add(new Payload( string.Empty, Pairs.ToArray() ));
            
            return result;
        }

    }
}
