using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GUILayout;

namespace PrimeTween {
    internal class PrimeTweenInstaller : ScriptableObject {
        [SerializeField] internal SceneAsset demoScene;
        [SerializeField] internal SceneAsset demoSceneUrp;
        [SerializeField] internal Color uninstallButtonColor;
        
        [ContextMenu(nameof(ResetReviewRequest))]
        void ResetReviewRequest() => ReviewRequest.ResetReviewRequest();
    }
    
    [CustomEditor(typeof(PrimeTweenInstaller), false)]
    internal class InstallerInspector : Editor {
        const string pluginName = "PrimeTween";
        const string pluginPackageId = "com.kyrylokuzyk.primetween";
        const string tgzPath = "Assets/Plugins/" + pluginName + "/internal/" + pluginPackageId + ".tgz";
        const string documentationUrl = "https://github.com/KyryloKuzyk/PrimeTween";
        bool isInstalled;
        GUIStyle boldButtonStyle;
        GUIStyle uninstallButtonStyle;
        
        void OnEnable() {
            isInstalled = CheckPluginInstalled();
        }

        static bool CheckPluginInstalled() {
            var listRequest = Client.List(true);
            while (!listRequest.IsCompleted) {
            }
            return listRequest.Result.Any(_ => _.name == pluginPackageId);
        }

        public override void OnInspectorGUI() {
            if (boldButtonStyle == null) {
                boldButtonStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
            }
            var installer = (PrimeTweenInstaller)target;
            if (uninstallButtonStyle == null) {
                uninstallButtonStyle = new GUIStyle(GUI.skin.button) { normal = { textColor = installer.uninstallButtonColor } };
            }
            Space(8);
            Label(pluginName, EditorStyles.boldLabel);
            if (!isInstalled) {
                Space(8);
                if (Button("Install " + pluginName)) {
                    installPlugin();
                }
                return;
            }

            Space(8);
            if (Button("Documentation", boldButtonStyle)) {
                Application.OpenURL(documentationUrl);
            }
            
            Space(8);
            if (Button("Open Demo", boldButtonStyle)) {
                var rpAsset = GraphicsSettings.renderPipelineAsset;
                bool isUrp = rpAsset != null && rpAsset.GetType().Name.Contains("Universal");
                var demoScene = isUrp ? installer.demoSceneUrp : installer.demoScene;
                if (demoScene == null) {
                    Debug.LogError("Please re-import the plugin from Asset Store and import the 'Demo' folder.\n");
                    return;
                }
                var path = AssetDatabase.GetAssetPath(demoScene);
                EditorSceneManager.OpenScene(path);
            }
            #if UNITY_2019_4_OR_NEWER
            if (Button("Import Basic Examples")) {
                EditorUtility.DisplayDialog(pluginName, $"Please select the '{pluginName}' package in 'Package Manager', then press the 'Samples/Import' button at the bottom of the plugin's description.", "Ok");
                UnityEditor.PackageManager.UI.Window.Open(pluginPackageId);
            }
            #endif
            if (Button("Support")) {
                Application.OpenURL("https://github.com/KyryloKuzyk/PrimeTween#support");
            }

            Space(8);
            if (Button("Uninstall", uninstallButtonStyle)) {
                Client.Remove(pluginPackageId);
                isInstalled = false;
                var msg = $"Please remove the folder manually to uninstall {pluginName} completely: 'Assets/Plugins/{pluginName}'";
                EditorUtility.DisplayDialog(pluginName, msg, "Ok");
                Debug.Log(msg);
            }

            ReviewRequest.DrawInspector();
        }

        static void installPlugin() {
            ReviewRequest.OnBeforeInstall();
            var path = $"file:../{tgzPath}";
            var addRequest = Client.Add(path);
            while (!addRequest.IsCompleted) {
            }
            if (addRequest.Status == StatusCode.Success) {
                Debug.Log($"{pluginName} installed successfully.\n" +
                          $"Offline documentation is located at Packages/{pluginName}/Documentation.md.\n" +
                          $"Online documentation: {documentationUrl}\n");
            } else {
                Debug.LogError($"Please re-import the plugin from the Asset Store and check that the file exists: [{path}].\n\n{addRequest.Error?.message}\n");
            }
        }
        
        #if !PRIME_TWEEN_INSTALLED && UNITY_2019_1_OR_NEWER
        internal class AssetPostprocessor : UnityEditor.AssetPostprocessor {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
                foreach (var path in importedAssets) {
                    if (path == tgzPath) {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<PrimeTweenInstaller>("Assets/Plugins/PrimeTween/PrimeTweenInstaller.asset");
                        installPlugin();
                        return;
                    }
                }
            }
        }
        #endif
    }

    internal static class ReviewRequest {
        const string version = "1.1.13";
        const string canAskKey = "PrimeTween.canAskForReview";
        const string versionKey = "PrimeTween.version";

        [InitializeOnLoadMethod]
        static void TryAskForReview() {
            if (!PRIME_TWEEN_INSTALLED) {
                log("not installed");
                return;
            }
            if (!EditorPrefs.GetBool(canAskKey, true)) {
                log("can't ask");
                return;
            }
            if (savedVersion == version) {
                log($"same version {version}");
                return;
            }
            log($"updated from version {savedVersion} to {version}, ask for review");
            savedVersion = version;
            DisableReviewRequest();
            var response = EditorUtility.DisplayDialogComplex("Enjoying PrimeTween?",
                "Would you mind to leave an honest review on Asset store? Honest reviews make PrimeTween better and help other developers discover it.",
                "Sure, leave a review!",
                "Never ask again",
                "");
            if (response == 0) {
                OpenReviewsURL();
            }
        }

        static bool PRIME_TWEEN_INSTALLED {
            get {
                #if PRIME_TWEEN_INSTALLED
                return true;
                #else 
                return false;
                #endif
            }
        }

        internal static void OnBeforeInstall() {
            log($"OnBeforeInstall {version}");
            if (string.IsNullOrEmpty(savedVersion)) {
                savedVersion = version;
            }
        }

        static string savedVersion {
            get => EditorPrefs.GetString(versionKey);
            set => EditorPrefs.SetString(versionKey, value);
        }
        
        static void DisableReviewRequest() => EditorPrefs.SetBool(canAskKey, false);
        static void OpenReviewsURL() => Application.OpenURL("https://assetstore.unity.com/packages/slug/252960#reviews");

        internal static void DrawInspector() {
            Space(32);
            Label("Enjoying PrimeTween?", EditorStyles.boldLabel);
            Space(8);
            Label("Consider leaving an <b>honest review</b> and starring PrimeTween on GitHub!\n\n" +
                  "Honest reviews make PrimeTween better and help other developers discover it.",
                new GUIStyle(GUI.skin.label) { wordWrap = true, richText = true, margin = new RectOffset(4, 4, 4, 4) });
            Space(8);
            if (Button("Leave review!", GUI.skin.button)) {
                DisableReviewRequest();
                OpenReviewsURL();
            }
        }

        internal static void ResetReviewRequest() {
            Debug.Log(nameof(ResetReviewRequest));
            EditorPrefs.DeleteKey(versionKey);
            EditorPrefs.DeleteKey(canAskKey);
        }

        [PublicAPI]
        static void log(string msg) {
            // Debug.Log($"ReviewRequest: {msg}");
        }
    }
}