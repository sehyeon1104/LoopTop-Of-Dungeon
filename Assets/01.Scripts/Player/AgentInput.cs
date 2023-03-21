using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AgentInput : MonoSingleton<AgentInput>,IAgentInput
{
    [SerializeField]
    Joystick _joyStick;
    public UnityEvent Skill1;
    public UnityEvent skill2;
    public UnityEvent dash;
    public UnityEvent ultimateSkill;
    [field:SerializeField] public UnityEvent Attack { get; set; }
    [field:SerializeField] public UnityEvent SkillFirst { get; set; }
    [field:SerializeField] public UnityEvent<Vector2> MovementInput { get; set; }

    private void FixedUpdate()
    {
        GetMovementInputMove();
    }
    void Update()   
    {
      
 
    }
    public void ButtonClick()
    {
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj.CompareTag("Attack"))
        {
            Attack.Invoke();
        }
        else if(clickedObj.CompareTag("Skill1"))
        {
            Skill1.Invoke();
        }
        else if(clickedObj.CompareTag("Skill2"))
        {
            skill2.Invoke();
        }
        else if(clickedObj.CompareTag("Dash"))
        {
            dash.Invoke();
        }
        else
        {
            ultimateSkill.Invoke();
        }
    }
    public void GetMovementInputMove()
    {
        MovementInput.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        MovementInput.Invoke(new Vector2(_joyStick.Horizontal, _joyStick.Vertical));
    }
}
