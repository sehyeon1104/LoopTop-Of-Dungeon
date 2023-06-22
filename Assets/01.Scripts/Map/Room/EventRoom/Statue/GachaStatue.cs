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
        // TODO : 33% Ȯ���� ���� ��ȭȹ��, ������ ȹ��, ������ ����
        if (!isUseable)
            return;
        isUseable = false;

        if (GameManager.Instance.Player.playerBase.Hp <= 2)
        {
            Debug.Log("ü�� ����");
            // TODO : ü�� ���� UI ǥ�� �� ���� ����
        }

        rand = Random.Range(0, 3);

        // ��ȭ ȹ��
        if(rand == 0)
        {
            FragmentCollectManager.Instance.DropFragmentByCircle(gameObject, 8);
        }
        // ������ ���� ���
        else if(rand == 1)
        {
            // TODO : ������ ��� ���� ���
        }
        // ������ ����
        else if(rand == 2)
        {
            GameManager.Instance.Player.OnDamage(2, 0);
        }
    }
}
