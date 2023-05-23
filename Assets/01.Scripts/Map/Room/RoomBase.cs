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
    [SerializeField]
    protected GameObject curLocatedMapIcon = null;

    private void Awake()
    {
        minimapIconSpriteRenderer = transform.parent.Find("MinimapIcon").GetComponent<SpriteRenderer>();
        curLocatedMapIcon = transform.parent.Find("CurLocatedIcon").gameObject;
    }

    protected virtual void Start()
    {
        minimapIconSpriteRenderer.gameObject.SetActive(false);
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
            curLocatedMapIcon.SetActive(true);

            ChangeMinimapIconColor();
            ShowInMinimap();
            CheckLinkedRoom();
            GameManager.Instance.minimapCamera.MoveMinimapCamera(transform.position);
        }
    }

    public void ChangeMinimapIconColor()
    {
        minimapIconSpriteRenderer.color = Color.white;
    }

    public void ShowInMinimap()
    {
        minimapIconSpriteRenderer.gameObject.SetActive(true);
    }

    public void CheckLinkedRoom()
    {
        StageManager.Instance.ShowLinkedMapInMinimap(transform.parent.position);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            minimapIconSpriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
            curLocatedMapIcon.SetActive(false);
        }
    }
}
