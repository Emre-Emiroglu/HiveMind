using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.Pool.Editor
{
    public sealed class PoolEditor : OdinEditorWindow
    {
        #region Constants
        private const string menuItemName = "HiveMind/Editors/PoolEditor";
        private const float minWidth = 512f;
        private const float minHeight = 512f;
        private const float maxWidth = 1024f;
        private const float maxHeight = 1024f;
        #endregion

        #region Core
        [MenuItem(menuItemName)]
        private static void OpenWindow()
        {
            PoolEditor window = GetWindow<PoolEditor>();
            window.minSize = new(minWidth, minHeight);
            window.maxSize = new(maxWidth, maxHeight);
            window.Show();
        }
        #endregion
    }
}
