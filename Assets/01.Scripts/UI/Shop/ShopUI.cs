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
    private TextMeshProUGUI questionTmp = null;
    [SerializeField]
    private TextMeshProUGUI curSkillTmp = null;
    [SerializeField]
    private TextMeshProUGUI levelTmp = null;
    [SerializeField]
    private TextMeshProUGUI amountTmp = null;
    [SerializeField]
    private TextMeshProUGUI priceTmp = null;
    [SerializeField]
    private Button purchaseBtn = null;
    [SerializeField]
    private Button cancleBtn = null;

    private int slotNum = 0;
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
        purchaseBtn.onClick.RemoveAllListeners();
        purchaseBtn.onClick.AddListener(Enhance);

        cancleBtn.onClick.RemoveAllListeners();
        cancleBtn.onClick.AddListener(Cancle);

        SkillSelectButtonInit();

        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
    }

    #region SkillBook_Func

    public void ToggleSkillBookPanel(int slotNum)
    {
        this.slotNum = slotNum;
        skillBookPanel.SetActive(!skillBookPanel.activeSelf);
        MouseManager.Lock(!skillBookPanel.activeSelf);
        MouseManager.Show(skillBookPanel.activeSelf);
        if (skillBookPanel.activeSelf)
            UpdateSkillBookPanel();
    }

    public void ToggleSkillBookPanel(int slotNum, bool isActive)
    {
        this.slotNum = slotNum;
        skillBookPanel.SetActive(isActive);
        MouseManager.Lock(!isActive);
        MouseManager.Show(isActive);
        if (skillBookPanel.activeSelf)
            UpdateSkillBookPanel();
    }

    public void UpdateSkillBookPanel()
    {

        // TODO : 몇번 슬롯인지 가져오기
        questionTmp.SetText($"[ {slotNum + 1} ] 번 슬롯을 강화하시겠습니까?");
        curSkillTmp.SetText($"{slotNum + 1}번 슬롯 장착 스킬 : {playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[slotNum]].skillName}");
        if(GameManager.Instance.Player.playerBase.SlotLevel[slotNum] == 5)
        {
            levelTmp.SetText($"현재 레벨 : {GameManager.Instance.Player.playerBase.SlotLevel[slotNum]} (MaxLevel)");
        }
        levelTmp.SetText($"현재 레벨 : {GameManager.Instance.Player.playerBase.SlotLevel[slotNum]}");


        amountTmp.SetText($"필요 재화 : ");
        // TODO : 필요 재화 수 테이블 제작 및 적용
        priceTmp.SetText($" {price}");
    }

    public void Enhance()
    {
        // TODO : 강화 가격 인상
        // TODO : 선택한 슬롯의 스킬 레벨 증가

        if (GameManager.Instance.Player.playerBase.FragmentAmount < price
            || GameManager.Instance.Player.playerBase.SlotLevel[1] == 5)
        {
            return;
        }

        PlayerSkill.Instance.SlotUp(slotNum);
        ToggleSkillBookPanel(slotNum);
    }

    public void Cancle()
    {
        ToggleSkillBookPanel(slotNum);
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
