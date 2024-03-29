using HiveMind.Core.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Editor
{
    public sealed class CharacterEditor : OdinEditorWindow
    {
        #region Constants
        private const string characterSettingsCreatorTitlePreffix = "Character Settings Creator";
        private const string generalsSuffix = "/Generals";
        private const string datasSuffix = "/Datas";
        private const string buttonsSuffix = "/Buttons";

        private const string menuItemName = "HiveMind/Editors/CharacterEditor";
        private const float minWidth = 512f;
        private const float minHeight = 512f;
        private const float maxWidth = 1024f;
        private const float maxHeight = 1024f;

        private const string defaultCharacterSettingsName = "CharacterSettings";
        private const string saveFolderPathError = "Save folder path can not be empty!";
        private const string assetSuffix = ".asset";
        private const string characterSettingsCreatedLog = "Character Settings created at: ";
        #endregion

        #region CharacterSettingsCreator
        [TitleGroup(characterSettingsCreatorTitlePreffix, "", TitleAlignments.Centered, true, true, false)]

        #region Generals
        [TitleGroup(characterSettingsCreatorTitlePreffix + generalsSuffix, "", TitleAlignments.Left, false, true, true)]
        [SerializeField] private string characterSettingsName;
        [TitleGroup(characterSettingsCreatorTitlePreffix + generalsSuffix, "", TitleAlignments.Left, false, true, true)]
        [FolderPath][SerializeField] private string saveFolderPath;
        #endregion

        #region Datas
        [TitleGroup(characterSettingsCreatorTitlePreffix + datasSuffix, "", TitleAlignments.Left, false, true, true)]
        [SerializeField] private InputData inputData;
        [TitleGroup(characterSettingsCreatorTitlePreffix + datasSuffix, "", TitleAlignments.Left, false, true, true)]
        [SerializeField] private MovementData movementData;
        [TitleGroup(characterSettingsCreatorTitlePreffix + datasSuffix, "", TitleAlignments.Left, false, true, true)]
        [SerializeField] private RotationData rotationData;
        #endregion

        #region Buttons
        [ButtonGroup(characterSettingsCreatorTitlePreffix + buttonsSuffix)]
        public void CreateCharacterSettings() => CreateCharacterSettingsProcess();
        #endregion

        #endregion

        #region Core
        [MenuItem(menuItemName)]
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
            bool saveFolderPathIsNullOrEmpty = string.IsNullOrEmpty(saveFolderPath);
            bool characterSettingsNameIsNullOrEmpty = string.IsNullOrEmpty(characterSettingsName);
            string name = characterSettingsNameIsNullOrEmpty ? defaultCharacterSettingsName : characterSettingsName;

            if (saveFolderPathIsNullOrEmpty)
            {
                Debug.LogError(saveFolderPathError);
                return;
            }

            CharacterSettings characterSettings = CreateInstance<CharacterSettings>();
            characterSettings.name = name;
            characterSettings.InputData = inputData;
            characterSettings.MovementData = movementData;
            characterSettings.RotationData = rotationData;

            string filePath = $"{saveFolderPath}/{name}{assetSuffix}";
            AssetDatabase.CreateAsset(characterSettings, filePath);
            AssetDatabase.SaveAssets();

            Debug.Log(characterSettingsCreatedLog + $"{filePath}");
        }
        #endregion
    }
}
