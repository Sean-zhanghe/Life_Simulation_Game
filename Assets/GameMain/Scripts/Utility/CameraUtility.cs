using GameFramework.Resource;
using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUtility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameEntry.Resource.LoadAsset(AssetUtility.GetRenderTexture("MinimapRenderTexture"), Constant.AssetPriority.UIFormAsset, new LoadAssetCallbacks(
        //    (assetName, asset, duration, userData) =>
        //    { 
        //        transform.GetComponent<Camera>().targetTexture = (RenderTexture)asset;
        //        Debug.Log("22222222222222");
        //    }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
