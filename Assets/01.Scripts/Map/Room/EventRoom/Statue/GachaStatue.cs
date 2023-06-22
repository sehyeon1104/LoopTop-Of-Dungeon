using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaStatue : StatueBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        button.onClick.RemoveListener(StatueFunc);
    }

    protected override void InteractiveWithPlayer()
    {
        base.InteractiveWithPlayer();
        button.onClick.RemoveListener(StatueFunc);
        button.onClick.AddListener(StatueFunc);
    }

    protected override void StandBy()
    {
        base.StandBy();
    }

    int rand = 0;
    protected override void StatueFunc()
    {
        // TODO : 33% 확률로 각각 재화획득, 아이템 획득, 데미지 입음
        if (!isUseable)
            return;
        isUseable = false;

        if (GameManager.Instance.Player.playerBase.Hp <= 2)
        {
            Debug.Log("체력 부족");
            // TODO : 체력 부족 UI 표시 및 사운드 실행
        }

        rand = Random.Range(0, 3);

        // 재화 획득
        if(rand == 0)
        {
            FragmentCollectManager.Instance.DropFragmentByCircle(gameObject, 8);
        }
        // 아이템 상자 드랍
        else if(rand == 1)
        {
            // TODO : 아이템 드롭 상자 드랍
        }
        // 데미지 입음
        else if(rand == 2)
        {
            GameManager.Instance.Player.OnDamage(2, 0);
        }
    }
}
