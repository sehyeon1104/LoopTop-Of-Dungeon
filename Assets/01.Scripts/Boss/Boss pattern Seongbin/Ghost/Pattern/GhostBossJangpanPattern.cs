using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GhostBossJangpanPattern : MonoBehaviour
{
    private SpriteRenderer FRPRSpriterenderer;
    private SpriteRenderer FRPRStartSpriterenderer;
    private SpriteRenderer FRPSpriterenderer;
    private SpriteRenderer FRPStartSpriterenderer;

    Poolable FRP;
    Poolable FRPS;
    Poolable circlesmoke;
    Poolable FRPCol;
    Poolable FRPR;
    Poolable FRPRS;
    Poolable recsmoke;
    Poolable FRPRCol;

    float currenttime = 0f;

    private void Awake()
    {
        FRP = Managers.Pool.PoolManaging("10.Effects/ghost/FPR", new Vector2(1000, 1000), Quaternion.identity);
        FRPS = Managers.Pool.PoolManaging("10.Effects/ghost/FPRS", new Vector2(1000, 1000), Quaternion.identity);
        circlesmoke = Managers.Pool.PoolManaging("10.Effects/ghost/CircleSmoke", new Vector2(1000, 1000), Quaternion.identity);
        FRPCol = Managers.Pool.PoolManaging("10.Effects/ghost/CircleCol", new Vector2(1000, 1000), Quaternion.identity);
        FRPR = Managers.Pool.PoolManaging("10.Effects/ghost/FRPR", new Vector2(1000, 1000), Quaternion.identity);
        FRPRS = Managers.Pool.PoolManaging("10.Effects/ghost/FRPRS", new Vector2(1000, 1000), Quaternion.identity);
        recsmoke = Managers.Pool.PoolManaging("10.Effects/ghost/RecSmoke", new Vector2(1000, 1000), Quaternion.identity);
        FRPRCol = Managers.Pool.PoolManaging("10.Effects/ghost/RecCol", new Vector2(1000, 1000), Quaternion.identity);
        Managers.Pool.Push(FRP);
        Managers.Pool.Push(FRPS);
        Managers.Pool.Push(circlesmoke);
        Managers.Pool.Push(FRPCol);
        Managers.Pool.Push(FRPR);
        Managers.Pool.Push(FRPRS);
        Managers.Pool.Push(recsmoke);
        Managers.Pool.Push(FRPRCol);


    }

    public void FloorPatternCircle()
    {
        Managers.Pool.Pop(FRP.gameObject, transform.position);
        Managers.Pool.Pop(FRPS.gameObject, transform.position);

        FRPSpriterenderer = FRP.GetComponent<SpriteRenderer>();
        FRPStartSpriterenderer = FRPS.GetComponent<SpriteRenderer>();

        FRP.transform.position = transform.position;
        FRPS.transform.position = transform.position;


        FRP.gameObject.SetActive(true);
        FRPS.gameObject.SetActive(true);

        FRP.transform.DOScale(new Vector2(30f, 30f), 4f);

        while (currenttime < 4f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;


        while (currenttime < 0.5f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;

        FRPS.transform.DOScale(new Vector2(30f, 30f), 2f);

        while (currenttime < 2f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;

        FRPSpriterenderer.enabled = false;
        FRPStartSpriterenderer.enabled = false;

        Managers.Pool.Pop(circlesmoke.gameObject, transform.position);

        Poolable Col = Managers.Pool.Pop(FRPCol.gameObject, transform.position);
        Col.transform.position = transform.position;

        while (currenttime < 5f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;

        FRP.transform.localScale = Vector2.zero;
        FRPS.transform.localScale = Vector2.zero;

        FRPSpriterenderer.enabled = true;
        FRPStartSpriterenderer.enabled = true;

        FRP.gameObject.SetActive(false);
        FRPS.gameObject.SetActive(false);
        FRPCol.gameObject.SetActive(false);

        Managers.Pool.Push(FRP);
        Managers.Pool.Push(FRPS);
        Managers.Pool.Push(FRPCol);

    }

    public void FloorPatternRectangle()
    {
        Managers.Pool.Pop(FRPR.gameObject, transform.position);
        Managers.Pool.Pop(FRPRS.gameObject, transform.position);

        FRPRSpriterenderer = FRPR.GetComponent<SpriteRenderer>();
        FRPRStartSpriterenderer = FRPRS.GetComponent<SpriteRenderer>();

        FRPR.transform.position = transform.position;
        FRPRS.transform.position = transform.position;


        FRPR.gameObject.SetActive(true);
        FRPRS.gameObject.SetActive(true);

        //30����
        FRPR.transform.DOScale(new Vector2(30f, 30f), 4f);

        while (currenttime < 4f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;

        while (currenttime < 0.5f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;

        FRPRS.transform.DOScale(new Vector2(30f, 30f), 2f);

        while (currenttime < 2f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0;

        FRPRS.transform.localScale = Vector2.zero;
        FRPR.transform.localScale = Vector2.zero;

        FRPRSpriterenderer.enabled = false;
        FRPRStartSpriterenderer.enabled = false;
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

        Poolable Col = Managers.Pool.Pop(FRPRCol.gameObject, transform.position);
        Col.transform.position = transform.position;

        while (currenttime < 5f)
        {
            currenttime += Time.deltaTime;
        }

        currenttime = 0f;

        FRPRSpriterenderer.enabled = true;
        FRPRStartSpriterenderer.enabled = true;

        FRPR.gameObject.SetActive(false);
        FRPRS.gameObject.SetActive(false);
        FRPRCol.gameObject.SetActive(false);

        Managers.Pool.Push(FRPR);
        Managers.Pool.Push(FRPRS);
        Managers.Pool.Push(FRPRCol);


    }

}
