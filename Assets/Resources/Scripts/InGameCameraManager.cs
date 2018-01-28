using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCameraManager : MonoBehaviour {

    public GameObject playerToFollow;
    public GameObject playerCameraFollow;

    public float mipMapBias;

    public Vector2 cameraBoundsMin, cameraBoundsMax;

    void Start () {
    }
    
    void Update () {
        GetComponent<Camera>().targetTexture.mipMapBias = mipMapBias;
    }

    void LateUpdate() {
        if (playerCameraFollow != null) {
            transform.position = new Vector3(playerCameraFollow.transform.position.x, playerCameraFollow.transform.position.y, transform.position.z);
        }

        // CAMERA BOUNDS
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, cameraBoundsMin.x, cameraBoundsMax.x),
            Mathf.Clamp(transform.position.y, cameraBoundsMin.y, cameraBoundsMax.y), transform.position.z);
    }

    internal void SetPixelEffectIntensity(float intensity) {
        mipMapBias = intensity;
    }
}
