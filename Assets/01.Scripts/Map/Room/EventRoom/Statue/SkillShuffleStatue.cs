using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShuffleStatue : StatueBase
{
    private int randSkillNum1 = 0;
    private int randSkillNum2 = 0;

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
        if (collision.CompareTag("Player"))
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

    protected override void StatueFunc()
    {
        // TODO : 랜덤선택된 스킬 UI로 표기
        if (!isUseable)
            return;
        isUseable = false;

        randSkillNum1 = Random.Range(0, 5);
        randSkillNum2 = Random.Range(0, 5);

        if(randSkillNum1 == randSkillNum2)
        {
            while (randSkillNum1 == randSkillNum2)
                randSkillNum2 = Random.Range(0, 5);
        }

        GameManager.Instance.Player.playerBase.PlayerSkillNum[0] = randSkillNum1;
        GameManager.Instance.Player.playerBase.PlayerSkillNum[1] = randSkillNum2;
        PlayerSkill.Instance.SkillSelect(GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag);
        ToggleInteractiveTMP();
    }
}
