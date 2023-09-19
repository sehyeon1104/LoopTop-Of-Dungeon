using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    #region SkillBook_Val

    [SerializeField]
    private GameObject skillBookPanel = null;
    [SerializeField]
    private GameObject skillShufflePanel = null;
    public bool isSkillBookPanelActive { get; private set; } = false;
    public bool isSkillShufflePanelActive { get; private set; } = false;

    public int slotNum { get; private set; } = 0;
    private int price = 0;

    #endregion

    #region SkillShuffle_Val


    PlayerSkillInfo[] playerskillInfo;
    //public GameObject skillSelect = null;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //SkillSelectButtonInit();

        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
    }

    #region SkillBook_Func

    public void ToggleSkillBookPanel()
    {
        isSkillBookPanelActive = !skillBookPanel.activeSelf;
        skillBookPanel.SetActive(!skillBookPanel.activeSelf);
        MouseManager.Lock(!skillBookPanel.activeSelf);
        MouseManager.Show(skillBookPanel.activeSelf);
    }

    #endregion

    #region SkillShuffle_Func
    //public void SkillSelectButtonInit()
    //{
    //    for (int i = 0; i < skillSelect.transform.childCount; i++)
    //    {
    //        GameObject button = skillSelect.transform.GetChild(i).gameObject;
    //        button.GetComponent<Button>().onClick.AddListener(() => button.SetActive(false));
    //    }
    //}

    public void ToggleSkillShufflePanel()
    {
        isSkillShufflePanelActive = !skillShufflePanel.activeSelf;
        skillShufflePanel.SetActive(!skillShufflePanel.activeSelf);
        MouseManager.Lock(!skillShufflePanel.activeSelf);
        MouseManager.Show(skillShufflePanel.activeSelf);
    }
    #endregion
}
