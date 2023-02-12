using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Skill Class
public partial class Player
{
    [Space]
    [Header("��ų")]
    [Header("��������")]
    [SerializeField]
    private GameObject ghostSummonerPrefab = null;
    [SerializeField]
    private int ghostSummonCount = 1;

    public float skillCooltime { private set; get; } = 0f;

    public void Skill1()
    {
        skillCooltime = playerTransformDataSO.skill1Delay;


    }

    public void HillaPattern()    // ���� ����
    {
        skillCooltime = playerTransformDataSO.skill2Delay;

        playerAnim.SetTrigger("Attack");

        for(int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }
    }

    public void UltimateSkill()
    {
        skillCooltime = playerTransformDataSO.ultiSkillDelay;


    }

}
