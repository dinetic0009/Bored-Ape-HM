using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//using UnityEngine.UIElements;


public class Assets : MonoBehaviour
{

    public List<Sprite> BodyTone = new();
    public List<Sprite> Eye = new();
    public List<Sprite> Mouth = new();
    public List<Sprite> Glasses = new();
    public List<Sprite> Hair = new();
    public List<Sprite> EarRings = new();
    public List<Sprite> Necklace = new();
    public List<Sprite> Shirt = new();
    public List<Sprite> Hat = new();
    public List<Sprite> Hand = new();
    public List<Sprite> BGs = new();

    public List<Sprite> EyetoShow = new();
    public List<Sprite> MouthtoShow = new();
    public List<Sprite> GlassestOShow = new();
    public List<Sprite> HairtoShow = new();
    public List<Sprite> EaringToshow = new();
    public List<Sprite> HandtoShow = new();
    public List<Sprite> NecklacetoShow = new();
    public List<Sprite> BGstoShow = new();

    List<int> intBodyTone = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    List<int> intEye = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 1 };
    List<int> intMouth = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1 };
    List<int> intGlasses = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1 };
    List<int> intHair = new() { 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 10, 1, 1, 0, 0, 1, 1, 0, 1, 0 };
    List<int> intEarRings = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 1 };
    List<int> intNecklace = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1 };
    List<int> intShirt = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1 };
    List<int> intHat = new() { 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1 };
    List<int> intHand = new() { 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0 };
    List<int> intBGs = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


    //Adding according to rhe sequence of buttons
    public List<Image> Spots = new List<Image>();

    public List<List<Sprite>> AllLists = new();
    public List<List<Sprite>> AllListstoShow = new();
    public List<List<int>> playerPrefsindex = new();


    public static Assets instance;

    private void Awake()
    {
        instance = this;

        AllLists.Add(BodyTone);
        AllLists.Add(Eye);
        AllLists.Add(Mouth);
        AllLists.Add(Glasses);
        AllLists.Add(Hair);
        AllLists.Add(EarRings);
        AllLists.Add(Necklace);
        AllLists.Add(Shirt);
        AllLists.Add(Hat);
        AllLists.Add(Hand);
        AllLists.Add(BGs);

        AllListstoShow.Add(BodyTone);
        AllListstoShow.Add(EyetoShow);
        AllListstoShow.Add(MouthtoShow);
        AllListstoShow.Add(GlassestOShow);
        AllListstoShow.Add(HairtoShow);
        AllListstoShow.Add(EaringToshow);
        AllListstoShow.Add(NecklacetoShow);
        AllListstoShow.Add(Shirt);
        AllListstoShow.Add(Hat);
        AllListstoShow.Add(HandtoShow);
        AllListstoShow.Add(BGstoShow);

        int bol = PlayerPrefs.GetInt("Bool", 0);
        if (bol == 0)
        {
            playerPrefsindex.Add(intBodyTone);
            playerPrefsindex.Add(intEye);
            playerPrefsindex.Add(intMouth);
            playerPrefsindex.Add(intGlasses);
            playerPrefsindex.Add(intHair);
            playerPrefsindex.Add(intEarRings);
            playerPrefsindex.Add(intNecklace);
            playerPrefsindex.Add(intShirt);
            playerPrefsindex.Add(intHat);
            playerPrefsindex.Add(intHand);
            playerPrefsindex.Add(intBGs);

            PlayerPrefs.SetInt("Pnl" + 2, 1);
            PlayerPrefs.SetInt("Pnl" + 7, 1);

            SetPlayerPrefs();
            PlayerPrefs.SetInt("Bool", 1);

        }

        playerPrefsindex = GetPlayerprefs();

        //Spawner.instance.SetBotomPanel(0);

        Spawner.instance.spriteList = AllLists[0];
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetCharacter();
    }

    public void SetPlayerPrefs()
    {
        for (int i = 0; i < playerPrefsindex.Count; i++)
        {
            for (int j = 0; j < playerPrefsindex[i].Count; j++)
            {
                PlayerPrefs.SetInt("idx_[" + i + "][" + j + "]", playerPrefsindex[i][j]);
            }
        }
    }

    public List<List<int>> GetPlayerprefs()
    {
        List<List<int>> allListTemp = new();

        for (int i = 0; i < AllLists.Count; i++)
        {
            List<int> list = new();
            //          int totalAdds = (AllLists[i].Count * 60) / 100;
            //            Debug.Log("Percentage% = " + totalAdds);

            for (int j = 0; j < AllLists[i].Count; j++)
            {
                if (j < 5)
                {
                    list.Add(PlayerPrefs.GetInt("idx_[" + i + "][" + j + "]"));
                }
                else
                    list.Add(PlayerPrefs.GetInt("idx_[" + i + "][" + j + "]"));
            }
            allListTemp.Add(list);
        }
        return allListTemp;
    }

    public void UnlockAllItems()
    {
        playerPrefsindex = GetPlayerprefs();
        for (int i = 0; i < playerPrefsindex.Count; i++)
        {
            for (int j = 0; j < playerPrefsindex[i].Count; j++)
            {
                PlayerPrefs.SetInt("idx_[" + i + "][" + j + "]", 0);
            }
        }

        playerPrefsindex = GetPlayerprefs();
        for (int i = 0; i < Spawner.instance.botmScrlParent.childCount; i++)
        {
            Destroy(Spawner.instance.botmScrlParent.GetChild(i).gameObject);
        }
        Spawner.instance.SetBotomPanel(0);
        for (int i = 0; i < Spawner.instance.horizScrlpnl.GetChild(0).transform.parent.childCount; i++)
        {
            Spawner.instance.horizScrlpnl.GetChild(0).transform.parent.GetChild(i).GetComponent<Image>().enabled = (0 == i);
        }

        PlayerPrefs.SetInt("ShuffleCount", -1);
        PlayerPrefs.SetInt("Pnl" + 2, 0);
        BtnController.instance.SetAddIcon(0, 2);
        PlayerPrefs.SetInt("Pnl" + 7, 0);
        BtnController.instance.SetAddIcon(0, 7);
    }

    public void SetCharacter(List<int> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            //Debug.Log(idx[i]);
            if (List[i] != -1)
            {
                Spots[i].enabled = true;
                List<Sprite> Alllists = AllLists[i];
                Spots[i].sprite = Alllists[List[i]];
            }
            else
            {
                Spots[i].enabled = false;
            }
        }
        Screenshot.instanse.isSaved = true;
        Spawner.instance.isedited = false;
        JsonContainer.instance.playerEntry.indexes = List;
    }

    public void ResetCharacter()
    {
        List<int> coll;
        int[] Collection = { 1, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        coll = Collection.ToList();
        //JsonContainer.instance.playerEntry.indexes = coll;
        SetCharacter(coll);
        BtnController.instance.resetPopupPnl.SetActive(false);
        BtnController.instance.doresetChar = false;
        Spawner.instance.isedited = false;
    }
}//class
