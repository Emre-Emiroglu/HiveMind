using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.Helpers.Editor
{
    public sealed class HelpersEditor : OdinEditorWindow
    {
        #region Constants
        private const string MENU_ITEM_NAME = "HiveMind/Editors/HelpersEditor";
        private const float MIN_WIDTH = 512f;
        private const float MIN_HEIGHT = 512f;
        private const float MAX_WIDTH = 1024f;
        private const float MAX_HEIGHT = 1024f;
        #endregion

        #region Core
        [MenuItem(MENU_ITEM_NAME)]
        private static void OpenWindow()
        {
            HelpersEditor window = GetWindow<HelpersEditor>();
            window.minSize = new(MIN_WIDTH, MIN_HEIGHT);
            window.maxSize = new(MAX_WIDTH, MAX_HEIGHT);
            window.Show();
        }
        #endregion
    }
}
