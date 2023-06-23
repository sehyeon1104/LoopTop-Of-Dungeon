using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceStatue : StatueBase
{
    private string diceEffect = "";

    protected override void Start()
    {
        base.Start();

        effectTmp.gameObject.SetActive(false);
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

    // ȿ��
    int dice1 = 0;
    // ��ġ
    int dice2 = 0;
    protected override void StatueFunc()
    {
        // TODO : �ֻ��� 2���� ���� ȿ�� ȹ��. 1���ֻ��� : ȿ��, 2���ֻ��� : ��ġ
        if (!isUseable)
            return;
        isUseable = false;

        // TODO : �ֻ��� ������ �ִϸ��̼� �߰�
        dice1 = Random.Range(1, 7);

        dice2 = Random.Range(1, 7);

        // �ֻ��� ȿ�� UI ����
        DiceEffect(dice1, dice2);
    }

    private void DiceEffect(int dice1, int dice2)
    {
        switch (dice1)
        {
            case 1:
                DiceScale1(dice2);
                break;
            case 2:
                DiceScale2(dice2);
                break;
            case 3:
                DiceScale3(dice2);
                break;
            case 4:
                DiceScale4(dice2);
                break;
            case 5:
                DiceScale5(dice2);
                break;
            case 6:
                DiceScale6(dice2);
                break;
        }
        SetDiceEffectTmp(dice1, dice2);
    }

    // ��������
    private void DiceScale1(int dice2)
    {
        Debug.Log("������ 2 ����");
        dice2 = Mathf.RoundToInt(dice2 * 0.5f);

        GameManager.Instance.Player.OnDamage(dice2, 0);
    }

    // ���ݷ� ����
    private void DiceScale2(int dice2)
    {
        Debug.Log("���ݷ� ����");
        dice2 = Mathf.RoundToInt(dice2 * 0.5f);

        GameManager.Instance.Player.playerBase.Attack += dice2;
    }

    // ü�� ȸ��
    private void DiceScale3(int dice2)
    {
        Debug.Log("ü�� ȸ��");
        dice2 = Mathf.RoundToInt(dice2 * 0.5f);

        GameManager.Instance.Player.playerBase.Hp += dice2;
    }

    // ��ȭ ȹ��
    private void DiceScale4(int dice2)
    {
        Debug.Log("��ȭ ȹ��");
        dice2 = 200 + (dice2 * 10);
        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, dice2, 10);
    }

    // �̵��ӵ� ����
    private void DiceScale5(int dice2)
    {
        Debug.Log("�̵��ӵ� ����");
        float increaseAmount = (dice2 / 3) * 0.1f;

        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * increaseAmount;
    }

    // ��ȭ ����
    private void DiceScale6(int dice2)
    {
        Debug.Log("��ȭ ����");
        dice2 = 100 + (dice2 * 10);
        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, dice2, 10);
    }

    private void SetDiceEffectTmp(int diceScale1, int diceScale2)
    {
        switch (diceScale1)
        {
            case 1:
                diceEffect = "2�� ����";
                effectTmp.SetText($"�ֻ��� �� : {diceScale1}, {diceScale2}\n�ֻ��� ȿ�� : {diceEffect}");
                break;
            case 2:
                diceEffect = "���ݷ� ����";
                effectTmp.SetText($"�ֻ��� �� : {diceScale1}, {diceScale2}\n�ֻ��� ȿ�� : {diceEffect}");
                break;
            case 3:
                diceEffect = "ü�� ȸ��";
                effectTmp.SetText($"�ֻ��� �� : {diceScale1}, {diceScale2}\n�ֻ��� ȿ�� : {diceEffect}");
                break;
            case 4:
                diceEffect = "��ȭ ȹ��";
                effectTmp.SetText($"�ֻ��� �� : {diceScale1}, {diceScale2}\n�ֻ��� ȿ�� : {diceEffect}");
                break;
            case 5:
                diceEffect = "�̵��ӵ� ����";
                effectTmp.SetText($"�ֻ��� �� : {diceScale1}, {diceScale2}\n�ֻ��� ȿ�� : {diceEffect}");
                break;
            case 6:
                diceEffect = "��ȭ ����";
                effectTmp.SetText($"�ֻ��� �� : {diceScale1}, {diceScale2}\n�ֻ��� ȿ�� : {diceEffect}");
                break;
        }

        StartCoroutine(IETextAnim());
    }

}
