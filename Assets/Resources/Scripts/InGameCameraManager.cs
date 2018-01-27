using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCameraManager : MonoBehaviour {

    public GameObject playerToFollow;

    public float mipMapBias;

    void Start () {
        
    }
    
    void Update () {
        GetComponent<Camera>().targetTexture.mipMapBias = mipMapBias;

        if (playerToFollow != null)
            transform.position = new Vector3(playerToFollow.transform.position.x, playerToFollow.transform.position.y, transform.position.z);
    }
}
