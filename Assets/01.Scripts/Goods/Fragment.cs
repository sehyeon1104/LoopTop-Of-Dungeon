using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fragment : MonoBehaviour
{
    [SerializeField]
    private float collectionTime = 1f;
    [SerializeField]
    private float moveToPlayerTime = 0.3f;

    [SerializeField]
    private float moveAmount = 0.75f;
    private float jumpPower = 0.5f;

    private Transform playerTransform;
    private Poolable fragmentPoolable;

    private bool isCreate = false;

    private bool isDecrease = false;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        fragmentPoolable = GetComponent<Poolable>();
        playerTransform = GameManager.Instance.Player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (!isCreate)
        {
            isCreate = true;
            return;
        }

        spriteRenderer.DOFade(1f, 0f);
        Invoke("FallToGround", 0.01f);
    }

    public void FallToGround()
    {
        Vector3 dir = (transform.position - playerTransform.position).normalized;
        transform.DOJump(transform.position + dir * moveAmount, jumpPower, 1, 0.4f);

        //transform.DOMove( (Random.insideUnitCircle * 0.75f) + (Vector2)transform.position , 0.4f);
        if (isDecrease)
            StartCoroutine(FadeFragment());
        else
            StartCoroutine(MoveToPlayer());
    }

    public IEnumerator MoveToPlayer()
    {
        ItemManager.Instance.FragmentDropRelatedItemEffects.Invoke(transform.position);
        yield return new WaitForSeconds(collectionTime);

        transform.DOMove(playerTransform.position, moveToPlayerTime).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(moveToPlayerTime);

        Managers.Pool.Push(fragmentPoolable);
        FragmentCollectManager.Instance.IncreaseGoods();
    }

    public IEnumerator FadeFragment()
    {
        yield return new WaitForSeconds(collectionTime);

        spriteRenderer.DOFade(0f, moveToPlayerTime);
    }

    public void SetIsDecrease(bool decrease)
    {
        isDecrease = decrease;
    }
}
