using MultiPlug.Base;
using MultiPlug.Ext.Recipe.File.Models;
using System.Collections.Generic;

namespace MultiPlug.Ext.Recipe.File.Models
{
    public class SettingsModel : MultiPlugBase
    {
        public ExtensionItem[] Extensions { get; internal set; }
        public string FilePath { get; set; }
        public string LastRead { get; internal set; }
        public string LastWrite { get; internal set; }
        public bool RebootUserPrompt { get; internal set; }
    }
}
