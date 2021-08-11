﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlug.Ext.Recipe.File.Models
{
    public class EditorModel
    {
        public string Json { get; set; }

        public ExtensionItem[] Extensions { get; set; }

        public string Selected { get; set; }
    }
}
