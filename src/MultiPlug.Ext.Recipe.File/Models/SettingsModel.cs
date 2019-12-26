using MultiPlug.Base;
using System.Collections.Generic;

namespace MultiPlug.Ext.Recipe.File.Models
{
    public class SettingsModel : MultiPlugBase
    {
        public List<string> ExtensionNames { get; internal set; }
        public string FilePath { get; set; }
        public string LastRead { get; internal set; }
    }
}
