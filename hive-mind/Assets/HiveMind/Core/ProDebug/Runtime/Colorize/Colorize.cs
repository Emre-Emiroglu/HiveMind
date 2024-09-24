using UnityEngine;

namespace HiveMind.Core.ProDebug.Runtime.Colorize
{
    public sealed class Colorize
    {
        #region Colors
        public static Colorize Red = new(Color.red);
        public static Colorize Yellow = new(Color.yellow);
        public static Colorize Green = new(Color.green);
        public static Colorize Blue = new(Color.blue);
        public static Colorize Cyan = new(Color.cyan);
        public static Colorize Magenta = new(Color.magenta);
        #endregion

        #region HexColors
        public static Colorize Orange = new("#FFA500");
        public static Colorize Olive = new("#808000");
        public static Colorize Purple = new("#800080");
        public static Colorize DarkRed = new("#8B0000");
        public static Colorize DarkGreen = new("#006400");
        public static Colorize DarkOrange = new("#FF8C00");
        public static Colorize Gold = new("#FFD700");
        #endregion

        #region Constants
        private const string Suffix = "</color>";
        #endregion
        
        #region Fields
        private readonly string _prefix;
        #endregion

        #region Constructors
        private Colorize(Color color)
        {
            _prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }
        private Colorize(string hexColor)
        {
            _prefix = $"<color={hexColor}>";
        }
        #endregion

        #region Operators
        public static string operator %(string text, Colorize color)
        {
            return color._prefix + text + Suffix;
        }
        #endregion
    }
}
