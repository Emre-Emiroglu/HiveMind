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
        private const string CSharpGeneratorFoldoutGroup = "CSharp Generator";
        private const string CSharpGeneratorTitle = "Generate CSharp Class";
        #endregion

        #region CSharpGenerator
        [FoldoutGroup(CSharpGeneratorFoldoutGroup, false)]
        [Title(CSharpGeneratorTitle, null, TitleAlignments.Centered)]
        [FoldoutGroup(CSharpGeneratorFoldoutGroup, false)]
        [HideLabel] [SerializeField] private CSharpGenerator cSharpGenerator;
        [FoldoutGroup(CSharpGeneratorFoldoutGroup, false)]
        [Button(ButtonSizes.Medium, ButtonStyle.CompactBox)] private void GenerateScript() =>  cSharpGenerator.GenerateScript();
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
