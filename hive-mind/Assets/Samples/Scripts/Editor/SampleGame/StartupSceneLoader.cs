using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace CodeCatGames.HiveMind.Samples.Editor.SampleGame
{
    [InitializeOnLoad]
    public class StartupSceneLoader
    {
        #region Constants
        private const string PreviousSceneKey = "PreviousScene";
        private const string ShouldLoadStartupSceneKey = "LoadStartupScene";
        private const string LoadStartupSceneOnPlay = "HiveMind/Samples/SampleGame/Load Startup Scene On Play";
        private const string DontLoadStartupSceneOnPlay = "HiveMind/Samples/SampleGame/Don't Load Startup Scene On Play";
        #endregion

        #region Fields
        private static bool _restartingToSwitchedScene;
        #endregion

        #region Getters
        private static string StartupScene => EditorBuildSettings.scenes[0].path;
        #endregion

        #region Props
        private static string PreviousScene
        {
            get => EditorPrefs.GetString(PreviousSceneKey);
            set => EditorPrefs.SetString(PreviousSceneKey, value);
        }
        private static bool ShouldLoadStartupScene
        {
            get
            {
                if (!EditorPrefs.HasKey(ShouldLoadStartupSceneKey))
                    EditorPrefs.SetBool(ShouldLoadStartupSceneKey, true);

                return EditorPrefs.GetBool(ShouldLoadStartupSceneKey);
            }

            set => EditorPrefs.SetBool(ShouldLoadStartupSceneKey, value);
        }
        #endregion

        #region Constructor
        static StartupSceneLoader() =>
            EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
        #endregion

        #region MenuItems
        [MenuItem(LoadStartupSceneOnPlay, true)]
        private static bool ShowLoadStartupSceneOnPlay() => !ShouldLoadStartupScene;
        [MenuItem(LoadStartupSceneOnPlay)]
        private static void EnableLoadStartupSceneOnPlay() => ShouldLoadStartupScene = true;
        [MenuItem(DontLoadStartupSceneOnPlay, true)]
        private static bool ShowDoNotLoadStartupSceneOnPlay() => ShouldLoadStartupScene;
        [MenuItem(DontLoadStartupSceneOnPlay)]
        private static void DisableDoNotLoadBootstrapSceneOnPlay() => ShouldLoadStartupScene = false;
        #endregion

        #region Executes
        private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (!ShouldLoadStartupScene)
                return;

            if (_restartingToSwitchedScene) //error check as multiple starts and stops happening
            {
                if (playModeStateChange == PlayModeStateChange.EnteredPlayMode)
                    _restartingToSwitchedScene = false;
                
                return;
            }

            if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
            {
                // cache previous scene to return to it after play session ends
                PreviousScene = SceneManager.GetActiveScene().path;

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    // user either hit "Save" or "Don't Save"; open bootstrap scene

                    if (!string.IsNullOrEmpty(StartupScene) && System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == StartupScene))
                    {
                        Scene activeScene = SceneManager.GetActiveScene();

                        _restartingToSwitchedScene = activeScene.path == string.Empty || !StartupScene.Contains(activeScene.path);

                        // only switch if editor is in an empty scene or active scene is not startup scene
                        if (_restartingToSwitchedScene)
                        {
                            EditorApplication.isPlaying = false;

                            // scene is included in build settings; open it
                            EditorSceneManager.OpenScene(StartupScene);

                            EditorApplication.isPlaying = true;
                        }
                    }
                }
                else
                {
                    // user either hit "Cancel" or exited window; don't open startup scene & return to editor
                    EditorApplication.isPlaying = false;
                }
            }
            //return to last open scene
            else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
            {
                if (!string.IsNullOrEmpty(PreviousScene))
                    EditorSceneManager.OpenScene(PreviousScene);
            }
        }
        #endregion
    }
}
