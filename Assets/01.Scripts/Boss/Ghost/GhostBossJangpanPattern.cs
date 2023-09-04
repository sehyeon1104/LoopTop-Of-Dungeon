using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Cinemachine;

public class GhostBossJangpanPattern : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera jangpanVCam;

    Poolable circlesmoke;
    Poolable FRPCol;
    Poolable FRPRS;
    Poolable recsmoke;
    Poolable FRPRCol;

    float currenttime = 0f;

    float patterncurrenttime = 0f;

    private void Awake()
    {
        circlesmoke = Managers.Pool.PoolManaging("10.Effects/ghost/CircleSmoke", new Vector2(1000, 1000), Quaternion.identity);
        FRPCol = Managers.Pool.PoolManaging("10.Effects/ghost/CircleCol", new Vector2(1000, 1000), Quaternion.identity);
        FRPRS = Managers.Pool.PoolManaging("10.Effects/ghost/FRPRS", new Vector2(1000, 1000), Quaternion.identity);
        recsmoke = Managers.Pool.PoolManaging("10.Effects/ghost/RecSmoke", new Vector2(1000, 1000), Quaternion.identity);
        FRPRCol = Managers.Pool.PoolManaging("10.Effects/ghost/RecCol", new Vector2(1000, 1000), Quaternion.identity);

        Managers.Pool.Push(circlesmoke);
        Managers.Pool.Push(FRPCol);
        Managers.Pool.Push(FRPRS);
        Managers.Pool.Push(recsmoke);
        Managers.Pool.Push(FRPRCol);
    }

    public IEnumerator FloorPatternCircle()
    {
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/GhostCircleWarning.prefab" ,transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3.5f);

        Managers.Pool.Pop(circlesmoke.gameObject, transform.position);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Hilla1.wav");

        yield return new WaitForSeconds(0.5f);

        Poolable Col = Managers.Pool.Pop(FRPCol.gameObject, transform.position);
        Col.transform.position = transform.position;

        yield return new WaitForSeconds(4f);

        Managers.Pool.Push(Col);

        yield return null;
    }

    public IEnumerator FloorPatternRectangle()
    {
        jangpanVCam.transform.position = new Vector3(15.5f, 7f, -1f);
        jangpanVCam.Priority = 11;

        Managers.Pool.Pop(FRPRS.gameObject, transform.position);
        Managers.Pool.PoolManaging("Assets/10.Effects/ghost/GhostBigWarning.prefab", transform.position, Quaternion.identity);

        FRPRS.transform.position = transform.position;

        FRPRS.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);

        Managers.Pool.Push(FRPRS);
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Boss/Ghost/G_Hilla1.wav");

        Poolable clone = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone.transform.position = new Vector2(transform.position.x + (-11.26f), transform.position.y + (-12.18f));
        Poolable clone1 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone1.transform.position = new Vector2(transform.position.x + (-23.73f), transform.position.y);
        Poolable clone2 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone2.transform.position = new Vector2(transform.position.x + (-11.26f), transform.position.y + 12.18f);
        Poolable clone3 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone3.transform.position = new Vector2(transform.position.x, transform.position.y + 24);
        Poolable clone4 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone4.transform.position = new Vector2(transform.position.x + 11.26f, transform.position.y + 12.18f);
        Poolable clone5 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone5.transform.position = new Vector2(transform.position.x + 23.73f, transform.position.y);
        Poolable clone6 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone6.transform.position = new Vector2(transform.position.x + 11.26f, transform.position.y + (-12.18f));
        Poolable clone7 = Managers.Pool.Pop(recsmoke.gameObject, transform.position);
        clone7.transform.position = new Vector2(transform.position.x, transform.position.y + (-23.73f));

        yield return new WaitForSeconds(0.7f);

        Poolable Col = Managers.Pool.Pop(FRPRCol.gameObject, transform.position);
        Col.transform.position = transform.position;

        yield return new WaitForSeconds(5f);

        currenttime = 0f;
        Managers.Pool.Push(FRPRCol);
        jangpanVCam.Priority = 0;


    }

}
