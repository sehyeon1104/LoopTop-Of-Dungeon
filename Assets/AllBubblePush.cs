using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBubblePush : MonoBehaviour
{
    WaitUntil waituntillbubble = new WaitUntil(() => GhostBossFieldPattern.isPushAllBubbles == true);

    void Start()
    {
        StartCoroutine(PushAllBubbles());
    }

    IEnumerator PushAllBubbles()
    {
        yield return waituntillbubble;
        Managers.Pool.Push(GetComponent<Poolable>());
        GhostBossFieldPattern.isPushAllBubbles = false;
    }
}
