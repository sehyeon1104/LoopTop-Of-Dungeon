using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Player Skill Class
public partial class Player
{

    [Space]
    [Header("��ų")]
    [Header("��������")]
    [SerializeField]
    private GameObject ghostSummonerPrefab = null;
    [SerializeField] GameObject jangPanPrefab;
    [Tooltip("���� ���ӽð�")]
    public float jangPanTime = 3;
    [Tooltip("������ ���ӽð�")]
    public float scratchTime = 3;
    [SerializeField] Transform Skill1Trans;
    [SerializeField]
    float JangPanPersDamage = 10;
    private int ghostSummonCount = 1;
    List<int> randomSkillNum = new List<int>();
    public float skillCooltime { private set; get; } = 0f;
    public void ListInit()
    {
        randomSkillNum.Clear();
        for (int i = 1; i < 6; i++)
        {
            randomSkillNum.Add(i);
        }

    }
    public void ListShuffle()
    {
        for (int i = 0; i < 100; i++)
        {
           int randomAt = Random.Range(0,randomSkillNum.Count-1);
            int newNum = Random.Range(1, randomSkillNum.Count);
            int currentNum = randomSkillNum[randomAt];
        }
    }
    public void Skill1()
    {
        if (isPDead)
            return;

        switch (pBase.PlayerTransformTypeFlag)
        {

        }
        HillaSkill();

        Debug.Log("1�� ��ų");


    }

    public void Skill2()
    {
        if (isPDead)
            return;

        Debug.Log("2�� ��ų");

        JangPanSkill();
    }
    public void SkillShuffle()
    {
        ListInit();

        for (int i = 0; i < 2; i++)
        {

        }
    }
    #region ��Ʈ ��ų
    public void HillaSkill()  //1�� ��ų ���� ��ų
    {
        if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
            return;

        skillCooltime = playerTransformDataSO.skill[1].skillDelay;

        playerAnim.SetTrigger("Attack");

        for (int i = 0; i < ghostSummonCount; ++i)
        {
            Instantiate(ghostSummonerPrefab, transform.position, Quaternion.identity);
        }
    }

    public void JangPanSkill() //2�� ��ų ���� ��ų �ִϸ��̼� �ʿ�
    {
        if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost || isPDead)
            return;

        StartCoroutine(JangPanSkill(jangPanTime));
        skillCooltime = playerTransformDataSO.skill[0].skillDelay;
    }
    public void TellportParrern() //3�� ��ų �ڷ���Ʈ ����
    {
        StartCoroutine(TeleportSkill(scratchTime));
    }
    public void ArmStretchSkill() // �� �o�� ��ų 
    {

    }
    public void RiseUpSkill() //�ھ� ������ ��ų
    {

    }
    public void UltimateSkill()
    {
        if (pBase.PlayerTransformTypeFlag != Define.PlayerTransformTypeFlag.Ghost)
            return;
        Debug.Log("�ñر�");

        skillCooltime = playerTransformDataSO.ultiSkillDelay;

    }

    IEnumerator JangPanSkill(float skillTime)
    {
        float timer = 0;
        float timerA = 0;
        Instantiate(jangPanPrefab, transform.position, Quaternion.identity, Skill1Trans);
        do
        {
            Collider2D[] attachObjs;

            timer += Time.deltaTime;
            timerA += Time.deltaTime;
            if (timerA > 0.1f)
            {
                attachObjs = Physics2D.OverlapCircleAll(transform.position, 2.5f);
                foreach (Collider2D c in attachObjs)
                {
                    if (c.CompareTag("Enemy") || c.CompareTag("Boss"))
                    {
                        c.GetComponent<IHittable>().OnDamage(1, gameObject, 0);
                    }
                }
                timerA = 0;

            }
            if (timer > skillTime)
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
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
}
