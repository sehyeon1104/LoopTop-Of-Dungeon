using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShuffle : MonoBehaviour
{
    [SerializeField]
    private SkillShuffleObj[] skillShuffleObjs = null;

    private HashSet<int> skillSelectNum = new HashSet<int>();

    private void OnEnable()
    {
        RandSkill();
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
}
