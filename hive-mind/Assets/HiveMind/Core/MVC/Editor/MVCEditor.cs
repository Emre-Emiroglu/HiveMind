using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.MVC.Editor
{
    public sealed class MVCEditor : OdinEditorWindow
    {
        #region Constants
        private const string menuItemName = "HiveMind/Editors/MVCEditor";
        private const float minWidth = 512f;
        private const float minHeight = 512f;
        private const float maxWidth = 1024f;
        private const float maxHeight = 1024f;
        #endregion

        #region Core
        [MenuItem(menuItemName)]
        private static void OpenWindow()
        {
            MVCEditor window = GetWindow<MVCEditor>();
            window.minSize = new(minWidth, minHeight);
            window.maxSize = new(maxWidth, maxHeight);
            window.Show();
        }
        #endregion
    }
}
