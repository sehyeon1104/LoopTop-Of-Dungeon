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
    private GameObject[] trailsArr;

    [SerializeField]
    private float safeDistance = 1f; // 공격 범위 판정

    private bool isSafe = false;

    private void Start()
    {
        trailsArr = new GameObject[4];

        for (int i = 0; i < 4; ++i)
        {
            GameObject trailObj = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            trailObj.transform.SetParent(transform);
            trailsArr[i] = trailObj;
            trailsArr[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActiveTrailPre();
        }
    }

    public void ActiveTrailPre()
    {
        isSafe = false;

        for(int i = 0; i < 4; ++i)
        {
            trailsArr[i].transform.position = PlayerMovement.Instance.transform.position;
            StartCoroutine(IEMoveTrail(trailsArr[i], i));
        }
    }

    public IEnumerator IEMoveTrail(GameObject trailObj, int count)
    {
        Vector3 dir;
        Vector3 initPos = PlayerMovement.Instance.transform.position;

        TrailRenderer trailRenderer = trailObj.GetComponent<TrailRenderer>();

        dir = count switch
        {
            0 => Vector3.up,
            1 => Vector3.down,
            2 => Vector3.left,
            3 => Vector3.right,

            _ => Vector3.zero,
        };


        // 범위가 상하좌우로 이동
        trailRenderer.time = trailSpeed + 1f;
        yield return new WaitUntil(() => trailRenderer.time == trailSpeed + 1f);

        trailObj.SetActive(true);
        trailObj.transform.DOMove(initPos + dir * trailDistance, trailSpeed);
        yield return new WaitForSeconds(trailSpeed);

        // 원래 위치로 돌아옴
        trailObj.transform.DOMove(initPos, 1f);
        while (trailObj.transform.position != initPos)
        {
            if(Vector3.Distance(trailObj.transform.position, PlayerMovement.Instance.transform.position) < safeDistance)
            {
                if (!isSafe)
                {
                    Debug.Log("Safe");
                    isSafe = true;
                }
            }

            yield return new WaitForEndOfFrame();
        }

        if (!isSafe)
        {
            Debug.Log("Hit");
        }

        trailRenderer.time = 0f;
        yield return new WaitUntil(() => trailRenderer.time == 0f);

        trailObj.SetActive(false);
    }
}
