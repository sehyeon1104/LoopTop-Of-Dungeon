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
                //데미지 입기
                Debug.Log("ㅇㅇ");
                BossRangePattern.isAttackStart = false;
            }

        }


    }
}
