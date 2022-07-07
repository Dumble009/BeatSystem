using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools.Utils;
using UnityEngine.TestTools;
using UnityEngine;

public class NeedleControllerTest
{
    public IEnumerator ArduinoControllTest()
    {
        var musicPaseGO = new GameObject("MusicPase");
        var musicPase = musicPaseGO.AddComponent<MockMusicPase>();
        yield return null;
    }
}
