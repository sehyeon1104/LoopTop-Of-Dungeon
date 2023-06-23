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

    // 효과
    int dice1 = 0;
    // 수치
    int dice2 = 0;
    protected override void StatueFunc()
    {
        // TODO : 주사위 2개를 굴려 효과 획득. 1번주사위 : 효과, 2번주사위 : 수치
        if (!isUseable)
            return;
        isUseable = false;

        // TODO : 주사위 굴리는 애니메이션 추가
        dice1 = Random.Range(1, 7);

        dice2 = Random.Range(1, 7);

        // 주사위 효과 UI 띄우기
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

    // 피해입음
    private void DiceScale1(int dice2)
    {
        Debug.Log("데미지 2 입음");
        dice2 = Mathf.RoundToInt(dice2 * 0.5f);

        GameManager.Instance.Player.OnDamage(dice2, 0);
    }

    // 공격력 증가
    private void DiceScale2(int dice2)
    {
        Debug.Log("공격력 증가");
        dice2 = Mathf.RoundToInt(dice2 * 0.5f);

        GameManager.Instance.Player.playerBase.Attack += dice2;
    }

    // 체력 회복
    private void DiceScale3(int dice2)
    {
        Debug.Log("체력 회복");
        dice2 = Mathf.RoundToInt(dice2 * 0.5f);

        GameManager.Instance.Player.playerBase.Hp += dice2;
    }

    // 재화 획득
    private void DiceScale4(int dice2)
    {
        Debug.Log("재화 획득");
        dice2 = 200 + (dice2 * 10);
        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, dice2, 10);
    }

    // 이동속도 증가
    private void DiceScale5(int dice2)
    {
        Debug.Log("이동속도 증가");
        float increaseAmount = (dice2 / 3) * 0.1f;

        GameManager.Instance.Player.playerBase.MoveSpeed += GameManager.Instance.Player.playerBase.InitMoveSpeed * increaseAmount;
    }

    // 재화 감소
    private void DiceScale6(int dice2)
    {
        Debug.Log("재화 감소");
        dice2 = 100 + (dice2 * 10);
        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, dice2, 10);
    }

    private void SetDiceEffectTmp(int diceScale1, int diceScale2)
    {
        switch (diceScale1)
        {
            case 1:
                diceEffect = "2의 피해";
                effectTmp.SetText($"주사위 눈 : {diceScale1}, {diceScale2}\n주사위 효과 : {diceEffect}");
                break;
            case 2:
                diceEffect = "공격력 증가";
                effectTmp.SetText($"주사위 눈 : {diceScale1}, {diceScale2}\n주사위 효과 : {diceEffect}");
                break;
            case 3:
                diceEffect = "체력 회복";
                effectTmp.SetText($"주사위 눈 : {diceScale1}, {diceScale2}\n주사위 효과 : {diceEffect}");
                break;
            case 4:
                diceEffect = "재화 획득";
                effectTmp.SetText($"주사위 눈 : {diceScale1}, {diceScale2}\n주사위 효과 : {diceEffect}");
                break;
            case 5:
                diceEffect = "이동속도 증가";
                effectTmp.SetText($"주사위 눈 : {diceScale1}, {diceScale2}\n주사위 효과 : {diceEffect}");
                break;
            case 6:
                diceEffect = "재화 감소";
                effectTmp.SetText($"주사위 눈 : {diceScale1}, {diceScale2}\n주사위 효과 : {diceEffect}");
                break;
        }

        StartCoroutine(IETextAnim());
    }

}
