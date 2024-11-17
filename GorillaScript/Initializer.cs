using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;

namespace GorillaScript
{
    [BepInPlugin("com.kosher.gorillascript", "Gorilla Script", "1.0.0")]
    public class Initializer : BaseUnityPlugin
    {
        void Start()
        {
            // sigma creation!!
            GameObject holder = new GameObject("GorillaScriptHolder");
            holder.AddComponent<GorillaScriptManager>();
            DontDestroyOnLoad(holder);
        }
    }
}