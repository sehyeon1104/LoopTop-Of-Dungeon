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

        // TODO : ��� �������� ��������
        questionTmp.SetText($"[ {slotNum + 1} ] �� ������ ��ȭ�Ͻðڽ��ϱ�?");
        curSkillTmp.SetText($"{slotNum + 1}�� ���� ���� ��ų : {playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[slotNum]].skillName}");
        if(GameManager.Instance.Player.playerBase.SlotLevel[slotNum] == 5)
        {
            levelTmp.SetText($"���� ���� : {GameManager.Instance.Player.playerBase.SlotLevel[slotNum]} (MaxLevel)");
        }
        levelTmp.SetText($"���� ���� : {GameManager.Instance.Player.playerBase.SlotLevel[slotNum]}");


        amountTmp.SetText($"�ʿ� ��ȭ : ");
        // TODO : �ʿ� ��ȭ �� ���̺� ���� �� ����
        priceTmp.SetText($" {price}");
    }

    public void Enhance()
    {
        // TODO : ��ȭ ���� �λ�
        // TODO : ������ ������ ��ų ���� ����

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
