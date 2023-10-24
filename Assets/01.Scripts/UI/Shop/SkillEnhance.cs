using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SkillEnhance : MonoBehaviour
{
    [SerializeField]
    private int skillSlotNum;

    [SerializeField]
    private TextMeshProUGUI skillNameTMP = null;
    [SerializeField]
    private TextMeshProUGUI contentTMP = null;
    [SerializeField]
    private TextMeshProUGUI priceTMP = null;
    [SerializeField]
    private TextMeshProUGUI levelTMP = null;
    [SerializeField]
    private Button enhanceBtn = null;

    private int price = 0;

    PlayerSkillInfo[] playerskillInfo;

    [SerializeField]
    private Image skillIcon = null;    // 스킬 아이콘
    [SerializeField]
    private GameObject content = null;  // 내용
    [SerializeField]
    private Image panel = null;         // Fade용 패널

    private RectTransform skillIconRectTransform = null;

    private bool isShow = false;

    private float waitDelay = 0.5f;     // 아이콘이 움직이는 딜레이
    private WaitForSeconds waitforWaitDelay;

    private Coroutine co = null;

    private float inputDis = 100f;
    private float outputDis = 250f;

    private void Awake()
    {
        skillIconRectTransform = skillIcon.transform.parent.GetComponent<RectTransform>();
        waitforWaitDelay = new WaitForSeconds(waitDelay);

        content.SetActive(false);
        panel.gameObject.SetActive(false);
        panel.fillAmount = 1f;

        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
    }

    private void OnEnable()
    {
        InitPos();
        UpdateValue();
    }

    public void InitPos()
    {
        skillIconRectTransform.DOAnchorPosY(0f, 0f);
        skillIconRectTransform.DOScale(Vector3.one, 0f);
        content.SetActive(false);
        panel.gameObject.SetActive(false);
        panel.fillAmount = 1f;
    }

    private void Update()
    {
        if (Vector2.Distance(Input.mousePosition, transform.position) < inputDis && !isShow)
        {
            CheckCoroutineNull();
            co = StartCoroutine(ShowContent());
        }
        else if (Vector2.Distance(Input.mousePosition, transform.position) > outputDis && isShow)
        {
            CheckCoroutineNull();
            co = StartCoroutine(HideContent());
        }
    }

    private void CheckCoroutineNull()
    {
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    /// <summary>
    /// 내용 업데이트
    /// </summary>
    public void UpdateValue()
    {
        Debug.Log(GameManager.Instance.Player.playerBase.PlayerTransformData);
        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;

        // 스킬 아이콘 업데이트
        if (GameManager.Instance.Player.playerBase.SlotLevel[skillSlotNum] == 5)
            skillIcon.sprite = playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[skillSlotNum]].skillIcon[1];
        else
            skillIcon.sprite = playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[skillSlotNum]].skillIcon[0];

        // 스킬 이름
        skillNameTMP.SetText($"{playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[skillSlotNum]].skillName}");

        // 스킬 설명
        contentTMP.SetText($"{playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[skillSlotNum]].skillExplanation}");

        // 스킬 레벨
        if (GameManager.Instance.Player.playerBase.SlotLevel[skillSlotNum] == 5)
            levelTMP.SetText($"Level : {GameManager.Instance.Player.playerBase.SlotLevel[skillSlotNum]} (MaxLevel)");

        levelTMP.SetText($"Level : {GameManager.Instance.Player.playerBase.SlotLevel[skillSlotNum]}");

        // 스킬 강화 가격
        priceTMP.SetText($"필요 재화 : {price}");
        // TODO : 필요 재화 수 테이블 제작 및 적용
    }

    /// <summary>
    /// 강화 버튼에 들어갈 함수
    /// </summary>
    public void Enhance()
    {
        Debug.Log("강화");
        // TODO : 강화 가격 인상
        if (GameManager.Instance.Player.playerBase.FragmentAmount < price
            || GameManager.Instance.Player.playerBase.SlotLevel[skillSlotNum] == 5)
        {
            Debug.Log("최대 강화");
            return;
        }

        PlayerSkill.Instance.SlotUp(skillSlotNum);
        UpdateValue();
    }

    /// <summary>
    /// 마우스가 올라왔을 때 사용될 함수
    /// </summary>
    public IEnumerator ShowContent()
    {
        if (isShow)
            yield break;

        isShow = true;

        panel.gameObject.SetActive(true);
        MoveAndRegScaleSkillIcon(panel.gameObject.activeSelf);
        content.SetActive(true);
        yield return waitforWaitDelay;
        FadeOutPanel();
    }

    /// <summary>
    /// 마우스를 올리지 않았을 때 사용될 함수
    /// </summary>
    public IEnumerator HideContent()
    {
        if (!isShow)
            yield break;

        isShow = false;

        panel.gameObject.SetActive(false);
        MoveAndRegScaleSkillIcon(panel.gameObject.activeSelf);
        FadeInPanel();
        content.SetActive(false);
    }

    private void MoveAndRegScaleSkillIcon(bool isActive)
    {
        if (isActive)
        {
            skillIconRectTransform.DOAnchorPosY(250f, waitDelay);
            skillIconRectTransform.DOScale(new Vector3(0.8f, 0.8f), waitDelay);
        }
        else
        {
            skillIconRectTransform.DOAnchorPosY(0f, waitDelay);
            skillIconRectTransform.DOScale(Vector3.one, waitDelay);
        }
    }

    private void FadeInPanel()
    {
        panel.DOFillAmount(1f, waitDelay);
    }
    private void FadeOutPanel()
    {
        panel.DOFillAmount(0f, waitDelay);
    }
}
