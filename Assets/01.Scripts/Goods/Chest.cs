using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Define;

public class Chest : MonoBehaviour
{
    private ChestRating _chestRating = ChestRating.Common;

    private SpriteRenderer spriteRenderer = null;

    private Coroutine _coroutine = null;

    private GameObject chestSpawnEffect = null;

    private bool isOpen = false;

    [SerializeField]
    private Sprite[] chestSprite;

    // 최소 경험의 조각 획득량
    private int minFragmentAmount = 100;
    // 최대 경험의 조각 획득량
    private int maxFragmentAmount = 150;

    // hp 회복구슬 회복량
    private int hpRegenAmount = 2;

    private GameObject dropItemPrefab = null;

    private WaitForSeconds waitChestRemove = new WaitForSeconds(2f);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dropItemPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/DropItem.prefab");
        chestSpawnEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/ChestSpawnEffect.prefab");
    }

    private void Start()
    {
        Poolable spawnEffect = Managers.Pool.Pop(chestSpawnEffect);
        spawnEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
        spawnEffect.transform.localScale = new Vector3(2, 2, 2);
    }

    // 상자 등급 설정
    public void SetChestRating(ChestRating chestRating)
    {
        _chestRating = chestRating;
        UpdateChest();
    }

    // 상자 업데이트
    public void UpdateChest()
    {
        ChangeChestSprite();
        SetReward();
    }

    public void ChangeChestSprite()
    {
        spriteRenderer.sprite = chestSprite[(int)_chestRating - 1];
    }

    private void SetReward()
    {
        // (경험의조각 획득량 * 상자 등급( 1 ~ 4 ) ) / 2
        minFragmentAmount *= (int)_chestRating; //Mathf.RoundToInt((float)_chestRating * 0.5f + 0.5f);
        maxFragmentAmount *= (int)_chestRating; //Mathf.RoundToInt((float)_chestRating * 0.5f + 0.5f);

        // hp 회복 구슬 = 반올림(상자등급 / 2)
        hpRegenAmount = Mathf.RoundToInt((float)_chestRating * 0.5f);
    }

    public void Open()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (isOpen)
            return;

        isOpen = true;

        _coroutine = StartCoroutine(IEOpen());
    }

    public IEnumerator IEOpen()
    {
        // TODO :  상자 오픈 애니메이션 제작

        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, Random.Range(minFragmentAmount, maxFragmentAmount), 10);

        // TODO : hp회복구슬 구현

        SpawnItem();

        StartCoroutine(IEDestroyChest());
        yield return null;
    }

    public void SpawnItem()
    {
        var dropItemObj = Managers.Pool.Pop(dropItemPrefab, transform.position);
        dropItemObj.GetComponent<DropItem>().SetItem(_chestRating);
        dropItemObj.transform.DOJump((Random.insideUnitCircle * 0.75f) + (Vector2)transform.position, 1, 1, 0.4f);
    }

    // 상자 제거 모션
    public IEnumerator IEDestroyChest()
    {
        yield return waitChestRemove;

        spriteRenderer.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);

        transform.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIManager.Instance.RotateInteractionButton();
        UIManager.Instance.GetInteractionButton().onClick.RemoveListener(Open);
        UIManager.Instance.GetInteractionButton().onClick.AddListener(Open);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.Instance.GetInteractionButton().onClick.RemoveListener(Open);
        UIManager.Instance.RotateAttackButton();
    }
}
