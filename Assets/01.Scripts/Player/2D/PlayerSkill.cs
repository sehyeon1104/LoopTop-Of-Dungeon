using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Player Skill Class
public partial class Player
{
    // TODO : ��ų ��� ���� �ٲٱ�

    [Space]
    [Header("��ų")]
    [Header("��������")]
    [SerializeField]
    private GameObject ghostSummonerPrefab = null;
    [SerializeField] GameObject jangPanPrefab;
    public float jangPanTime =3;
    [SerializeField] Transform Skill1Trans;
    [SerializeField]
    float JangPanPersDamage = 10;
    private int ghostSummonCount = 1;

    // ��ų ��� ���� ����
    private bool[] coolTimes;

    public float skillCooltime { private set; get; } = 0f;

    public void InitCooltimeBools()
    {
        coolTimes = new bool[4];
    }

    public void Skill1()
    {
        if (coolTimes[1] == true || pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
            return;

        //if (!coolTimes[1] && pBase.PlayerTransformTypeFlag == Define.PlayerTransformTypeFlag.Ghost)
        JangPanPattern();

        Debug.Log("��ų 1");

        coolTimes[1] = true;
        skillCooltime = playerTransformDataSO.skill1Delay;

        StartCoroutine(ProgressCooltime(skillCooltime, 1));
    }

    public void HillaPattern()    // ���� ����
    {
        if (coolTimes[2] || pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
            return;
        Debug.Log("��ų 2");

        coolTimes[2] = true;
        skillCooltime = playerTransformDataSO.skill2Delay;

        playerAnim.SetTrigger("Attack");

        for (int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(ProgressCooltime(skillCooltime, 2));
    }

    public void JangPanPattern()
    {
        StartCoroutine(JangPanSkill(jangPanTime));

    }
    public void UltimateSkill()
    {
        if (coolTimes[3] || pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("�ñر�");

        coolTimes[3] = true;
        skillCooltime = playerTransformDataSO.ultiSkillDelay;

        StartCoroutine(ProgressCooltime(skillCooltime, 3));
    }

    // TODO : ���� �� �ٲٱ�.. �̤�

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
    IEnumerator JangPanSkill(float skillTime)
    {
        float timer = 0;
        float timerA = 0;
        Instantiate(jangPanPrefab, transform.position, Quaternion.identity,Skill1Trans);
        do
        {
           Collider2D[] attachObjs;
           
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if(timerA>1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, 2.5f);
                foreach(Collider2D c in attachObjs)
                {
                    if(c.CompareTag("Enemy") || c.CompareTag("Boss"))
                    {
                        c.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
                    }
                }
                timerA = 0;
                
            }
            if (timer>skillTime)
            {
                print("ss");
                Destroy(Skill1Trans.GetChild(0).gameObject);
            }
            yield return null;
        } while (timer < skillTime);


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}
