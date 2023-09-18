using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class SkillShuffle : MonoBehaviour
{
    [SerializeField]
    private int skillSlotNum;

    [SerializeField]
    private TextMeshProUGUI skillNameTMP = null;
    [SerializeField]
    private TextMeshProUGUI contentTMP = null;
    [SerializeField]
    private Button selectBtn = null;

    PlayerSkillInfo[] playerskillInfo;
    private SkillShuffle[] anotherSkillShuffleOption = null;

    [SerializeField]
    private Image skillIcon = null;    // ��ų ������
    [SerializeField]
    private GameObject content = null;  // ����
    [SerializeField]
    private Image panel = null;         // Fade�� �г�

    private RectTransform skillIconRectTransform = null;

    private bool isShow = false;

    private float waitDelay = 0.5f;     // �������� �����̴� ������
    private WaitForSeconds waitforWaitDelay;

    private Coroutine co = null;
    private Coroutine moveAndRegCo = null;

    private float inputDis = 60f;
    private float outputDis = 200f;

    private void Awake()
    {
        skillIconRectTransform = skillIcon.transform.parent.GetComponent<RectTransform>();
        waitforWaitDelay = new WaitForSeconds(waitDelay);

        content.SetActive(false);
        panel.gameObject.SetActive(false);
        panel.fillAmount = 1f;
    }

    private void OnEnable()
    {
        InitPos();
        RandSkill();
    }

    public void InitPos()
    {
        skillIconRectTransform.DOAnchorPosY(0f, 0f);
        skillIconRectTransform.DOScale(Vector3.one, 0f);
        content.SetActive(false);
        panel.gameObject.SetActive(false);
        panel.fillAmount = 1f;
    }

    private void Start()
    {
        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
        anotherSkillShuffleOption = transform.parent.GetComponentsInChildren<SkillShuffle>();
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
    /// ��ų ���� ����
    /// </summary>
    public void RandSkill()
    {
        // ��ų ��Ÿ���� �������� ��� X
        if (!UIManager.Instance.SkillCooltime(GameManager.Instance.Player.playerBase.PlayerTransformData, PlayerSkill.Instance.skillIndex[0], true) 
            || !UIManager.Instance.SkillCooltime(GameManager.Instance.Player.playerBase.PlayerTransformData, PlayerSkill.Instance.skillIndex[1], true))
            return;

        PlayerMovement.Instance.IsControl = false;
    }

    /// <summary>
    /// ���� ��ư�� �� �Լ�
    /// </summary>
    public void Select()
    {

    }

    /// <summary>
    /// ���콺�� �ö���� �� ���� �Լ�
    /// </summary>
    public IEnumerator ShowContent()
    {
        if (isShow)
            yield break;

        isShow = true;

        panel.gameObject.SetActive(true);
        if (moveAndRegCo != null)
        {
            StopCoroutine(moveAndRegCo);
            moveAndRegCo = null;
        }
        moveAndRegCo = StartCoroutine(MoveAndRegScaleSkillIcon(panel.gameObject.activeSelf));
        RegScaleAnotherSkillIcon(panel.gameObject.activeSelf);
        content.SetActive(true);
        yield return waitforWaitDelay;
        FadeOutPanel();
    }

    /// <summary>
    /// ���콺�� �ø��� �ʾ��� �� ���� �Լ�
    /// </summary>
    public IEnumerator HideContent()
    {
        if (!isShow)
            yield break;

        isShow = false;

        if(moveAndRegCo != null)
        {
            StopCoroutine(moveAndRegCo);
            moveAndRegCo = null;
        }

        panel.gameObject.SetActive(false);
        moveAndRegCo = StartCoroutine(MoveAndRegScaleSkillIcon(panel.gameObject.activeSelf));
        RegScaleAnotherSkillIcon(panel.gameObject.activeSelf);
        FadeInPanel();
        content.SetActive(false);
    }

    private IEnumerator MoveAndRegScaleSkillIcon(bool isActive)
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

        yield break;
    }

    public void ReduceSkillShuffleScale()
    {
        skillIconRectTransform.DOAnchorPosY(0f, waitDelay);
        skillIconRectTransform.DOScale(new Vector3(0.5f, 0.5f), waitDelay);
    }

    private void RegScaleAnotherSkillIcon(bool isActive)
    {
        for(int i = 0; i < anotherSkillShuffleOption.Length; ++i)
        {
            if (anotherSkillShuffleOption[i] == this)
                continue;

            if (isActive)
                anotherSkillShuffleOption[i].ReduceSkillShuffleScale();
            else
            {
                if(moveAndRegCo == null)
                    moveAndRegCo = StartCoroutine(anotherSkillShuffleOption[i].MoveAndRegScaleSkillIcon(false));
            }
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
