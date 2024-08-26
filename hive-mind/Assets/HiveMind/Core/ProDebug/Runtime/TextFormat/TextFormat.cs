namespace HiveMind.Core.ProDebug.Runtime.TextFormat
{
    public sealed class TextFormat
    {
        #region Fields
        public static TextFormat Bold = new("b");
        public static TextFormat Italic = new("i");
        private readonly string _prefix;
        private readonly string _suffix;
        #endregion

        #region Constructor
        private TextFormat(string format)
        {
            _prefix = $"<{format}>";
            _suffix = $"</{format}>";
        }
        #endregion

        #region Operators
        public static string operator %(string text, TextFormat textFormat)
        {
            return textFormat._prefix + text + textFormat._suffix;
        }
        #endregion
    }
}
