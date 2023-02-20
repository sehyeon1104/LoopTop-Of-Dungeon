using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Player Skill Class
public partial class Player
{
    [Space]
    [Header("½ºÅ³")]
    [Header("Èú¶óÆÐÅÏ")]
    [SerializeField]
    private GameObject ghostSummonerPrefab = null;
    [SerializeField] GameObject jangPanPrefab;
    public float jangPanTime =3;
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

        Debug.Log("1¹ø ½ºÅ³");

        skillCooltime = playerTransformDataSO.skill1Delay;
    }

    public void Skill2()
    {
        if (isPDead)
            return;

        Debug.Log("2¹ø ½ºÅ³");

        HillaPattern();
    }

    public void HillaPattern()    // Èú¶ó ÆÐÅÏ
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
        Debug.Log("±Ã±Ø±â");

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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}
