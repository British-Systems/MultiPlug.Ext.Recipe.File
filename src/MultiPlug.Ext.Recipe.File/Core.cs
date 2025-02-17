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

        public event EventHandler<Extension.Core.Exchange.Recipe> Loaded;

        internal ExtensionItem[] ExtensionItems { get; private set; } = new ExtensionItem[0];
        internal string FilePath { get; private set; }
        internal string LastRead { get; private set; }
        internal string LastWrite{ get; private set; }
        private string m_SnapShotsPath;

        internal const string c_MainFile = "Main";

        internal bool RebootUserPrompt { get; set; }

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
            m_SnapShotsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snapshots");
        }

        internal void SetSave(string theExtension, bool shouldSave)
        {
            ExtensionItem Search = ExtensionItems.FirstOrDefault(Extension => Extension.Name == theExtension);
            if (Search != null)
            {
                Search.Save = shouldSave;
            }
        }

        internal void SetOverwrite(string theExtension, bool shouldOverwrite)
        {
            ExtensionItem Search = ExtensionItems.FirstOrDefault(Extension => Extension.Name == theExtension);
            if (Search != null)
            {
                Search.Overwrite = shouldOverwrite;
            }
        }

        internal void SetOverwriteAll(bool shouldOverwrite)
        {
            foreach ( var Extension in ExtensionItems)
            {
                Extension.Overwrite = shouldOverwrite;
            }
        }

        internal string ReadFile(string theFileName)
        {
            try
            {
                if(theFileName == c_MainFile)
                {
                    return System.IO.File.ReadAllText(FilePath);
                }
                else
                {
                    return System.IO.File.ReadAllText(Path.Combine(m_SnapShotsPath, theFileName + ".json"));
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        internal RecipeCollection Replace(string theFileName, string theJson)
        {
            RecipeCollection Collection = null;

            try
            {
                RecipeItem item = new RecipeItem(theJson);

                if( item.Assembly == null)
                {
                    Collection = Extension.Core.Exchange.Recipe.ToObject(theJson);
                }
                else
                {
                    Collection = new RecipeCollection { Extensions = new RecipeItem[] { item }};
                }
                        
                var Recipe = new Extension.Core.Exchange.Recipe(Collection);
                Save(theFileName, Recipe, true);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
            }

            return Collection;
        }

        internal string Load(string theFileName, string theAssembly )
        {
            string config = ReadFile(theFileName);
            RecipeCollection Object = Extension.Core.Exchange.Recipe.ToObject(config);

            return GetJson(Object, theAssembly);
        }

        internal string GetJson(RecipeCollection theRecipeCollection, string theAssembly)
        {
            string Result = string.Empty;

            if (theRecipeCollection != null)
            {
                var Search = theRecipeCollection.Extensions.FirstOrDefault(e => e.Assembly.Equals(theAssembly, StringComparison.OrdinalIgnoreCase));
                if (Search != null)
                {
                    Result = Search.ToJson(true);
                }
            }
            return Result;
        }

        internal void LoadSingle(string theAssembly)
        {
            string config = ReadFile(c_MainFile);
            RecipeCollection Object = Extension.Core.Exchange.Recipe.ToObject(config);

            var Search = Object.Extensions.FirstOrDefault(e => e.Assembly.Equals(theAssembly, StringComparison.OrdinalIgnoreCase));

            if( Search != null)
            {
                Loaded?.Invoke(this, new Extension.Core.Exchange.Recipe(new RecipeCollection(new[] { Search })));             
            }
        }

        internal void PushRecipe()
        {
            Loaded?.Invoke(this, LoadItems());
        }

        private Extension.Core.Exchange.Recipe LoadItems()
        {
            Extension.Core.Exchange.Recipe Recipe = null;
            DateTime DateTimeNow = DateTime.Now;

            RecipeCollection Object = null;

            try
            {
                Recipe = new Extension.Core.Exchange.Recipe { Json = ReadFile(c_MainFile) };
                Object = Extension.Core.Exchange.Recipe.ToObject(Recipe.Json);
                LastRead = DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");
            }
            catch
            {
                Recipe = new Extension.Core.Exchange.Recipe { Json = string.Empty };
                LastRead = "Read Error! at " + DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");
            }


            if (Object != null)
            {
                ExtensionItems = NewInTo(ExtensionItems, Object.Extensions.Select(e => new ExtensionItem { Name = e.Assembly, Save = true, Overwrite = true }).ToArray());
            }

            return Recipe;
        }

        private ExtensionItem[] NewInTo(ExtensionItem[] theExisting, ExtensionItem[] theNew)
        {
            List<ExtensionItem> Existing = new List<ExtensionItem>(theExisting);

            foreach (ExtensionItem Item in theNew)
            {
                ExtensionItem Search = Existing.FirstOrDefault(i => i.Name == Item.Name);

                if (Search == null)
                {
                    Existing.Add(Item);
                }
            }

            return Existing.ToArray();
        }

        private RecipeCollection MergeNewInTo(RecipeCollection theExisting, RecipeCollection theNew)
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


        internal void Save(string theFileName, Extension.Core.Exchange.Recipe theRecipe, bool shouldOverwriteAndSave)
        {
            RecipeCollection ExistingRecipeCollection = null;

            try
            {
                string ConfigFile = ReadFile(theFileName);
                ExistingRecipeCollection = Extension.Core.Exchange.Recipe.ToObject(ConfigFile);
            }
            catch
            {
            }

            RecipeCollection NewRecipeCollection = Extension.Core.Exchange.Recipe.ToObject(theRecipe.Json);

            if (shouldOverwriteAndSave == false && theFileName == c_MainFile)
            {
                NewRecipeCollection = RemoveNoOverwriteItems(NewRecipeCollection, ExtensionItems);
            }

            if (ExistingRecipeCollection != null)
            {
                NewRecipeCollection = MergeNewInTo(ExistingRecipeCollection, NewRecipeCollection);
            }

            if(shouldOverwriteAndSave == false && theFileName == c_MainFile)
            {
                NewRecipeCollection = RemoveNotToBeSaved(NewRecipeCollection, ExtensionItems);
            }

            if (theFileName == c_MainFile)
            {
                ExtensionItems = NewInTo(ExtensionItems, NewRecipeCollection.Extensions.Select(e => new ExtensionItem { Name = e.Assembly, Save = true, Overwrite = true }).ToArray());
            }

            var Recipe = new Extension.Core.Exchange.Recipe();

            Recipe.ToJson(NewRecipeCollection);

            string SavePath;

            if (theFileName == c_MainFile)
            {
                SavePath = FilePath;
            }
            else
            {
                SavePath = Path.Combine(m_SnapShotsPath, theFileName + ".json");
            }

            using (Stream FileStream = new FileStream(SavePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter StreamWriter = new StreamWriter(FileStream))
                {
                    StreamWriter.Write(Recipe.Json);
                }
            }

            if(theFileName == c_MainFile)
            {
                DateTime DateTimeNow = DateTime.Now;
                LastWrite = DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");
            }
        }

        private static RecipeCollection RemoveNoOverwriteItems(RecipeCollection theNewRecipeCollection, ExtensionItem[] theExtensionItems )
        {
            var NoOverwriteExtensions = theExtensionItems.Where(item => item.Overwrite == false);
            return new RecipeCollection(theNewRecipeCollection.Extensions.Where(item => NoOverwriteExtensions.FirstOrDefault(Extension => Extension.Name == item.Assembly) == null).ToArray());
        }

        private static RecipeCollection RemoveNotToBeSaved(RecipeCollection theNewRecipeCollection, ExtensionItem[] theExtensionItems)
        {
            var UnsaveExtensions = theExtensionItems.Where(item => item.Save == false);
            return new RecipeCollection( theNewRecipeCollection.Extensions.Where(item => UnsaveExtensions.FirstOrDefault( Extension => Extension.Name == item.Assembly) == null ).ToArray());
        }

        internal string[] GetSnapShots()
        {
            if(Directory.Exists(m_SnapShotsPath))
            {
                return Directory.GetFiles(m_SnapShotsPath, "*.json").Select( file => Path.GetFileNameWithoutExtension(file)).ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        internal void CreateSnapShot( string theFileName )
        {
            if(string.IsNullOrEmpty(theFileName))
            {
                return;
            }

            if (!Directory.Exists(m_SnapShotsPath))
            {
                Directory.CreateDirectory(m_SnapShotsPath);
            }

            theFileName = string.Join("-", theFileName.Split(Path.GetInvalidFileNameChars()));
            theFileName = theFileName.Replace(" ", "-");

            var NewSnapShotsPath = Path.Combine(m_SnapShotsPath, theFileName + ".json");

            if(System.IO.File.Exists(NewSnapShotsPath) == false)
            {
                System.IO.File.Copy(FilePath, Path.Combine(m_SnapShotsPath, theFileName + ".json"));
            }
        }

        internal bool DeleteSnapShot(string theFileName )
        {
            try
            {
                System.IO.File.Delete(Path.Combine(m_SnapShotsPath, theFileName + ".json"));
                return true;
            }
            catch
            {
                return false;
            }

        }

        internal bool CopyToMainRecipe(string theFileName)
        {
            bool Result = false;

            if (!string.IsNullOrEmpty(theFileName))
            {
                var SnapShotPath = Path.Combine(m_SnapShotsPath, theFileName + ".json");

                if (System.IO.File.Exists(SnapShotPath))
                {
                    try
                    {
                        System.IO.File.Copy(SnapShotPath, FilePath, true);
                        DateTime DateTimeNow = DateTime.Now;
                        LastWrite = DateTimeNow.ToString("d/M/yyyy") + " at " + DateTimeNow.ToString("h:mm:ss");

                        LoadItems();

                        foreach (var item in ExtensionItems)
                        {
                            item.Overwrite = false;
                        }
                        RebootUserPrompt = true;
                        Result = true;
                    }
                    catch
                    {
                    }
                }
            }

            return Result;
        }
    }
}
