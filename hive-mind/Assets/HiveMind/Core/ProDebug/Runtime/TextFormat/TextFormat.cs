namespace HiveMind.Core.ProDebug.Runtime.TextFormat
{
    public sealed class TextFormat
    {
        #region Fields
        public static TextFormat Bold = new TextFormat("b");
        public static TextFormat Italic = new TextFormat("i");
        private string prefix;
        private string suffix;
        #endregion

        #region Constructor
        private TextFormat(string format)
        {
            prefix = $"<{format}>";
            suffix = $"</{format}>";
        }
        #endregion

        #region Operators
        public static string operator %(string text, TextFormat textFormat)
        {
            return textFormat.prefix + text + textFormat.suffix;
        }
        #endregion
    }
}
