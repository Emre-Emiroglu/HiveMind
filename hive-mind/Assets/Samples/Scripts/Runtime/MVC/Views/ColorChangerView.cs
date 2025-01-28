using System;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
{
    public sealed class ColorChangerView : View
    {
        #region Actions
        public event Action<int> ClickChangeColorButton; 
        #endregion
        
        #region Fields
        [Header("Color Changer View Fields")]
        [SerializeField] private Button[] changeColorButtons;
        [SerializeField] private TextMeshProUGUI[] changeColorButtonTexts;
        #endregion

        #region Getters
        public Button[] ChangeColorButtons => changeColorButtons;
        public TextMeshProUGUI[] ChangeColorButtonTexts => changeColorButtonTexts;
        #endregion

        #region ButtonReceivers
        public void OnChangeColorButtonClicked(int index) => ClickChangeColorButton?.Invoke(index);
        #endregion
    }
}