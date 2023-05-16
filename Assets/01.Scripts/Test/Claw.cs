using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    private void OnEnable()
    {
        Managers.Sound.Play("Assets/05.Sounds/SoundEffects/Ghost/G_Claw.mp3", Define.Sound.Effect, 1, transform.lossyScale.x * 0.5f);
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(transform.lossyScale.y * 2, transform.lossyScale.x * 2), transform.rotation.z, 1<<8);
        foreach (var col in cols)
        {
            GameManager.Instance.Player.OnDamage(2, 0);
        }
    }
}
