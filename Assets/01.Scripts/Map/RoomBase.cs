using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomBase : MonoBehaviour
{
    [SerializeField]
    protected Define.MapTypeFlag mapTypeFlag;

    [SerializeField]
    protected Define.RoomTypeFlag roomTypeFlag;
    protected bool isClear = false;

    [SerializeField]
    protected SpriteRenderer minimapIconSpriteRenderer = null;

    private void Awake()
    {
        minimapIconSpriteRenderer = transform.parent.Find("MinimapIcon").GetComponent<SpriteRenderer>();
    }

    protected abstract void SetRoomTypeFlag();
    protected abstract void IsClear();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (StageManager.Instance.isSetting)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            minimapIconSpriteRenderer.color = Color.white;
        }

        GameManager.Instance.minimapCamera.MoveMinimapCamera(transform.position);
    }

}
