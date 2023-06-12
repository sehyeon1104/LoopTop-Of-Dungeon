using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondSlotUp : MonoBehaviour
{
    [SerializeField] PlayerSkill playerSkill;
    Button button;
    private void Start()
    {
        button = UIManager.Instance.GetInteractionButton();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

        UIManager.Instance.RotateInteractionButton();
        button.onClick.AddListener(SecondSlotUpdate);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {

        UIManager.Instance.RotateAttackButton();
        button.onClick.RemoveListener(SecondSlotUpdate);
        }
    }
    public void SecondSlotUpdate()
    {
        playerSkill.SlotUp(1);
    }
}
