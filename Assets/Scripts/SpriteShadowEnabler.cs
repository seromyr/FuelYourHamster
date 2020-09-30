using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowEnabler : MonoBehaviour
{
    /// <summary>
    /// WARNING!!!
    /// Only works with custom shader created with shader graph in URP
    /// Read more https://forum.unity.com/threads/urp-shader-for-sprites-to-cast-receive-shadows.950933/
    /// </summary>
    void Start()
    {
            GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            GetComponent<Renderer>().receiveShadows = true;
    }
}
