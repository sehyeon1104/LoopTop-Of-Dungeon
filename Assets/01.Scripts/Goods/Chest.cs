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

    // 최소 경험의 조각 획득량
    private float minFragmentAmount = 100;
    // 최대 경험의 조각 획득량
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
        // (경험의조각 획득량 * 상자 등급( 1 ~ 4 ) ) / 2
        minFragmentAmount *= (float)_chestRating * 0.5f;
        maxFragmentAmount *= (float)_chestRating * 0.5f;

        // hp 회복 구슬 = 반올림(상자등급 / 2)
        hpRegenAmount = Mathf.RoundToInt((float)_chestRating * 0.5f);
    }

    public IEnumerator IEOpen()
    {
        yield return null;
    }
}
