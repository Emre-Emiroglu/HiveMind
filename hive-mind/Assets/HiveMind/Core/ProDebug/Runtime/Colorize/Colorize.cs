using UnityEngine;

namespace HiveMind.Core.ProDebug.Runtime.Colorize
{
    public sealed class Colorize
    {
        #region Colors
        public static Colorize Red = new Colorize(Color.red);
        public static Colorize Yellow = new Colorize(Color.yellow);
        public static Colorize Green = new Colorize(Color.green);
        public static Colorize Blue = new Colorize(Color.blue);
        public static Colorize Cyan = new Colorize(Color.cyan);
        public static Colorize Magenta = new Colorize(Color.magenta);
        #endregion

        #region HexColors
        public static Colorize Orange = new Colorize("#FFA500");
        public static Colorize Olive = new Colorize("#808000");
        public static Colorize Purple = new Colorize("#800080");
        public static Colorize DarkRed = new Colorize("#8B0000");
        public static Colorize DarkGreen = new Colorize("#006400");
        public static Colorize DarkOrange = new Colorize("#FF8C00");
        public static Colorize Gold = new Colorize("#FFD700");
        #endregion

        #region Fields
        private const string SUFFIX = "</color>";
        private readonly string prefix;
        #endregion

        #region Constructors
        private Colorize(Color color)
        {
            prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }
        private Colorize(string hexColor)
        {
            prefix = $"<color={hexColor}>";
        }
        #endregion

        #region Operators
        public static string operator %(string text, Colorize color)
        {
            return color.prefix + text + SUFFIX;
        }
        #endregion
    }
}
