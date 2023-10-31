using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDonationStatue : StatueBase
{
    private int requireHp = 5;
    private Coroutine co = null;

    [SerializeField]
    private GameObject visual = null;
    [SerializeField]
    private Sprite[] spriteImage = null;
    private SpriteRenderer spriteRenderer = null;

    private int count = 0;

    protected override void Start()
    {
        base.Start();
        dialogueText = "����� ���� ���̸� ������ ��ǰ�� �����ص帳�ϴ�!";
        acceptText = $"{requireHp}�����ϱ�";
        spriteRenderer = visual.GetComponent<SpriteRenderer>();
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

        if (co != null)
            StopCoroutine(co);

        co = null;

        if (requireHp > GameManager.Instance.Player.playerBase.Hp)
        {
            effectTmp.SetText($"ü���� �����մϴ�! �䱸ġ : {requireHp}");
            co = StartCoroutine(IETextAnim());
            return;
        }

        count++;
        spriteRenderer.sprite = spriteImage[count];

        GameManager.Instance.Player.playerBase.Hp -= requireHp;
        if(Random.Range(0, 100) < requireHp * 4)
        {
            var dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
            dropItemObj.transform.position = transform.position;
            dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Rare);
            dropItemObj.transform.DOJump(new Vector3(transform.position.x, transform.position.y - 3f), 1, 1, 0.4f);
            isUseable = false;
        }

        effectTmp.SetText($"ü�� {requireHp}����");
        co = StartCoroutine(IETextAnim());

        requireHp *= 2;

        acceptText = $"{requireHp}�����ϱ�";

        ToggleInteractiveTMP();
    }
}
