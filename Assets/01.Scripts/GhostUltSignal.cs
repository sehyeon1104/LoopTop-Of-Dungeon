using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostUltSignal : MonoBehaviour
{
    [SerializeField] private Animation GhostUltAnim;

    [SerializeField] private Image panel;

    [SerializeField] private SpriteRenderer[] GhostSprite;


    List<string> animArray;

    internal int index = 0;

    bool isArrayed = false;

    float alpha = 0;


    public void UltSkill()
    {
        
        if (isArrayed == false)
        {
            animArray = new List<string>();
            AnimationArray();
        }
        
        GhostUltAnim.Play(animArray[0]);
    }

    public void AnimationArray()
    {
        foreach (AnimationState states in GhostUltAnim)
        {
            animArray.Add(states.name);
            Debug.Log(states.name);
        }
        isArrayed = true;
    }

    public void ScreenDark()
    {
        StartCoroutine(ScreenDarkCor());
    }

    public void ScreenWhite()
    {
        StartCoroutine(ScreenWhiteCor());
    }

    public IEnumerator ScreenDarkCor()
    {
        
        panel.GetComponent<Image>();
        Color color = panel.color;
        
        while(alpha < 0.5f)
        {
            color.a = alpha;
            yield return new WaitForSeconds(0.01f);
            panel.color = color;
            alpha += 0.01f;
        }

    }

    public IEnumerator ScreenWhiteCor()
    {

        panel.GetComponent<Image>();
        Color color = panel.color;

        while (alpha > 0.0f)
        {
            color.a = alpha;
            yield return new WaitForSeconds(0.01f);
            panel.color = color;
            alpha -= 0.01f;
        }

    }


}
