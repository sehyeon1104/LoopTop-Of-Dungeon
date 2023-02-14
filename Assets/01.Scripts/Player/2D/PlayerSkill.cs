using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Skill Class
public partial class Player
{
    // TODO : 스킬 사용 구조 바꾸기

    [Space]
    [Header("스킬")]
    [Header("힐라패턴")]
    [SerializeField]
    private GameObject ghostSummonerPrefab = null;
    [SerializeField]
    private int ghostSummonCount = 1;

    // 스킬 사용 가능 여부
    private bool[] coolTimes;

    public float skillCooltime { private set; get; } = 0f;

    public void InitCooltimeBools()
    {
        coolTimes = new bool[4];
    }

    public void Skill1()
    {
        if (coolTimes[1] || pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("스킬 1");

        coolTimes[1] = true;
        skillCooltime = playerTransformDataSO.skill1Delay;

        StartCoroutine(ProgressCooltime(skillCooltime, 1));
    }

    public void HillaPattern()    // 힐라 패턴
    {
        if (coolTimes[2] || pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("스킬 2");

        coolTimes[2] = true;
        skillCooltime = playerTransformDataSO.skill2Delay;

        playerAnim.SetTrigger("Attack");

        for(int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(ProgressCooltime(skillCooltime, 2));
    }

    public void UltimateSkill()
    {
        if (coolTimes[3] || pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("궁극기");

        coolTimes[3] = true;
        skillCooltime = playerTransformDataSO.ultiSkillDelay;

        StartCoroutine(ProgressCooltime(skillCooltime, 3));
    }

    // TODO : 구조 꼭 바꾸기.. ㅜㅜ
    
    private IEnumerator ProgressCooltime(float cooltime, int skillNum)
    {
        while (cooltime > 0f)
        {
            cooltime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        coolTimes[skillNum] = false;
        yield break;
    }

}
