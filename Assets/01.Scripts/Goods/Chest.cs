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

    [SerializeField]
    private Sprite[] chestSprite;

    // �ּ� ������ ���� ȹ�淮
    private float minFragmentAmount = 100;
    // �ִ� ������ ���� ȹ�淮
    private float maxFragmentAmount = 150;

    private int hpRegenAmount = 2;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
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

    public void Open()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(IEOpen());
    }

    private void SetReward()
    {
        // (���������� ȹ�淮 * ���� ���( 1 ~ 4 ) ) / 2
        minFragmentAmount *= (float)_chestRating * 0.5f;
        maxFragmentAmount *= (float)_chestRating * 0.5f;

        // hp ȸ�� ���� = �ݿø�(���ڵ�� / 2)
        hpRegenAmount = Mathf.RoundToInt((float)_chestRating * 0.5f);
    }

    public IEnumerator IEOpen()
    {
        yield return null;
    }
}
