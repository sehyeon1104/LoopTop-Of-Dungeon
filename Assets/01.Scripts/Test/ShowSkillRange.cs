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


    private void Start()
    {
        Invoke("InstantiateTrailPre", 3f);
    }

    public void InstantiateTrailPre()
    {
        for(int i = 0; i < 4; ++i)
        {
            GameObject trailObj = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            MoveTrail(trailObj, i);
        }
    }

    public void MoveTrail(GameObject trailObj, int count)
    {
        Vector3 dir;

        dir = count switch
        {
            0 => Vector3.up,
            1 => Vector3.down,
            2 => Vector3.left,
            3 => Vector3.right,

            _ => Vector3.zero,
        };


        trailObj.transform.DOMove(dir * trailDistance, trailSpeed);
    }
}
