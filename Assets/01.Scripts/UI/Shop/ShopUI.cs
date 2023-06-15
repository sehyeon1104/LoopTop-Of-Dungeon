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
        // TODO : 몇번 슬롯인지 가져오기
        questionTmp.SetText($"[ {1} ] 번 슬롯을 강화하시겠습니까?");
        curSkillTmp.SetText($"{1}번 슬롯 장착 스킬 : {GameManager.Instance.Player.playerBase.PlayerSkillNum[1]}");
        levelTmp.SetText($"현재 레벨 : {GameManager.Instance.Player.playerBase.SlotLevel[1]}");
        amountTmp.SetText($"필요 재화 : ");
        // TODO : 필요 재화 수 테이블 제작 및 적용
        priceTmp.SetText($"{1}");
    }

    public void Enhance()
    {
        // TODO : 강화 가격 인상
        // TODO : 선택한 슬롯의 스킬 레벨 증가
        // TODO : 가격이 부족할 경우 구매 불가
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
