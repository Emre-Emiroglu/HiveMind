using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
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
        [SerializeField] private Dictionary<SettingsTypes, Button> _settingsButtons;
        [SerializeField] private Dictionary<SettingsTypes, GameObject> _settingsOnImages;
        [SerializeField] private Dictionary<SettingsTypes, GameObject> _settingsOffImages;
        #endregion

        #region Getters
        public GameObject VerticalGroup => verticalGroup;
        public Button Button => button;
        public Button ExitButton => exitButton;
        public Dictionary<SettingsTypes, Button> SettingsButtons => _settingsButtons;
        public Dictionary<SettingsTypes, GameObject> SettingsOnImages => _settingsOnImages;
        public Dictionary<SettingsTypes, GameObject> SettingsOffImages => _settingsOffImages;
        #endregion
    }
}
