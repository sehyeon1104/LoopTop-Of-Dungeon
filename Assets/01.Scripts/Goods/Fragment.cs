using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fragment : MonoBehaviour
{
    [SerializeField]
    private float collectionTime = 1.5f;
    [SerializeField]
    private float moveToPlayerTime = 0.5f;
    
    private Transform playerTransform;
    private Poolable fragmentPoolable;

    private void Start()
    {
        fragmentPoolable = GetComponent<Poolable>();
    }

    private void OnEnable()
    {
        playerTransform = GameManager.Instance.Player.transform;
        transform.position = playerTransform.position;

        FallToGround();
    }

    public void FallToGround()
    {
        transform.DOMove( (Random.insideUnitCircle * 0.5f) + (Vector2)transform.position , 0.4f);
        StartCoroutine(MoveToPlayer());
    }

    public IEnumerator MoveToPlayer()
    {
        yield return new WaitForSeconds(collectionTime);

        transform.DOMove(playerTransform.position, moveToPlayerTime);
        yield return new WaitForSeconds(moveToPlayerTime);

        Managers.Pool.Push(fragmentPoolable);
        GameManager.Instance.Player.playerBase.FragmentAmount += Random.Range(2, 3);
    }


}
