using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class Spawner : MonoBehaviour
{
    public Transform botmScrlParent, BGTrtoMove, horizScrlpnl;
    public GameObject botmBtn, crossBtn;
    public GameObject startPnlCHar;
    public List<Sprite> spriteList, spriteListtoShow, Characters;
    Image spot;
    public Tween tween;
    public int whatpnl = 0, buttonCount = 0;
    public bool isedited = false, adPlayed = false;
    public Vector3 btmpnlPos, horizpnlPos;

    public static Spawner instance;


    private void Awake()
    {
        instance = this;
        btmpnlPos = botmScrlParent.position;
        horizpnlPos = horizScrlpnl.position;

    }

    // Start is called before the first frame update
    void Start()
    {
        //horizScrlpnl.position = new Vector3(300, 200, 0);
        startPnlCHar.GetComponent<Image>().sprite = Characters[Random.Range(0, Characters.Count)];
        float width = BGTrtoMove.gameObject.GetComponent<RectTransform>().rect.width - 250f;
        Debug.Log(width + "Width of BG");
        tween = BGTrtoMove.DOMoveX(BGTrtoMove.position.x + width, 6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

    }



    public void SetBotomPanel(int pnlIdx)
    {

        Debug.Log("Setting Botm Pnl " + pnlIdx);
        spriteListtoShow = Assets.instance.AllListstoShow[pnlIdx];
        spriteList = Assets.instance.AllLists[pnlIdx];
        spot = Assets.instance.Spots[pnlIdx];
        whatpnl = pnlIdx;
        //Debug.Log(spriteList.Count);

        for (int i = 0; i < horizScrlpnl.gameObject.transform.childCount; i++)
        {
            horizScrlpnl.gameObject.transform.GetChild(i).GetComponent<Image>().enabled = (whatpnl == i);
        }

        if (whatpnl != 0 && whatpnl != 1 && whatpnl != 7 && whatpnl != 10)
        {
            GameObject obj = Instantiate(crossBtn, botmScrlParent);
            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            obj.GetComponent<Button>().onClick.AddListener(() => DisableImgSpot());
        }

        for (int i = 0; i < spriteListtoShow.Count; i++)
        {

            GameObject obj = Instantiate(botmBtn, botmScrlParent);

            if (spriteListtoShow == Assets.instance.EaringToshow || spriteListtoShow == Assets.instance.EyetoShow)
            {
                obj.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            if (Assets.instance.playerPrefsindex[pnlIdx][i] == 0)
            {
                obj.transform.GetChild(1).gameObject.SetActive(false);
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() => SpawnImage());
            }
            else
            {
                if (pnlIdx == 0)
                {
                    obj.transform.GetChild(1).gameObject.SetActive(false);
                    obj.GetComponent<Button>().onClick.RemoveAllListeners();
                    obj.GetComponent<Button>().onClick.AddListener(() => SpawnImage());
                }
                else
                {
                    obj.transform.GetChild(1).gameObject.SetActive(true);
                    int copyIndexi = pnlIdx, copyIndexj = i;
                    obj.GetComponent<Button>().onClick.RemoveAllListeners();
                    obj.GetComponent<Button>().onClick.AddListener(() => PlayAdforItem(copyIndexi, copyIndexj));
                }

            }
            obj.transform.GetChild(0).GetComponent<Image>().sprite = spriteListtoShow[i];
        }
        if (!adPlayed)
            botmScrlParent.gameObject.GetComponent<RectTransform>().position = btmpnlPos;
        adPlayed = false;
    }



    public void SpawnImage()
    {
        //setting spot as hair spot
        BtnController.instance.onClickAudSou.Play();
        //spot = Assets.instance.Spots[0];

        GameObject obj = EventSystem.current.currentSelectedGameObject;
        int whatsprite = obj.transform.GetSiblingIndex();

        Debug.Log(obj.name + "  ,  " + whatsprite);

        //SelectSpot();


        if (whatpnl != 0 && whatpnl != 1 && whatpnl != 7 && whatpnl != 10)
        {
            whatsprite--;

        }

        //for (int i = 0; i < Assets.instance.AllLists.Count; i++)
        //{
        //    if (spriteList == Assets.instance.AllListstoShow[i])
        //    {
        //        spriteList = Assets.instance.AllLists[i];
        //        BtnController.instance.doresetChar = true;
        //        break;
        //    }
        //}
        //else
        //    spriteList = Assets.instance.AllLists[idx];


        for (int i = 0; i < spriteList.Count; i++)
        {
            if (i == whatsprite)
            {
                spot.enabled = true;
                Screenshot.instanse.isSaved = false;
                Debug.Log(JsonContainer.instance.playerEntry.indexes.Count + " List Count ");
                JsonContainer.instance.playerEntry.indexes[whatpnl] = whatsprite;
                spot.sprite = spriteList[i];
                buttonCount++;
                isedited = true;
                Debug.Log("Button Count = " + buttonCount);
                if (buttonCount == 20)
                {
                    AdsMediation.instance.showInterstial();
                    buttonCount = 0;
                }
                break;
            }
        }
    }


    public void SelectSpot()
    {
        for (int i = 0; i < Assets.instance.Spots.Count; i++)
        {
            if (i == whatpnl)
            {
                spot = Assets.instance.Spots[i];
                break;
            }
        }

        Debug.Log("Selecte spot idx == " + spot.name);
    }


    public void DisableImgSpot()
    {
        BtnController.instance.onClickAudSou.Play();
        //SelectSpot();
        JsonContainer.instance.playerEntry.indexes[whatpnl] = -1;
        spot.enabled = false;
        BtnController.instance.doresetChar = true;
        isedited = true;

    }

    int savedI, savedJ;

    public void PlayAdforItem(int i, int j)
    {
        if (AdsMediation.instance.isRewardedVideoReady())
        {
            savedI = i;
            savedJ = j;
            AdsMediation.rewardUserNow += onVideoComplete;
            AdsMediation.instance.showRewardedVideo();
        }
        else
        {
            StartCoroutine(BtnController.instance.ShowToast(BtnController.instance.videoToastObj));
        }
    }

    void onVideoComplete(bool status)
    {
        if (status)
        {
            BtnController.instance.onClickAudSou.Play();
            PlayerPrefs.SetInt("idx_[" + savedI + "][" + savedJ + "]", 0);
            Assets.instance.playerPrefsindex = Assets.instance.GetPlayerprefs();

            Debug.Log("PlayAd");

            for (int k = 0; k < botmScrlParent.childCount; k++)
            {
                Destroy(botmScrlParent.GetChild(k).gameObject);

            }
            adPlayed = true;
            SetBotomPanel(savedI);
            SpawnImage();
        }
        else
        {

        }
    }

    GameObject selectedCat = null;
    public void PnlIndex()
    {
        selectedCat = EventSystem.current.currentSelectedGameObject;
        whatpnl = selectedCat.transform.GetSiblingIndex();

        if (whatpnl == 2 || whatpnl == 7)
        {
            if (PlayerPrefs.GetInt("Pnl" + whatpnl) == 1)
            {
                if (AdsMediation.instance.isRewardedVideoReady())
                {
                    AdsMediation.rewardUserNow += unlockCategorey;
                    AdsMediation.instance.showRewardedVideo();
                }
                else
                {
                    StartCoroutine(BtnController.instance.ShowToast(BtnController.instance.videoToastObj));
                }
                return;
            }
        }

        for (int i = 0; i < botmScrlParent.childCount; i++)
        {
            Destroy(botmScrlParent.GetChild(i).gameObject);
        }

        Debug.Log(selectedCat.transform.parent.childCount);
        Debug.Log(whatpnl);
        for (int i = 0; i < selectedCat.transform.parent.childCount; i++)
        {
            selectedCat.transform.parent.GetChild(i).GetComponent<Image>().enabled = (whatpnl == i);
        }

        Debug.Log("Index " + whatpnl);
        SetBotomPanel(whatpnl);
    }

    void unlockCategorey(bool isComplete)
    {
        if (isComplete)
        {
            for (int i = 0; i < botmScrlParent.childCount; i++)
            {
                Destroy(botmScrlParent.GetChild(i).gameObject);
            }
            Debug.Log("PlayAdd For Pnl += " + whatpnl);
            PlayerPrefs.SetInt("Pnl" + whatpnl, 0);
            horizScrlpnl.GetChild(whatpnl).GetChild(1).gameObject.SetActive(false);
            for (int i = 0; i < selectedCat.transform.parent.childCount; i++)
            {
                selectedCat.transform.parent.GetChild(i).GetComponent<Image>().enabled = (whatpnl == i);
            }
            SetBotomPanel(whatpnl);
        }
        else
        {
            //Show some message here if vide is not completed
        }
    }

}//class
