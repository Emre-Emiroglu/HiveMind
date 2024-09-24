using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace HiveMind.Core.MVC.Editor
{
    public sealed class MvcEditor : OdinEditorWindow
    {
        #region Constants
        private const string MenuItemName = "HiveMind/Editors/MVCEditor";
        private const float MinWidth = 512f;
        private const float MinHeight = 512f;
        private const float MaxWidth = 1024f;
        private const float MaxHeight = 1024f;
        #endregion

        #region Core
        [MenuItem(MenuItemName)]
        private static void OpenWindow()
        {
            MvcEditor window = GetWindow<MvcEditor>();
            window.minSize = new(MinWidth, MinHeight);
            window.maxSize = new(MaxWidth, MaxHeight);
            window.Show();
        }
        #endregion
    }
}
