using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ��ų ���� ����
public class SkillShufflMerchant : MerchantBase
{
    protected override void SetDialogueText()
    {
        dialogueText.Clear();
        dialogueText.Append("�������! ��ų ������ ��ȭ�ϰ� �����Ű���?");
    }

    protected override void InteractiveWithPlayer()
    {
        // DialogueManager.Instance.SetContentNPos(dialogueText.ToString(), gameObject);
    }

    protected override void MerchantFunc()
    {

    }

    public void SkillNum(List<int> skillList)
    {
        Button[] selectTexts = UIManager.Instance.shopUI.skillSelect.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < selectTexts.Length; i++)
        {
            selectTexts[i].GetComponentInChildren<TextMeshProUGUI>().text = skillList[i].ToString();
        }
    }
}
