using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerStatue : StatueBase
{
    private GameObject chestPreview = null;
    private SpriteRenderer chestSpriteRenderer = null;

    private int prayCount = 0;

    private bool isCool = false;
    private float curDelay = 0f;
    private float prayDelay = 0.5f;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private Coroutine textCoroutine = null;

    protected override void Start()
    {
        base.Start();
        chestPreview = transform.Find("ChestPreview").gameObject;
        chestSpriteRenderer = chestPreview.GetComponent<SpriteRenderer>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        button.onClick.RemoveListener(StatueFunc);
        TakeChest();
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
        if (!isUseable)
            return;

        if (GameManager.Instance.Player.playerBase.Hp < 2)
            return;

        if (prayCount >= 4)
            return;

        if (isCool)
            return;

        prayCount++;
        isCool = true;

        effectTmp.text = "신께 기도합니다..";
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
        }

        textCoroutine = StartCoroutine(IETextAnim());

        GameManager.Instance.Player.OnDamage(1, 0);
        chestSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"Assets/04.Sprites/chests.png[chests_{prayCount - 1}]");
        StartCoroutine(IECooltime());
    }

    private IEnumerator IECooltime()
    {
        curDelay = 0f;

        while (curDelay < prayDelay)
        {
            curDelay += Time.deltaTime;

            yield return waitForEndOfFrame;
        }

        isCool = false;
    }

    public void TakeChest()
    {
        if (prayCount == 0 || !isUseable)
            return;

        isUseable = false;
        GameObject chestObj = Managers.Resource.Instantiate("Assets/03.Prefabs/Chest.prefab");
        Chest chest = chestObj.GetComponent<Chest>();
        chest.SetChestRating((Define.ChestRating)prayCount);
        chest.transform.position = chestPreview.transform.position;
        chestPreview.gameObject.SetActive(false);
    }
}
