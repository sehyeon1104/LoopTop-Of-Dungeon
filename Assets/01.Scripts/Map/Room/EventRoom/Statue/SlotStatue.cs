using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStatue : StatueBase
{

    PlayerSkillInfo[] playerskillInfo;
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

    int rand = 0;
    int randLevel = 0;
    int randSlot = 0;
    protected override void StatueFunc()
    {
        if (!isUseable)
            return;
        isUseable = false;

        rand = Random.Range(0, 100);

        if (rand >= 0 && rand < 20)
            randLevel = 1;
        else if (rand >= 20 && rand < 45)
            randLevel = 2;
        else if (rand >= 45 && rand < 65)
            randLevel = 3;
        else if (rand >= 65 && rand < 85)
            randLevel = 4;
        else if (rand >= 85 && rand < 100)
            randLevel = 5;

        randSlot = Random.Range(0, 1);

        Debug.Log($"스킬 레벨 변동 : {GameManager.Instance.Player.playerBase.SlotLevel[randSlot]} -> {randLevel}");
        // TODO : 스킬 레벨업 알림
        playerskillInfo = GameManager.Instance.Player.playerBase.PlayerTransformData.skill;
        effectTmp.SetText($"스킬 : {playerskillInfo[GameManager.Instance.Player.playerBase.PlayerSkillNum[randSlot]].skillName}\n레벨 변동 : {GameManager.Instance.Player.playerBase.SlotLevel[randSlot]} -> {randLevel}");

        GameManager.Instance.Player.playerBase.SlotLevel[randSlot] = randLevel;
        PlayerSkill.Instance.SkillSelect(GameManager.Instance.Player.playerBase.PlayerTransformTypeFlag);

        StartCoroutine(IETextAnim());
        ToggleInteractiveTMP();
    }
}
