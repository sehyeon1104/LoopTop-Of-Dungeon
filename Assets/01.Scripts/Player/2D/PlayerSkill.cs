using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Player Skill Class
public partial class Player
{
    [Space]
    [Header("스킬")]
    [Header("힐라패턴")]
    [SerializeField]
    private GameObject ghostSummonerPrefab = null;
    [SerializeField] GameObject jangPanPrefab;
    [Tooltip("장판 지속시간")]
    public float jangPanTime =3;
    [Tooltip("할퀴기 지속시간")]
    public float scratchTime = 3;
    [SerializeField] Transform Skill1Trans;
    [SerializeField]
    float JangPanPersDamage = 10;
    private int ghostSummonCount = 1;

    public float skillCooltime { private set; get; } = 0f;

    public void Skill1()
    {
        if (isPDead)
            return;

        JangPanPattern();

        Debug.Log("1번 스킬");

        skillCooltime = playerTransformDataSO.skill1Delay;
    }

    public void Skill2()
    {
        if (isPDead)
            return;

        Debug.Log("2번 스킬");

        HillaPattern();
    }
    public void TellportParrern() //텔레포트 패턴
    {
        StartCoroutine(TeleportSkill(scratchTime));
    }
    public void HillaPattern()    // 힐라 패턴
    {
        if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
            return;

        skillCooltime = playerTransformDataSO.skill2Delay;

        playerAnim.SetTrigger("Attack");

        for (int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }
    }

    public void JangPanPattern()
    {
        if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
            return;

        StartCoroutine(JangPanSkill(jangPanTime));
    }

    public void UltimateSkill()
    {
        if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("궁극기");

        skillCooltime = playerTransformDataSO.ultiSkillDelay;

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
            if(timerA>0.1f)
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
                Destroy(Skill1Trans.GetChild(0).gameObject);
            }
            yield return null;
        } while (timer < skillTime);

    }
    IEnumerator TeleportSkill(float skillTime)
    {
        float timer = 0;
        float timerA = 0;
        do
        {
            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.05f)
            {
                RaycastHit2D[] enemys = Physics2D.BoxCastAll(transform.position, new Vector2(2, 2), 0, Vector2.up, 2);
                foreach (RaycastHit2D c in enemys)
                {
                    if (c.collider.CompareTag("Enemy") || c.collider.CompareTag("Boss"))
                    {
                        c.collider.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
                    }
                }
                timerA = 0;
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
