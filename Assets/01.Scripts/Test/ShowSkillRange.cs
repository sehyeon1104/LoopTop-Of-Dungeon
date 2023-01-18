using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowSkillRange : MonoBehaviour
{
    [SerializeField]
    private GameObject trailPrefab;
    [SerializeField]
    private float trailDistance = 20f;
    [SerializeField]
    private float trailSpeed = 5f;

    [SerializeField]
    private float safeDistance = 1f; // 공격 범위 판정


    private void Start()
    {
        Invoke("InstantiateTrailPre", 3f);
    }

    public void InstantiateTrailPre()
    {
        for(int i = 0; i < 4; ++i)
        {
            GameObject trailObj = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            trailObj.transform.SetParent(transform);
            StartCoroutine(IEMoveTrail(trailObj, i));
        }
    }

    public IEnumerator IEMoveTrail(GameObject trailObj, int count)
    {
        Vector3 dir;
        Vector3 initPos = trailObj.transform.position;

        dir = count switch
        {
            0 => Vector3.up,
            1 => Vector3.down,
            2 => Vector3.left,
            3 => Vector3.right,

            _ => Vector3.zero,
        };

        trailObj.transform.DOMove(initPos + dir * trailDistance, trailSpeed);
        yield return new WaitForSeconds(trailSpeed);
        trailObj.transform.DOMove(initPos, 1f);
        while (trailObj.transform.position != initPos)
        {
            if(Vector3.Distance(trailObj.transform.position, PlayerMovement.Instance.transform.position) < safeDistance)
            {
                Debug.Log("Safe");
                // break;
            }

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);

        if(trailObj.transform.position == initPos)
        {
            Debug.Log("Clear");
        }

        trailObj.SetActive(false);
    }
}
