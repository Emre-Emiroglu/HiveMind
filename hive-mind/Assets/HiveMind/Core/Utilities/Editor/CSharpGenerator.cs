using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HiveMind.Core.Utilities.Editor
{
    [Serializable]
    public sealed class CSharpGenerator
    {
        #region Constants
        private const string DefaultClassName = "SampleClass";
        private const string DefaultInheritedName = "MonoBehaviour";
        private const string DefaultNamespaceName = "RootNamespace";
        private const string DefaultFolderPathName = "Assets";
        #endregion

        #region Statics
        private static readonly List<string> DefaultUsings = new List<string>()
        {
            "UnityEngine",
        };
        private static readonly List<string> DefaultRegions = new List<string>()
        {
            "Constants",
            "Statics",
            "Injects",
            "ReadonlyFields",
            "Fields",
            "Getters",
            "Props",
            "Constructor",
            "Core",
            "Executes"
        };
        #endregion
        
        #region Fields
        [Header("CSharp Generator Fields")]
        [SerializeField] private string className = DefaultClassName;
        [SerializeField] private string inheritedName = DefaultInheritedName;
        [SerializeField] private string namespaceName = DefaultNamespaceName;
        [SerializeField] private List<string> usings = DefaultUsings;
        [SerializeField] private List<string> regions = DefaultRegions;
        [FolderPath] [SerializeField] private string folderPath = DefaultFolderPathName;
        internal string FinalCode;
        #endregion

        #region Executes
        public void GenerateScript()
        {
            FinalCode = string.Empty;
            
            GenerateUsings();
            GenerateNamespace();
            GenerateClass();
            GenerateRegions();
            CloseCode();
            SaveFile();
        }
        private void GenerateUsings()
        {
            foreach (string t in usings)
                FinalCode += $"using {t};\n";
        }
        private void GenerateNamespace()
        {
            if (!string.IsNullOrEmpty(namespaceName))
            {
                FinalCode += $"\nnamespace {namespaceName}\n";
                FinalCode += "{\n";
            }
        }
        private void GenerateClass()
        {
            FinalCode += $"\tpublic class {className} : {inheritedName}\n";
            FinalCode += "\t{\n";
        }
        private void GenerateRegions()
        {
            foreach (string region in regions)
            {
                FinalCode += $"\t\t#region {region}\n";
                FinalCode += "\t\t#endregion\n\n";
            }
        }
        private void CloseCode()
        {
            FinalCode += "\t}\n";

            if (!string.IsNullOrEmpty(namespaceName))
                FinalCode += "}\n";
        }
        private void SaveFile()
        {
            string textSaver = Path.Combine(folderPath, className + ".cs");
            if (File.Exists(textSaver))
                File.Delete(textSaver);

            File.WriteAllText(textSaver, FinalCode);
        }
        #endregion
    }
}