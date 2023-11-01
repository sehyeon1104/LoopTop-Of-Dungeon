using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFountainStatue : StatueBase
{

    protected override void Start()
    {
        base.Start();

        dialogueText = $"���ٷ� ���� �� ���ۿ��� ���� �� ������ �ʿ��� �и� ����Ͻñ� �ٶ��ϴ�.";
        acceptText = $"�����ϱ�";
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (collision.CompareTag("Player"))
            button.onClick.RemoveListener(StatueFunc);
    }

    protected override void InteractiveWithPlayer()
    {
        base.InteractiveWithPlayer();
        button.onClick.RemoveListener(StatueFunc);
        button.onClick.AddListener(StatueFunc);
    }

    protected override void StatueFunc()
    {
        if (!isUseable)
            return;

        if (GameManager.Instance.Player.playerBase.Hp <= GameManager.Instance.Player.playerBase.MaxHp * 0.6f)
        {
            GameManager.Instance.Player.playerBase.Hp = GameManager.Instance.Player.playerBase.MaxHp;
            effectTmp.SetText("ü�� ȸ��!");
        }
        else
        {
            GameManager.Instance.Player.playerBase.MaxHp -= 15;

            var dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
            dropItemObj.transform.position = transform.position;
            dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Special);
            dropItemObj.transform.DOJump(new Vector3(transform.position.x, transform.position.y - 3f), 1, 1, 0.4f);
            effectTmp.SetText("�ִ� ü�� 15 ����");
        }

        StartCoroutine(IETextAnim());
        isUseable = false;
        ToggleInteractiveTMP();
    }
}
