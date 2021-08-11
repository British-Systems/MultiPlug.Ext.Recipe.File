using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MultiPlug.Extension.Core.Exchange;
using MultiPlug.Ext.Recipe.File.Models;

namespace MultiPlug.Ext.Recipe.File
{
    internal class Core
    {
        private static Core m_Instance = null;

        internal void SetSave(string theExtension, bool shouldSave)
        {
            ExtensionItem Search = ExtensionItems.FirstOrDefault(Extension => Extension.Name == theExtension);
            if(Search != null)
            {
                Search.Save = shouldSave;
            }
        }

        public event EventHandler<Extension.Core.Exchange.Recipe> Loaded;

        internal ExtensionItem[] ExtensionItems { get; private set; } = new ExtensionItem[0];
        internal string FilePath { get; private set; }
        internal string LastRead { get; private set; }
        internal string LastWrite{ get; private set; }

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

        internal string ReadFile()
        {
            return System.IO.File.ReadAllText(FilePath);
        }


        private Extension.Core.Exchange.Recipe LoadFile()
        {
            Extension.Core.Exchange.Recipe Result = new Extension.Core.Exchange.Recipe { Json = ReadFile() };
            return Result;
        }

        internal void Replace(string theJson)
        {
            try
            {
                RecipeItem item = new RecipeItem(theJson);

                RecipeCollection Collection;

                if( item.Assembly == null)
                {
                    Collection = Extension.Core.Exchange.Recipe.ToObject(theJson);
                }
                else
                {
                    Collection = new RecipeCollection { Extensions = new RecipeItem[] { item }};
                }
                        
                var Recipe = new Extension.Core.Exchange.Recipe(Collection);
                Save(Recipe);

                ExtensionItems = Merge(ExtensionItems, Collection.Extensions.Select(e => new ExtensionItem { Name = e.Assembly, Save = true }).ToArray());
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

            string Result = string.Empty;

            if(Search != null)
            {
                Result = Search.ToJson(true);
            }

            return Result;
        }

        internal void LoadSingle(string theAssembly)
        {
            Extension.Core.Exchange.Recipe config = LoadFile();
            RecipeCollection Object = Extension.Core.Exchange.Recipe.ToObject(config.Json);

            var Search = Object.Extensions.FirstOrDefault(e => e.Assembly.Equals(theAssembly, StringComparison.OrdinalIgnoreCase));

            if( Search != null)
            {
                Loaded?.Invoke(this, new Extension.Core.Exchange.Recipe(new RecipeCollection(new[] { Search })));             
            }
        }

        internal void Load()
        {
            Extension.Core.Exchange.Recipe Recipe = null;
            DateTime DateTimeNow = DateTime.Now;

            RecipeCollection Object = null;

            try
            {
                Recipe = LoadFile();
                Object = Extension.Core.Exchange.Recipe.ToObject(Recipe.Json);
                LastRead = DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");
            }
            catch
            {
                Recipe = new Extension.Core.Exchange.Recipe { Json = string.Empty };
                LastRead = "Read Error! at " + DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");
            }


            if( Object != null)
            {
                ExtensionItems = Merge(ExtensionItems, Object.Extensions.Select(e => new ExtensionItem { Name = e.Assembly, Save = true }).ToArray());
            }

            Loaded?.Invoke(this, Recipe);
        }

        private ExtensionItem[] Merge(ExtensionItem[] theExisting, ExtensionItem[] theNew)
        {
            List<ExtensionItem> New = new List<ExtensionItem>(theNew);

            foreach (ExtensionItem Item in theExisting)
            {
                ExtensionItem Search = New.FirstOrDefault(i => i.Name == Item.Name);

                if (Search == null)
                {
                    New.Add(Item);
                }
            }

            return New.ToArray();
        }

        private RecipeCollection Merge(RecipeCollection theExisting, RecipeCollection theNew)
        {
            if (theNew.Extensions.Any())
            {
                List<RecipeItem> New = new List<RecipeItem>(theNew.Extensions);

                foreach (RecipeItem Item in theExisting.Extensions)
                {
                    RecipeItem Search = New.FirstOrDefault(i => i.Assembly == Item.Assembly);

                    if (Search == null)
                    {
                        New.Add(Item);
                    }
                }

                theExisting.Extensions = New.ToArray();
            }

            return theExisting;
        }


        internal void Save(Extension.Core.Exchange.Recipe theRecipe)
        {
            RecipeCollection ExistingRecipeCollection = null;

            try
            {
                Extension.Core.Exchange.Recipe ConfigFile = LoadFile();
                ExistingRecipeCollection = Extension.Core.Exchange.Recipe.ToObject(ConfigFile.Json);
            }
            catch
            {
            }


            RecipeCollection NewRecipeCollection = Extension.Core.Exchange.Recipe.ToObject(theRecipe.Json);

            if(ExistingRecipeCollection != null)
            {
                NewRecipeCollection = Merge(ExistingRecipeCollection, NewRecipeCollection);
            }


            NewRecipeCollection = RemoveNotToBeSaved(NewRecipeCollection, ExtensionItems);

            var Recipe = new Extension.Core.Exchange.Recipe();

            Recipe.ToJson(NewRecipeCollection);

            using (Stream FileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter StreamWriter = new StreamWriter(FileStream))
                {
                    StreamWriter.Write(Recipe.Json);
                }
            }

            DateTime DateTimeNow = DateTime.Now;
            LastWrite = DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");
        }

        private RecipeCollection RemoveNotToBeSaved(RecipeCollection newRecipeCollection, ExtensionItem[] extensionItems)
        {
            var UnsaveExtensions = extensionItems.Where(item => item.Save == false);
            return new RecipeCollection( newRecipeCollection.Extensions.Where(item => UnsaveExtensions.FirstOrDefault( Extension => Extension.Name == item.Assembly) == null ).ToArray());
        }
    }
}
