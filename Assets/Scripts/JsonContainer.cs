using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;


public class JsonContainer : MonoBehaviour
{

    public PlayerData playerData;
    public Entry playerEntry;
    public AudioSource onDelete;
    public GameObject savePnl;
    public static JsonContainer instance;
    //public bool isSaved = false;


    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        var content = PlayerPrefs.GetString("user_data");
        if (!string.IsNullOrEmpty(content))
        {
            Debug.Log("content " + content);
            playerData = JsonUtility.FromJson<PlayerData>(content);
        }
        else
        {
            playerData = new PlayerData();
        }
        //playerEntry = new Entry();
        SetGallaryOnStart.instance.OnDataLoaded();
        ResetGallery();
    }

    public void ResetGallery()
    {
        for (int i = 0; i < SetGallaryOnStart.instance.GalleryTransform.transform.childCount; i++)
        {
            Destroy(SetGallaryOnStart.instance.GalleryTransform.transform.GetChild(i).gameObject);
        }
        playerData = new PlayerData();
        var content = PlayerPrefs.GetString("user_data");
        if (!string.IsNullOrEmpty(content))
        {
            playerData = JsonUtility.FromJson<PlayerData>(content);
        }
        SetGallaryOnStart.instance.OnDataLoaded();

    }

    public void TakeScreenshotandSave()
    {

        string ImgPath = "";
        ImgPath = Screenshot.instanse.TakeScreenShot(filePath =>
        {
            AdsMediation.instance.showBanner();
            if (BtnController.instance.doShare)
            {
                Debug.Log("Share Clicked");
                SetGallaryOnStart.instance.OnclickShareBtn(ImgPath);
            }
            if (BtnController.instance.justSave)
            {
                Debug.Log("Just Save");
                //BtnController.instance.toastObj.SetActive(true);
                //BtnController.instance.savetoastOBJ.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Saved to Album";
                StartCoroutine(BtnController.instance.ShowToast(BtnController.instance.savetoastOBJ));

            }
            if (BtnController.instance.NoSaveClicked)
            {
                SetsavePnl(filePath);
            }
        });

    }
    public void SetsavePnl(string filePath)
    {
        savePnl.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Screenshot.instanse.LoadSprite(filePath);
        savePnl.transform.GetChild(0).GetChild(3).GetComponent<TMPro.TMP_InputField>().text = playerEntry.CharName;

        savePnl.transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
        savePnl.transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(() => SaveToJson(filePath));

        savePnl.SetActive(true);

    }
    public void SaveToJson(string filePath)
    {
        BtnController.instance.onClickAudSou.Play();
        Spawner.instance.horizScrlpnl.position = Spawner.instance.horizpnlPos;
        bool fromGallery = SetGallaryOnStart.instance.fromGallary;
        playerEntry.ImagePath = filePath;
        Debug.Log(fromGallery);
        if (fromGallery)
        {
            playerData.Entries[SetGallaryOnStart.instance.CharIndex] = playerEntry;
        }
        else
        {

            if (!SetGallaryOnStart.instance.fromGallary)
            {
                playerData.Entries.Add(playerEntry);
                //SetGallaryOnStart.instance.fromGallary = false;
            }
            else
            {
                //var n = playerData.Entries.Count;
                //playerData.Entries[n - 1] = playerEntry;
                Debug.Log("PlayerNumber " + SetGallaryOnStart.instance.CharIndex);
                playerData.Entries[SetGallaryOnStart.instance.CharIndex] = playerEntry;
            }
        }

        string content = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("user_data", content);
        Debug.Log(content);
        playerData = JsonUtility.FromJson<PlayerData>(content);
        // resetArchive();
        savePnl.SetActive(false);
        Screenshot.instanse.isSaved = true;

        if (BtnController.instance.NoSaveClicked)
        {
            BtnController.instance.NoSaveClicked = false;
            Spawner.instance.BGTrtoMove.DOPlay();
            Spawner.instance.BGTrtoMove.gameObject.SetActive(true);
            BtnController.instance.StartPnl.SetActive(true);
        }
    }


    public void OnDeleteList(int deleteIndex)
    {
        Debug.Log("In Delete mode and the index is : " + deleteIndex);
        //onDelete.Play();
        BtnController.instance.onClickAudSou.Play();
        for (int i = 0; i < playerData.Entries.Count; i++)
        {
            if (i == deleteIndex)
            {
                playerData.Entries.RemoveAt(i);
                Debug.Log("Removed");
                string content = PlayerPrefs.GetString("user_data");
                Debug.Log(content);
                string jsonText = JsonUtility.ToJson(playerData);
                PlayerPrefs.SetString("user_data", jsonText);
                BtnController.instance.onselectPnl.SetActive(false);
                break;
            }
        }
        /// OnLoadManager.instance.OnselectPnl.SetActive(false);
        ResetGallery();
    }


}//Class


[System.Serializable]
public class PlayerData
{
    public List<Entry> Entries = new List<Entry>();
}

[System.Serializable]
public class Entry
{
    public List<int> indexes = new List<int>();
    public string ImagePath;
    public string CharName = "";

    public Entry()
    {

        indexes = new() { 1, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        Debug.Log("Serializint indexes Count = " + indexes.Count);

    }
}
