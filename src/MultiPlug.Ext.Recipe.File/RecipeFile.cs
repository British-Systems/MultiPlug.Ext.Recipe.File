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
using MultiPlug.Ext.Recipe.File.Controllers.Settings;

namespace MultiPlug.Ext.Recipe.File
{
    public class RecipeFile : MultiPlugExtension, IRecipe
    {
        readonly HttpEndpoint[] m_Apps;

        public RecipeFile()
        {
            m_Apps = new HttpEndpoint[] { new SettingsApp() };

            Core.Instance.Loaded += OnRecipeLoaded;
        }

        private void OnRecipeLoaded(object sender, Extension.Core.Exchange.Recipe e)
        {
            MultiPlugSignals.Updates.Recipe(e);
        }

        public override HttpEndpoint[] HttpEndpoints
        {
            get
            {
                return m_Apps;
            }
        }

        public override RazorTemplate[] RazorTemplates
        {
            get
            {
                return new RazorTemplate[]
                {
                    new RazorTemplate("SimpleFileConfiguratorSettingsView", Resources.settings),
                    new RazorTemplate("RecipeFileEditor", Resources.Editor)
                };
            }
        }

        public override void RecipeLoad()
        {
            Core.Instance.Load();
        }

        public override void RecipeSave(Extension.Core.Exchange.Recipe theRecipe)
        {
            //Extension.Core.Exchange.Recipe LoadedFromFile = null;

            //try
            //{
            //    LoadedFromFile = LoadFile();
            //}
            //catch
            //{
            //    // File not found ?
            //}

            //var configList = theRecipe.Extensions.ToList();

            //if (LoadedFromFile != null)
            //{
            //    LoadedFromFile.Extensions.ToList().ForEach(e =>
            //    {
            //        if (configList.Find(ce => ce.Key == e.Key) == null)
            //        {
            //            configList.Add(e);
            //        }
            //    });
            //}

            //var json = KeyValuesJson.Stringify( configList.ToArray() );

            //var json = theRecipe.Json;

            //using (Stream stream = new FileStream(m_Settings.FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    using (StreamWriter writer = new StreamWriter(stream))
            //    {
            //        writer.Write(json);
            //    }
            //}

            Core.Instance.Save(theRecipe);
        }

        public override object Save()
        {
            //var result = new List<Payload>(); 

            //Type type = m_Settings.GetType();

            //List<Pair> Pairs = new List<Pair>();

            //foreach (var Prop in type.GetProperties())
            //{
            //    Pairs.Add(new Pair( Prop.Name, Prop.GetValue(m_Settings, null).ToString() ) );
            //}

            //result.Add(new Payload( string.Empty, Pairs.ToArray() ));

            //return result;

            return Core.Instance;
        }

    }
}
