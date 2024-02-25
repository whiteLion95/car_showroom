using UnityEngine;
using UnityEditor;
using System.IO;


[InitializeOnLoad]
public class PreloadSigningAlias
{

    static PreloadSigningAlias()
    {
        PlayerSettings.Android.keystorePass = "carshowroom";
        PlayerSettings.Android.keyaliasName = "carshowroom";
        PlayerSettings.Android.keyaliasPass = "carshowroom";
    }

}
