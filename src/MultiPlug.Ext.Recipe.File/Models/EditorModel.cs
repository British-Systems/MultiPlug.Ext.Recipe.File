
namespace MultiPlug.Ext.Recipe.File.Models
{
    public class EditorModel
    {
        public string Json { get; set; }

        public string[] Extensions { get; set; }

        public string SelectedExtension { get; set; }
        public string[] SnapShots { get; set; }
        public string SelectedFile { get; set; }
        public bool RebootUserPrompt { get; internal set; }
    }
}
