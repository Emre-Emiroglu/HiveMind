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
        [TitleGroup("Character Settings/Input Data")]
        [SerializeField] private InputData inputData;
        [TitleGroup("Character Settings/Movement Data")]
        [SerializeField] private MovementData movementData;
        [TitleGroup("Character Settings/Rotation Data")]
        [SerializeField] private RotationData rotationData;

        [TitleGroup("Character Settings/Create Buttons")]
        [ButtonGroup("Character Settings/Create Buttons/Buttons")] public void CreateCharacterSettings() { }
        [ButtonGroup("Character Settings/Create Buttons/Buttons")] public void SecondButton() { }
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
    }
}
