using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace HiveMind.Core.Utilities.Editor
{
    public sealed class UtilitiesEditor : OdinEditorWindow
    {
        #region Constants
        private const string MenuItemName = "HiveMind/Editors/UtilitiesEditor";
        private const float MinWidth = 512f;
        private const float MinHeight = 512f;
        private const float MaxWidth = 1024f;
        private const float MaxHeight = 1024f;
        private const string CSharpGeneratorTitle = "C# Generator";
        #endregion

        #region CSharpGenerator
        [TitleGroup(CSharpGeneratorTitle, null, TitleAlignments.Centered, true, true)]
        [SerializeField] private string className;
        #endregion

        #region Core
        [MenuItem(MenuItemName)]
        private static void OpenWindow()
        {
            UtilitiesEditor window = GetWindow<UtilitiesEditor>();
            window.minSize = new(MinWidth, MinHeight);
            window.maxSize = new(MaxWidth, MaxHeight);
            window.Show();
        }
        #endregion
    }
}
