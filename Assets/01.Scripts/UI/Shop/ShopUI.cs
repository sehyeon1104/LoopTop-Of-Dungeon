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

    public int slotNum { get; private set; } = 0;
    private int price = 0;

    #endregion

    #region SkillShuffle_Val


    PlayerSkillInfo[] playerskillInfo;
    public GameObject skillSelect = null;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SkillSelectButtonInit();

        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
    }

    #region SkillBook_Func

    public void ToggleSkillBookPanel()
    {
        skillBookPanel.SetActive(!skillBookPanel.activeSelf);
        MouseManager.Lock(!skillBookPanel.activeSelf);
        MouseManager.Show(skillBookPanel.activeSelf);
    }

    public void Enhance()
    {
        // TODO : 강화 가격 인상
        // TODO : 선택한 슬롯의 스킬 레벨 증가

        if (GameManager.Instance.Player.playerBase.FragmentAmount < price
            || GameManager.Instance.Player.playerBase.SlotLevel[UIManager.Instance.shopUI.slotNum] == 5)
        {
            Debug.Log("최대 강화");
            return;
        }

        PlayerSkill.Instance.SlotUp(slotNum);
        ToggleSkillBookPanel();
    }

    public void Cancle()
    {
        ToggleSkillBookPanel();
    }

    #endregion

    #region SkillShuffle_Func
    public void SkillSelectButtonInit()
    {
        for (int i = 0; i < skillSelect.transform.childCount; i++)
        {
            GameObject button = skillSelect.transform.GetChild(i).gameObject;
            button.GetComponent<Button>().onClick.AddListener(() => button.SetActive(false));
        }
    }
    #endregion
}
