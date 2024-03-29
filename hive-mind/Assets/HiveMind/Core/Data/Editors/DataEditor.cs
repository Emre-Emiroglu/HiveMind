using System.IO;
using UnityEditor;
using UnityEngine;

namespace HiveMind.Core.Data.Editors
{
    public class DataEditor : EditorWindow, IHasCustomMenu
    {
        #region Constants
        private const int windowWidth = 512;
        private const int windowHeight = 256;
        private const string baseKey = "HiveMind.Data.Editors.DataEditor";
        private const string autoRepaintKey = baseKey + ".autoRepaint";
        #endregion

        #region Fields
        private bool autoRepaint = true;
        #endregion

        #region Core
        [MenuItem("HiveMind/Editors/DataEditor")]
        public static void ShowDataEditor()
        {
            DataEditor dataEditor = GetWindow<DataEditor>();

            dataEditor.minSize = new Vector2(windowWidth, windowHeight);
            dataEditor.maxSize = new Vector2(windowWidth, windowHeight);

            dataEditor.titleContent = new GUIContent("Data Editor");
        }
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Auto repaint"), autoRepaint, () => EditorPrefs.SetBool(autoRepaintKey, autoRepaint = !autoRepaint));
        }
        private void OnEnable()
        {
            Rect windowRect = position;
            windowRect.width = windowWidth;
            windowRect.height = windowHeight;
            position = windowRect;
        }
        #endregion

        #region Executes
        private void Notification(string message, string tooltip = "", double fadeoutWait = 1)
        {
            this.ShowNotification(new GUIContent(message, tooltip), fadeoutWait);
        }
        #endregion

        #region Visualize
        private void ToolBarButtons()
        {
            if (GUILayout.Button(new GUIContent("Open Data Folder", "Open data folder button for showing data files."), EditorStyles.toolbarButton))
            {
                EditorUtility.RevealInFinder(Application.persistentDataPath + "/");

                Notification("Data Folder Opened");
            }

            GUILayout.Space(16);

            if (GUILayout.Button(new GUIContent("Delete All Datas", "Delete all datas button for deleting data files."), EditorStyles.toolbarButton))
            {
                string path = Application.persistentDataPath + "/";
                bool isExists = Directory.Exists(path);

                if (isExists)
                {
                    string[] files = Directory.GetFiles(path);
                    if (files.Length == 0)
                        Notification($"Any data files not found in: {path}");
                    else
                    {
                        foreach (string file in files)
                            File.Delete(file);

                        Notification("All data files deleted");
                    }
                }
                else
                    Notification($"Directory not found in: {path}");
            }

            GUILayout.Space(16);

            if (GUILayout.Button(new GUIContent("Delete Player Prefs", "Delete player prefst button for deleting player prefs datas."), EditorStyles.toolbarButton))
            {
                PlayerPrefs.DeleteAll();

                Notification("All player prefses deleted");
            }
        }
        #endregion

        #region OnGUI
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.Space(8);

            ToolBarButtons();

            GUILayout.Space(8);

            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
