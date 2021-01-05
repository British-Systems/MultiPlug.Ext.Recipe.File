using MultiPlug.Extension.Core;
using MultiPlug.Ext.Recipe.File.Properties;
using MultiPlug.Extension.Core.Exchange;
using MultiPlug.Extension.Core.Http;

namespace MultiPlug.Ext.Recipe.File
{
    public class RecipeFile : MultiPlugExtension, IRecipe
    {


        public RecipeFile()
        {
            Core.Instance.Loaded += OnRecipeLoaded;
        }

        private void OnRecipeLoaded(object sender, Extension.Core.Exchange.Recipe e)
        {
            MultiPlugActions.Extension.Updates.Recipe(e);
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
            Core.Instance.Save(theRecipe);
        }

        public override object Save()
        {
            return Core.Instance;
        }

    }
}
