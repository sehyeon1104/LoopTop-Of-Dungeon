using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class GhostUltSignal : MonoBehaviour
{
    [SerializeField] private GameObject GhostBoss;

    [SerializeField] private GameObject Player;

    [SerializeField] private GameObject GhostBossSkill;

    [SerializeField] private Animation GhostUltAnim;

    [SerializeField] private Image panel;

    [SerializeField] private SpriteRenderer[] GhostSprite;

    PlayableDirector PD;

    List<string> animArray;

    WaitForSeconds zerodotzeroone = new WaitForSeconds(0.01f);

    internal int index = 0;

    bool isArrayed = false;

    float alpha = 0;

    private void Start()
    {
        UltSkillAnim(); //이걸 스타트에 해주고 시네머신에는 몇번째 울트 애니메이션인지 넣자
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


    public void UltSkillAnim()
    {
        
        if (isArrayed == false)
        {
            animArray = new List<string>();
            AnimationArray();
        }
        
        
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
        
        while(alpha < 1f)
        {
            color.a = alpha;
            yield return zerodotzeroone;
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
            yield return zerodotzeroone;
            panel.color = color;
            alpha -= 0.01f;
        }

    }
    public void UltSkillCast()
    {
        PD = GetComponent<PlayableDirector>();
        PD.Play();
    }

    public void GhostBossTransform()
    {
        GhostBoss.transform.position = Player.transform.position;
        GhostBossSkill.transform.position = Player.transform.position;
    }

    public void Ult1()
    {
        GhostUltAnim.Play(animArray[0]);
    }

    public void Ult2()
    {
        GhostUltAnim.Play(animArray[1]);
    }

    public void Ult3()
    {
        GhostUltAnim.Play(animArray[2]);
    }

    public void Ult4()
    {
        GhostUltAnim.Play(animArray[3]);
    }


}
