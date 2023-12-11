using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraFetcher : MonoBehaviour
{
    void Start()
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        //GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().worldCamera = GetComponent<Camera>();
        
    }
}
