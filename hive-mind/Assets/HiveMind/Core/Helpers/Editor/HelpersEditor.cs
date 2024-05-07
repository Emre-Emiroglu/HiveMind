using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.Helpers.Editor
{
    public sealed class HelpersEditor : OdinEditorWindow
    {
        #region Constants
        private const string menuItemName = "HiveMind/Editors/HelpersEditor";
        private const float minWidth = 512f;
        private const float minHeight = 512f;
        private const float maxWidth = 1024f;
        private const float maxHeight = 1024f;
        #endregion

        #region Core
        [MenuItem(menuItemName)]
        private static void OpenWindow()
        {
            HelpersEditor window = GetWindow<HelpersEditor>();
            window.minSize = new(minWidth, minHeight);
            window.maxSize = new(maxWidth, maxHeight);
            window.Show();
        }
        #endregion
    }
}
