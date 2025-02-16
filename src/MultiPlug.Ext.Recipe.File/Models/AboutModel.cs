
namespace MultiPlug.Ext.Recipe.File.Models
{
    public class AboutModel
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string Trademark { get; set; }
        public string Version { get; set; }
        public string RecipeFileLocation { get; set; }
        public bool RebootUserPrompt { get; internal set; }
    }
}
