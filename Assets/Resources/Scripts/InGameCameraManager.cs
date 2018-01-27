using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCameraManager : MonoBehaviour {

    public float mipMapBias;

    void Start () {
        
    }
    
    void Update () {
        GetComponent<Camera>().targetTexture.mipMapBias = mipMapBias;
    }
}
