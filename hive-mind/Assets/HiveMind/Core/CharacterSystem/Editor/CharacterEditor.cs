using HiveMind.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace HiveMind.CharacterSystem.Editor
{
    public sealed class CharacterEditor : OdinEditorWindow
    {
        #region Constants
        private const float minWidth = 512f;
        private const float minHeight = 512f;
        private const float maxWidth = 1024f;
        private const float maxHeight = 1024f;
        #endregion

        #region CharacterSettingsCreator
        [BoxGroup("Character Settings Creator", true, true, 99)]

        #region Datas
        [TitleGroup("Character Settings Creator/Input Data")]
        [SerializeField] private InputData inputData;
        [TitleGroup("Character Settings Creator/Movement Data")]
        [SerializeField] private MovementData movementData;
        [TitleGroup("Character Settings Creator/Rotation Data")]
        [SerializeField] private RotationData rotationData;
        #endregion

        #region Essentials
        [TitleGroup("Character Settings Creator/Essentials")]
        [AssetSelector][SerializeField] private CharacterSettings tempCharacterSettings;
        [TitleGroup("Character Settings Creator/Essentials")]
        [FolderPath][SerializeField] private string saveFolderPath;
        #endregion

        #region Buttons
        [TitleGroup("Character Settings Creator/Create Buttons")]
        [ButtonGroup("Character Settings Creator/Create Buttons/Buttons")] public void CreateCharacterSettings() => CreateCharacterSettingsProcess();
        #endregion

        #endregion

        #region Core
        [MenuItem("HiveMind/Editors/CharacterEditor")]
        private static void OpenWindow()
        {
            CharacterEditor window = GetWindow<CharacterEditor>();
            window.minSize = new(minWidth, minHeight);
            window.maxSize = new(maxWidth, maxHeight);
            window.Show();
        }
        #endregion

        #region ButtonProcess
        private void CreateCharacterSettingsProcess()
        {

        }
        #endregion
    }
}
