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

    [SerializeField]
    private Sprite[] chestSprite;

    // �ּ� ������ ���� ȹ�淮
    private int minFragmentAmount = 100;
    // �ִ� ������ ���� ȹ�淮
    private int maxFragmentAmount = 150;

    private int hpRegenAmount = 2;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        chestSpawnEffect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/ChestSpawnEffect.prefab");
        Poolable spawnEffect = Managers.Pool.Pop(chestSpawnEffect);
        spawnEffect.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f);
        spawnEffect.transform.localScale = new Vector3(2, 2, 2);
        UpdateChest();
    }

    // ���� ��� ����
    public void SetChestRating(ChestRating chestRating)
    {
        _chestRating = chestRating;
        UpdateChest();
    }

    // ���� ������Ʈ
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
        // (���������� ȹ�淮 * ���� ���( 1 ~ 4 ) ) / 2
        minFragmentAmount *= (int)((float)_chestRating * 0.5f);
        maxFragmentAmount *= (int)((float)_chestRating * 0.5f);

        // hp ȸ�� ���� = �ݿø�(���ڵ�� / 2)
        hpRegenAmount = Mathf.RoundToInt((float)_chestRating * 0.5f);
    }

    public void Open()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(IEOpen());
    }

    public IEnumerator IEOpen()
    {
        // TODO :  ���� ���� �ִϸ��̼� ����

        FragmentCollectManager.Instance.DropFragmentByCircle(GameManager.Instance.Player.gameObject, Random.Range(minFragmentAmount, maxFragmentAmount), 10);
        // TODO : hpȸ������ ����

        yield return null;
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
