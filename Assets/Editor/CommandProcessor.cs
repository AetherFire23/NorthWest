using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
namespace Assets.Automation
{
    public class CommandProcessor
    {
        [MenuItem("Customs/Refresh Assets")]
        public static void RefreshAndPlay()
        {
            AssetDatabase.Refresh();
            Thread.Sleep(10);
            EditorApplication.isPlaying = true;
        }
    }
}
