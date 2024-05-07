using HiveMind.Core.ProDebug.Runtime.Colorize;
using HiveMind.Core.ProDebug.Runtime.TextFormat;
using UnityEngine;

namespace HiveMind.Samples.ProDebugSample.SampleClasses
{
    public class SampleClass1 : MonoBehaviour
    {
        #region Logs
        [ContextMenu("Log")]
        public void Log()
        {
            //Colors
            Debug.Log("Hello Red" % Colorize.Red);
            Debug.Log("Hello Yellow" % Colorize.Yellow);
            Debug.Log("Hello Green" % Colorize.Green);
            Debug.Log("Hello Blue" % Colorize.Blue);
            Debug.Log("Hello Cyan" % Colorize.Cyan);
            Debug.Log("Hello Magenta" % Colorize.Magenta);

            //HexColors
            Debug.Log("Hello Orange" % Colorize.Orange);
            Debug.Log("Hello Olive" % Colorize.Olive);
            Debug.Log("Hello Purple" % Colorize.Purple);
            Debug.Log("Hello DarkRed" % Colorize.DarkRed);
            Debug.Log("Hello DarkGreen" % Colorize.DarkGreen);
            Debug.Log("Hello DarkOrange" % Colorize.DarkOrange);
            Debug.Log("Hello Gold" % Colorize.Gold);

            //TextFormas
            Debug.Log("Hello Bold Red" % Colorize.Red % TextFormat.Bold);
            Debug.Log("Hello Italic Orange" % Colorize.Orange % TextFormat.Italic);

            //Mix
            Debug.Log(
                "ProDebug" % Colorize.Green % TextFormat.Bold + " is perfect tool. " + 
                "Attention!" % Colorize.Red % TextFormat.Bold + " this tool is so dope. " +
                "Maybe I will improve this tool." % Colorize.Purple % TextFormat.Italic
            );
        }
        #endregion
    }
}
