using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
    public sealed class SettingsButtonView: View
    {
        #region Fields
        [Header("Settings Button View Fields")]
        [SerializeField] private GameObject verticalGroup;
        [SerializeField] private Button button;
        [SerializeField] private Button exitButton;
        [SerializeField] private SettingsButtonsMap settingsButtons;
        [SerializeField] private SettingsOnImagesMap settingsOnImages;
        [SerializeField] private SettingsOffImagesMap settingsOffImages;
        #endregion

        #region Getters
        public GameObject VerticalGroup => verticalGroup;
        public Button Button => button;
        public Button ExitButton => exitButton;
        public Dictionary<SettingsTypes, Button> SettingsButtons => settingsButtons.Clone();
        public Dictionary<SettingsTypes, GameObject> SettingsOnImages => settingsOnImages.Clone();
        public Dictionary<SettingsTypes, GameObject> SettingsOffImages => settingsOffImages.Clone();
        #endregion
    }
    
    [Serializable]
    public sealed class SettingsButtonsMap : SerializableDictionaryBase<SettingsTypes, Button> { }
    [Serializable]
    public sealed class SettingsOnImagesMap : SerializableDictionaryBase<SettingsTypes, GameObject> { }
    [Serializable]
    public sealed class SettingsOffImagesMap : SerializableDictionaryBase<SettingsTypes, GameObject> { }
}
