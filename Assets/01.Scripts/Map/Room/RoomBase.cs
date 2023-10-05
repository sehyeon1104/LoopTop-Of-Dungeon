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
    protected GameObject minimapUnexplorerdRoomIcon = null;
    [SerializeField]
    protected SpriteRenderer minimapIconSpriteRenderer = null;
    [SerializeField]
    protected GameObject curLocatedMapIcon = null;

    protected GameObject doors = null;

    protected virtual void Awake()
    {
        minimapIconSpriteRenderer = transform.parent.Find("MinimapIcon").GetComponent<SpriteRenderer>();
        minimapIconSpriteRenderer.gameObject.SetActive(false);
        doors = transform.parent.Find("Doors").gameObject;
        doors.SetActive(false);
        minimapUnexplorerdRoomIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/UnExploreredMinimapIcon.prefab", transform);
        minimapUnexplorerdRoomIcon.SetActive(false);
        // curLocatedMapIcon = transform.parent.Find("CurLocatedIcon").gameObject;
    }

    protected virtual void SetRoomTypeFlag()
    {

    }

    protected abstract void IsClear();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (StageManager.Instance.isSetting)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            // curLocatedMapIcon.SetActive(true);  
            if (!isClear)
            {
                ShowInMinimap();
                CheckLinkedRoom();
            }

            ShowIcon();
            ChangeMinimapIconColor();
            GameManager.Instance.minimapCamera.MoveMinimapCamera(minimapIconSpriteRenderer.transform.position);
        }
    }

    public void ChangeMinimapIconColor()
    {
        minimapIconSpriteRenderer.color = Color.white;
    }

    public void ShowInMinimap()
    {
        if (!minimapIconSpriteRenderer.gameObject.activeSelf)
        {
            minimapIconSpriteRenderer.gameObject.SetActive(true);
            minimapUnexplorerdRoomIcon.gameObject.SetActive(true);
            // ShowIcon();
        }
    }

    public void CheckLinkedRoom()
    {
        int nx = 0;
        int ny = 0;
        int[] dx = new int[] { 0, 0, -1, 1 };
        int[] dy = new int[] { 1, -1, 0, 0 };

        for (int i = 0; i < 4; ++i)
        {
            nx = (int)(transform.parent.position.x / 28f) + dx[i];
            ny = ((int)(transform.parent.position.y / 28f) + dy[i]) * -1;

            if (nx > StageManager.Instance.arrSize - 1 || nx < 0 || ny > StageManager.Instance.arrSize - 1 || ny < 0)
                continue;
            if (StageManager.Instance.GetMapArr()[ny, nx] == 0)
                continue;

            StageManager.Instance.wallDic[new Vector3(nx, ny)].GetComponentInChildren<RoomBase>().ShowInMinimap();
        }
    }

    protected virtual void ShowIcon()
    {
        minimapUnexplorerdRoomIcon.SetActive(false);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            minimapIconSpriteRenderer.color = new Color(0.8f, 0.8f, 0.8f);
            //curLocatedMapIcon.SetActive(false);
        }
    }

    public void ToggleDoors()
    {
        doors.SetActive(!isClear);
    }
}
