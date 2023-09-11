using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColumnAttack : MonoBehaviour
{
    [SerializeField] PowerSkill powerSkill;
    VisualEffect columnTrail =null;
    Transform player; 
    Vector3[] columnVector3 = new Vector3[] { new Vector3(-0.35f,-0.2f,0),new Vector3(0.47f,-0.2f,0),new Vector3(-0.55f, -0.2f, 0), new Vector3(0.75f, 0.15f, 0), new Vector3(-0.55f, -0.2f, 0), new Vector3(0.65f, 0.1f, 0)};
    private void Awake()
    {
        player = GameManager.Instance.Player.transform;
        columnTrail = Managers.Resource.Load<VisualEffect>("Assets/10.Effects/player/Power/Column.vfx");
    }
    public void ColumnStart()
    {
        if (powerSkill.isColumn != true)
            return;
        columnTrail = player.Find("ColumnTrail").GetComponent<VisualEffect>();
        columnTrail.SetVector3("LeftHand", columnVector3[0]);
        columnTrail.SetVector3("RightHand", columnVector3[1]);
    }
    public void ColumnEnd()
    {
        if (powerSkill.isColumn != true)
            return;
        columnTrail = player.Find("ColumnTrail").GetComponent<VisualEffect>();
        columnTrail.SetVector3("LeftHand",PlayerVisual.Instance.IsFlipX() ? columnVector3[2] : columnVector3[4]);
        columnTrail.SetVector3("RightHand", PlayerVisual.Instance.IsFlipX() ? columnVector3[3] : columnVector3[5]);
        print(columnVector3[5]);
    }
}
