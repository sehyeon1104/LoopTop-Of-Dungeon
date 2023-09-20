using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayTimerManager : MonoSingleton<GamePlayTimerManager>
{
    private static float startTime = 0f;
    private float resultTime = 0f;
    private string timeStr = string.Empty;

    public void StartTimer()
    {
        startTime = Time.time;
    }

    public void ResetTimer()
    {
        startTime = 0;
    }

    public string GetTimer()
    {
        ReplacementTime();
        return timeStr;
    }

    public void ReplacementTime()
    {
        int num = 3600;
        timeStr = string.Empty;
        resultTime = Mathf.CeilToInt(Time.time - startTime);

        while(resultTime > 0)
        {
            if ((int)(resultTime / num) > 0)
                timeStr += (int)(resultTime / num);
            else
                timeStr += "00";

            timeStr += ":";
            resultTime %= num;
            num /= 60;
        }
        timeStr = timeStr.Remove(timeStr.Length - 1);
    }
}
