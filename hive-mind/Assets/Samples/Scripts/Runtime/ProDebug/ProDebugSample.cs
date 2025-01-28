using CodeCatGames.HiveMind.Core.Runtime.ProDebug.Colorize;
using CodeCatGames.HiveMind.Core.Runtime.ProDebug.TextFormat;
using TMPro;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.ProDebug
{
    public sealed class ProDebugSample : MonoBehaviour
    {
        #region Fields
        [Header("Pro Debug Sample Fields")]
        [SerializeField] private TMP_Dropdown colorizeDropdown;
        [SerializeField] private TMP_Dropdown textFormatDropdown;
        private int _colorizeDropdownValue;
        private int _textFormatDropdownValue;
        #endregion
        
        #region ButtonReceivers
        public void OnColorizeDropdownValueChanged() => _colorizeDropdownValue = colorizeDropdown.value;
        public void OnTextFormatDropdownValueChanged() => _textFormatDropdownValue = textFormatDropdown.value;
        public void OnShowDebugButtonClicked()
        {
            Colorize colorize = _colorizeDropdownValue switch
            {
                0 => Colorize.Red,
                1 => Colorize.Yellow,
                2 => Colorize.Green,
                3 => Colorize.Blue,
                4 => Colorize.Cyan,
                5 => Colorize.Magenta,
                6 => Colorize.Orange,
                7 => Colorize.Olive,
                8 => Colorize.Purple,
                9 => Colorize.DarkRed,
                10 => Colorize.DarkGreen,
                11 => Colorize.DarkOrange,
                12 => Colorize.Gold,
                _ => null
            };

            TextFormat textFormat = _textFormatDropdownValue switch
            {
                0 => TextFormat.Bold,
                1 => TextFormat.Italic,
                _ => null
            };
            
            string logMessage = "Log Message" % colorize % textFormat;
            
            Debug.Log(logMessage);
        }
        #endregion
    }
}