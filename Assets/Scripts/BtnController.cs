using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BtnController : MonoBehaviour
{
    private bool isMute;
    public GameObject poppnl, savetoastOBJ, shareToastObj, videoToastObj;
    public GameObject VlmBtn1, VlmBtn2, Mute1, Mute2, adsBtn, adsBtn1, shuffleBtn;
    public AudioSource BGmusic, onClickAudSou;
    public TextMeshProUGUI loadingTxt;
    //public Image img1, img2;
    //public GameObject EditPnl, ArchPnl, Archprnt;
    public GameObject GalleryPnl, StartPnl, LoadingPnl, onselectPnl, resetPopupPnl, shopPnl;
    public bool NoSaveClicked = false, doShare = false, doresetChar = false, justSave = false;

    //public Slider slider;
    public static BtnController instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isMute = PlayerPrefs.GetInt("isMute") == 1;
        savetoastOBJ.SetActive(false);
        shareToastObj.SetActive(false);
        VlmBtn1.SetActive(true);
        VlmBtn2.SetActive(true);
        Mute1.SetActive(false);
        Mute2.SetActive(false);
        if (AdsConfig.Get_RemoveAds_Status() == 1)
        {
            adsBtn.SetActive(false);
            adsBtn1.SetActive(false);
        }
    }

    public void VolumeBtn()
    {

        if (isMute)
        {
            onClickAudSou.Play();
            isMute = !isMute;
            BGmusic.Play();
            onClickAudSou.volume = 1f;
            VlmBtn1.SetActive(true);
            VlmBtn2.SetActive(true);
            Mute1.SetActive(false);
            Mute2.SetActive(false);

            PlayerPrefs.SetInt("isMuted", isMute ? 0 : 1);
        }
        else
        {
            isMute = !isMute;
            VlmBtn1.SetActive(false);
            VlmBtn2.SetActive(false);
            Mute1.SetActive(true);
            Mute2.SetActive(true);
            BGmusic.Stop();
            onClickAudSou.Stop();
            onClickAudSou.volume = 0f;
            PlayerPrefs.SetInt("isMuted", isMute ? 1 : 0);
        }

    }

    public IEnumerator ShowToast(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        obj.SetActive(false);
    }

    public void StartBtnClic()
    {
        onClickAudSou.Play();
        StartPnl.SetActive(false);
        JsonContainer.instance.playerEntry = new Entry();
        SetGallaryOnStart.instance.fromGallary = false;
        LoadingPnl.SetActive(true);
        StartCoroutine(LoaidingAnim());
        Assets.instance.ResetCharacter();
        for (int k = 0; k < Spawner.instance.botmScrlParent.childCount; k++)
        {
            Destroy(Spawner.instance.botmScrlParent.GetChild(k).gameObject);

        }
        Spawner.instance.SetBotomPanel(0);
        Spawner.instance.horizScrlpnl.position = Spawner.instance.horizpnlPos;

    }

    public void EditpnlShareBtn()
    {
        doShare = true;
        onClickAudSou.Play();

        Debug.Log("Play");
        AdsMediation.instance.showInterstial();
        JsonContainer.instance.TakeScreenshotandSave();
    }

    public IEnumerator LoaidingAnim()
    {
        LoadingPnl.transform.GetChild(2).GetChild(0).GetComponent<Slider>().DOValue(1, 2);
        yield return new WaitForSeconds(0);
        loadingTxt.text = "Loading.";
        yield return new WaitForSeconds(0.75f);
        loadingTxt.text = "Loading..";
        yield return new WaitForSeconds(0.75f);
        loadingTxt.text = "Loading...";
        yield return new WaitForSeconds(0.75f);

        int shufleCount = PlayerPrefs.GetInt("ShuffleCount", 3);
        SetShuffleBtn(shufleCount);

        int count = PlayerPrefs.GetInt("Pnl" + 2, 1);
        SetAddIcon(count, 2);

        count = PlayerPrefs.GetInt("Pnl" + 7, 1);
        SetAddIcon(count, 7);

        LoadingPnl.SetActive(false);
        LoadingPnl.transform.GetChild(2).GetChild(0).GetComponent<Slider>().value = 0;
        Spawner.instance.BGTrtoMove.gameObject.SetActive(false);
        Spawner.instance.BGTrtoMove.DOPause();
    }

    public void SetAddIcon(int count, int SibIdx)
    {
        if (count == 1)
        {
            Spawner.instance.horizScrlpnl.GetChild(SibIdx).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            Spawner.instance.horizScrlpnl.GetChild(SibIdx).GetChild(1).gameObject.SetActive(false);
        }
    }

    public void SetShuffleBtn(int count)
    {
        if (count == 0)
        {
            shuffleBtn.transform.GetChild(1).gameObject.SetActive(true);
            shuffleBtn.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (count == -1)
        {
            shuffleBtn.transform.GetChild(1).gameObject.SetActive(false);
            shuffleBtn.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            shuffleBtn.transform.GetChild(1).gameObject.SetActive(false);
            shuffleBtn.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = count.ToString();
            shuffleBtn.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void ShuffleBtn()
    {
        int shuffleCount = PlayerPrefs.GetInt("ShuffleCount", 3);
        //Debug.Log("Shuffle Cout Btn = " + shuffleCount);
        if (shuffleCount > 0)
        {
            List<int> idxList = new();
            for (int i = 0; i < Assets.instance.AllLists.Count; i++)
            {
                int num = Random.Range(0, Assets.instance.AllLists[i].Count);
                idxList.Add(num);
            }

            shuffleCount -= 1;
            PlayerPrefs.SetInt("ShuffleCount", shuffleCount);
            Assets.instance.SetCharacter(idxList);
            Spawner.instance.isedited = true;
        }
        else if (shuffleCount == -1)
        {
            List<int> idxList = new();
            for (int i = 0; i < Assets.instance.AllLists.Count; i++)
            {
                int num = Random.Range(0, Assets.instance.AllLists[i].Count);
                idxList.Add(num);
            }
            Assets.instance.SetCharacter(idxList);
            Spawner.instance.isedited = true;
        }
        else
        {
            if (AdsMediation.instance.isRewardedVideoReady())
            {
                AdsMediation.rewardUserNow += rewardShuffles;
                AdsMediation.instance.showRewardedVideo();
            }
            else
            {
                StartCoroutine(BtnController.instance.ShowToast(BtnController.instance.videoToastObj));
            }
            Debug.Log("Play Adds");
        }
        //Debug.Log("After Shuffle Use = " + shuffleCount);
        SetShuffleBtn(shuffleCount);
    }

    void rewardShuffles(bool isComplete)
    {
        if (isComplete)
        {
            PlayerPrefs.SetInt("ShuffleCount", 3);
            SetShuffleBtn(PlayerPrefs.GetInt("ShuffleCount"));
        }
        else
        {

        }
    }

    public void StoreBtn()
    {
        shopPnl.SetActive(true);
        shopPnl.transform.GetChild(0).GetChild(2).GetChild(0).transform.DOScale(1, 1).SetEase(Ease.OutBack);
    }

    public void GalleryBtn()
    {
        onClickAudSou.Play();
        JsonContainer.instance.ResetGallery();
        GalleryPnl.SetActive(true);
    }

    public void SetName(string Name)
    {
        Debug.Log("Name of " + SetGallaryOnStart.instance.CharIndex + "is " + Name);
        JsonContainer.instance.playerEntry.CharName = Name;
    }

    public void OnClickResetCharBtn()
    {
        onClickAudSou.Play();

        Debug.Log("PLay Add");
        AdsMediation.instance.showInterstial();
        resetPopupPnl.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        resetPopupPnl.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(() => onClickAudSou.Play());
        resetPopupPnl.transform.GetChild(0).GetChild(3).GetComponent<Button>().onClick.AddListener(() => Assets.instance.ResetCharacter());
        if (Spawner.instance.isedited)
            resetPopupPnl.SetActive(true);
    }

    public void SaveBtnclick()
    {
        AdsMediation.instance.showInterstial();
        doShare = false;
        justSave = true;
        JsonContainer.instance.TakeScreenshotandSave();
        InAppReviewManager.Instance.AskForReview();
    }

    public void OnClickHomeBtn()
    {
        onClickAudSou.Play();
        if (Spawner.instance.isedited)
        {
            poppnl.SetActive(true);
        }
        else
        {
            OkLeave();
        }

        doShare = false;
        justSave = false;
        AdsMediation.instance.showInterstial();
    }

    public void OkLeave()
    {
        onClickAudSou.Play();

        Screenshot.instanse.isSaved = false;
        poppnl.SetActive(false);
        Spawner.instance.BGTrtoMove.DOPlay();
        StartPnl.SetActive(true);
        Spawner.instance.BGTrtoMove.gameObject.SetActive(true);
        Spawner.instance.horizScrlpnl.position = Spawner.instance.horizpnlPos;
        //AdsMediation.instance.showInterstial();
    }

    public void NoSave()
    {
        onClickAudSou.Play();
        poppnl.SetActive(false);
        NoSaveClicked = true;
        justSave = false;
        Spawner.instance.isedited = false;
        JsonContainer.instance.TakeScreenshotandSave();

    }

    public void onRemoveAds()
    {
        adsBtn.SetActive(false);
        adsBtn1.SetActive(false);
        AdsConfig.Set_RemoveAds_Status();
        AdsMediation.instance.hideBanner();
    }


    public void BuyFirstItem()
    {
        //If buy successfully
        Assets.instance.UnlockAllItems();
        onRemoveAds();

        SetShuffleBtn(-1);
    }

    public void BuyScndItem()
    {
        Assets.instance.UnlockAllItems();
        onRemoveAds();
    }

    public void RestorePurchases()
    {

    }

    public void EscShopPnl()
    {
        shopPnl.transform.GetChild(0).GetChild(2).GetChild(0).transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        shopPnl.SetActive(false);

    }
}
