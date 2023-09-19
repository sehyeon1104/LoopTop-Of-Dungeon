using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayTimerManager : MonoSingleton<GamePlayTimerManager>
{
    private static float timer = 0f;

    private static bool isStart = false;

    private void Update()
    {
        if (!isStart)
            return;

        timer += Time.deltaTime;
    }

    private void StartTimer()
    {
        isStart = true;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    public float GetTimer()
    {
        return timer;
    }
}
