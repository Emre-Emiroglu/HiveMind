using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.Utilities.Editor
{
    public sealed class UtilitiesEditor : OdinEditorWindow
    {
        #region Constants
        private const string MENU_ITEM_NAME = "HiveMind/Editors/UtilitiesEditor";
        private const float MIN_WIDTH = 512f;
        private const float MIN_HEIGHT = 512f;
        private const float MAX_WIDTH = 1024f;
        private const float MAX_HEIGHT = 1024f;
        #endregion

        #region Core
        [MenuItem(MENU_ITEM_NAME)]
        private static void OpenWindow()
        {
            UtilitiesEditor window = GetWindow<UtilitiesEditor>();
            window.minSize = new(MIN_WIDTH, MIN_HEIGHT);
            window.maxSize = new(MAX_WIDTH, MAX_HEIGHT);
            window.Show();
        }
        #endregion
    }
}
