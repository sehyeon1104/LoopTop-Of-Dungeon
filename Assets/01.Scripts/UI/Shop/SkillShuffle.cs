using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShuffle : MonoBehaviour
{
    [SerializeField]
    private SkillShuffleObj[] skillShuffleObjs = null;

    private HashSet<int> skillSelectNum = new HashSet<int>();

    private int skillSlotNum = 0;

    private void OnEnable()
    {
        RandSkill();
    }

    /// <summary>
    /// 스킬 랜덤 선택
    /// </summary>
    public void RandSkill()
    {
        // 스킬 쿨타임이 돌고있을 경우 X
        if (!UIManager.Instance.SkillCooltime(GameManager.Instance.Player.playerBase.PlayerTransformData, PlayerSkill.Instance.skillIndex[0], true)
            || !UIManager.Instance.SkillCooltime(GameManager.Instance.Player.playerBase.PlayerTransformData, PlayerSkill.Instance.skillIndex[1], true))
            return;

        PlayerMovement.Instance.IsControl = false;

        int rand = 0;
        int skillCount = 0;
        PlayerSkillInfo[] playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
        skillSelectNum.Clear();

        while (skillCount < 3)
        {
            rand = Random.Range(1, 6);
            if (skillSelectNum.Contains(rand))
                continue;
            skillSelectNum.Add(rand);

            skillShuffleObjs[skillCount].SetValue(rand, playerskillInfo[rand].skillName, playerskillInfo[rand].skillExplanation, playerskillInfo[rand].skillIcon[0]);
            skillCount++;
        }
    }

    public void ApplySkillShuffleInPlayer(int _skillNum)
    {
        GameManager.Instance.Player.playerBase.PlayerSkillNum[skillSlotNum] = _skillNum;
        UIManager.Instance.UpdateUI();
        skillSlotNum++;
        CheckSkillChangeCount();
    }

    public void CheckSkillChangeCount()
    {
        if(skillSlotNum % 2== 0)
        {
            skillSlotNum = 0;
            PlayerMovement.Instance.IsControl = true;
            UIManager.Instance.shopUI.ToggleSkillShufflePanel();
            PlayerSkill.Instance.SkillSelect(GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag);
        }
    }
}
