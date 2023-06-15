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

    #endregion

    #region SkillShuffle_Val

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
        cancleBtn.onClick.AddListener(CanCle);
    }

    #region SkillBook_Func

    public void ToggleSkillBookPanel()
    {
        skillBookPanel.SetActive(!skillBookPanel.activeSelf);
        if (skillBookPanel.activeSelf)
            UpdateSkillBookPanel();
    }

    public void UpdateSkillBookPanel()
    {
        // TODO : ��� �������� ��������
        questionTmp.SetText($"[ {1} ] �� ������ ��ȭ�Ͻðڽ��ϱ�?");
        curSkillTmp.SetText($"{1}�� ���� ���� ��ų : {GameManager.Instance.Player.playerBase.PlayerSkillNum[1]}");
        levelTmp.SetText($"���� ���� : {GameManager.Instance.Player.playerBase.SlotLevel[1]}");
        amountTmp.SetText($"�ʿ� ��ȭ : ");
        // TODO : �ʿ� ��ȭ �� ���̺� ���� �� ����
        priceTmp.SetText($"{1}");
    }

    public void Enhance()
    {
        // TODO : ��ȭ ���� �λ�
        // TODO : ������ ������ ��ų ���� ����
        // TODO : ������ ������ ��� ���� �Ұ�
        PlayerSkill.Instance.SlotUp(0);
        ToggleSkillBookPanel();
    }

    public void CanCle()
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
