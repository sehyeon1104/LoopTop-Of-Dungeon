using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillShuffle : MonoBehaviour
{
    [SerializeField] PlayerSkill playerSkill;
    Button button;
    private void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIManager.Instance.RotateInteractionButton();
        button.onClick.AddListener(SlotShuffle);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.Instance.RotateAttackButton();
        button.onClick.RemoveListener(SlotShuffle);
    }
    public void SlotShuffle()
    {
        StartCoroutine(playerSkill.SkillShuffle());
    }
}
