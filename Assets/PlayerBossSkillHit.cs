using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBossSkillHit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (BossRangePattern.isAttackStart == true)
        {
            if (collision.transform.CompareTag("BossSkill"))
            {
                //������ �Ա�
                Debug.Log("����");
                BossRangePattern.isAttackStart = false;
            }

        }


    }
}
