using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ��ų ���� ����
public class SkillShuffleMerchant : MerchantBase
{
    Button button;
    private void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
    }

    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        dialogueText.Append("�������! ��ų ������ ��ȭ�ϰ� �����Ű���?");
    }

    protected override void InteractiveWithPlayer()
    {
        base.InteractiveWithPlayer();
        isInteractive = true;

        if(GameManager.Instance.sceneType == Define.Scene.Field)
            StageManager.Instance.shop.isInteractive = isInteractive;
        UIManager.Instance.RotateInteractionButton();
        button.onClick.RemoveListener(MerchantFunc);
        button.onClick.AddListener(MerchantFunc);

        // DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
    }

    protected override void MerchantFunc()
    {
        UIManager.Instance.shopUI.ToggleSkillShufflePanel();
        interactionTMP.gameObject.SetActive(false);
        //StartCoroutine(PlayerSkill.Instance.SkillShuffle());
    }

    public void SkillNum(List<int> skillList)
    {
        //Button[] selectTexts = UIManager.Instance.shopUI.skillSelect.GetComponentsInChildren<Button>(true);
        //for (int i = 0; i < selectTexts.Length; i++)
        //{
        //    selectTexts[i].GetComponentInChildren<TextMeshProUGUI>().text = skillList[i].ToString();
        //}
    }

    protected override void StandBy()
    {
        if (!isInteractive)
            return;

        isInteractive = false;
        if (GameManager.Instance.sceneType == Define.Scene.Field)
            StageManager.Instance.shop.isInteractive = isInteractive;
        UIManager.Instance.RotateAttackButton();
        button.onClick.RemoveListener(MerchantFunc);
    }
}
