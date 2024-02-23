using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;



public class SetGallaryOnStart : MonoBehaviour
{
    //GameObject ob = EventSystem.current.currentSelectedGameObject;
    // Start is called before the first frame update

    public Transform GalleryTransform;
    public GameObject charbtn;
    public static SetGallaryOnStart instance;
    public GameObject OnselectPnl, GalleryPnl;
    public int CharIndex = 0;
    public bool fromGallary = false;
    public AudioSource onclick;


    private void Awake()
    {
        instance = this;
    }


    public void OnDataLoaded()
    {
        for (int i = 0; i < JsonContainer.instance.playerData.Entries.Count; i++)
        {

            Entry entry = JsonContainer.instance.playerData.Entries[i];
            GameObject ob = Instantiate(charbtn, GalleryTransform);

            string Imgpath = entry.ImagePath;
            Debug.Log(Imgpath + " Image Name");
            string Name = entry.CharName;
            //Name = PlayerPrefs.GetString("CharName");

            int copyIndex = i;
            //CharIndex = copyIndex;

            ob.transform.GetChild(1).GetComponent<Image>().sprite = Screenshot.instanse.LoadSprite(Imgpath);
            Debug.Log("ImageName: " + entry.ImagePath);

            if (string.IsNullOrEmpty(Name))
            {
                ob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Name " + (i + 1);
            }
            else
                ob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Name;
            Debug.Log("Name: " + Name);

            ob.GetComponent<Button>().onClick.RemoveAllListeners();
            ob.GetComponent<Button>().onClick.AddListener(() => SetSelectedPnl(copyIndex, Imgpath, Name));
        }
    }


    public void SetSelectedPnl(int index, string ImagPath, string Name)
    {
        //onclick.Play();
        BtnController.instance.onClickAudSou.Play();
        fromGallary = true;
        var entry = JsonContainer.instance.playerData.Entries[index];

        CharIndex = index;

        OnselectPnl.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Name;
        OnselectPnl.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Screenshot.instanse.LoadSprite(ImagPath);
        OnselectPnl.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        OnselectPnl.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Button>().onClick.AddListener(() => EditChar(entry.indexes));
        OnselectPnl.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        OnselectPnl.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Button>().onClick.AddListener(() => JsonContainer.instance.OnDeleteList(CharIndex));
        OnselectPnl.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        OnselectPnl.transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Button>().onClick.AddListener(() => OnclickShareBtn(ImagPath));
        OnselectPnl.SetActive(true);
    }

    public void EditChar(List<int> list)
    {
        BtnController.instance.onClickAudSou.Play();
        Assets.instance.SetCharacter(list);
        fromGallary = true;
        Debug.Log("Character is set.");
        Debug.Log(CharIndex + "CharIndex");
        Spawner.instance.BGTrtoMove.DOPause();
        //startPnl.SetActive(false);
        BtnController.instance.StartPnl.SetActive(false);
        Spawner.instance.BGTrtoMove.gameObject.SetActive(false);
        GalleryPnl.SetActive(false);
        OnselectPnl.SetActive(false);

        Debug.Log("Deactivated");

    }

    public void OnclickShareBtn(string ImagePath)
    {
        BtnController.instance.onClickAudSou.Play();
        Debug.Log("Shairing Image ");
        //BtnController.instance.savetoastOBJ.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Share Successful!!!";
        Debug.Log(BtnController.instance.savetoastOBJ.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        BtnController.instance.savetoastOBJ.SetActive(false);
        StartCoroutine(BtnController.instance.ShowToast(BtnController.instance.shareToastObj));

        new NativeShare().AddFile(ImagePath)
          .SetSubject("Avatar").SetText("Avatar").Share();
    }


}//class
