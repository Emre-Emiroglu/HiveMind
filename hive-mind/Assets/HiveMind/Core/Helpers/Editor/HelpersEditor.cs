using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.Helpers.Editor
{
    public sealed class HelpersEditor : OdinEditorWindow
    {
        #region Constants
        private const string MenuItemName = "HiveMind/Editors/HelpersEditor";
        private const float MinWidth = 512f;
        private const float MinHeight = 512f;
        private const float MaxWidth = 1024f;
        private const float MaxHeight = 1024f;
        #endregion

        #region Core
        [MenuItem(MenuItemName)]
        private static void OpenWindow()
        {
            HelpersEditor window = GetWindow<HelpersEditor>();
            window.minSize = new(MinWidth, MinHeight);
            window.maxSize = new(MaxWidth, MaxHeight);
            window.Show();
        }
        #endregion
    }
}
