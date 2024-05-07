using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.Utilities.Editor
{
    public sealed class UtilitiesEditor : OdinEditorWindow
    {
        #region Constants
        private const string menuItemName = "HiveMind/Editors/UtilitiesEditor";
        private const float minWidth = 512f;
        private const float minHeight = 512f;
        private const float maxWidth = 1024f;
        private const float maxHeight = 1024f;
        #endregion

        #region Core
        [MenuItem(menuItemName)]
        private static void OpenWindow()
        {
            UtilitiesEditor window = GetWindow<UtilitiesEditor>();
            window.minSize = new(minWidth, minHeight);
            window.maxSize = new(maxWidth, maxHeight);
            window.Show();
        }
        #endregion
    }
}
