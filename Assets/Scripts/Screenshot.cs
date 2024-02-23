using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class Screenshot : MonoBehaviour
{
    public GameObject vlmBtn, editPnl;
    public Sprite sprite;
    public bool isSaved = false;
    public Transform ssPoint;

    public static Screenshot instanse;

    private void Awake()
    {
        instanse = this;
    }
    private void Start()
    {
        isSaved = false;
    }

    public string TakeScreenShot(Action<string> callback)
    {
        string thmName = "ScreenShot " + DateTime.Now.ToString("yyy-MM-dd_HH-mm-ss") + ".png";
        string filePath = Path.Combine(Application.persistentDataPath, thmName);

        vlmBtn.SetActive(false);
        BtnController.instance.adsBtn.SetActive(false);
        BtnController.instance.shuffleBtn.SetActive(false);
        editPnl.SetActive(false);
        AdsMediation.instance.hideBanner();

        StartCoroutine(ScreenShot(filePath, callback));
        return filePath;
    }



    private IEnumerator ScreenShot(string filePath, Action<string> callback)
    {
        Debug.Log("Starting screenshot coroutine");
        yield return new WaitForEndOfFrame();

        var startX = 0;
        var startY = ssPoint.position.y;

        var tex = new Texture2D(Screen.width, Screen.height - ((int)startY), TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(startX, startY, Screen.width, Screen.height - ((int)startY)), 0, 0);
        tex.Apply();
        NativeGallery.SaveImageToGallery(tex, "Bored Ape Creator", "Name " + SetGallaryOnStart.instance.CharIndex);

        Debug.Log("Captured");
        byte[] byteArray = tex.EncodeToPNG();
        Destroy(tex);
        File.WriteAllBytes(filePath, byteArray);
        Debug.Log("Write");
        Debug.Log(filePath);
        vlmBtn.SetActive(true);
        BtnController.instance.shuffleBtn.SetActive(true);
        //if (AdsConfig.Get_RemoveAds_Status() == 0)
        //{
        //    BtnController.instance.adsBtn.SetActive(true);
        //}
        editPnl.SetActive(true);

        callback(filePath);
    }

    public Sprite LoadSprite(string path)
    {
        Debug.Log("Loading Image");
        string filePath = path;
        byte[] fileData;

        Texture2D tex;
        Debug.Log("The File Path  is" + filePath);
        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.0f), 1.0f);
        }
        Debug.Log("SpriteReturned" + filePath);
        return sprite;
    }

}//class