using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestRoom : RoomBase
{
    private int spawnChestCount = 3;
    private GameObject chestObj = null;
    private GameObject minimapIcon = null;

    private List<Chest> chestList = new List<Chest>();

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    protected override void Awake()
    {
        base.Awake();
        minimapIcon = Managers.Resource.Instantiate("Assets/03.Prefabs/MinimapIcon/ChestRoomIcon.prefab", transform);
        minimapIcon.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y + 1.5f);
        minimapIcon.SetActive(false);

        //minimapIconSpriteRenderer = transform.parent.Find("MinimapIcon").GetComponent<SpriteRenderer>();
        //minimapIconSpriteRenderer.gameObject.SetActive(false);
        //curLocatedMapIcon = transform.parent.Find("CurLocatedIcon").gameObject;
    }

    private void Start()
    {
        chestObj = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Chest.prefab");
        SpawnChest();
    }

    protected override void IsClear()
    {
        isClear = true;
    }

    protected override void ShowIcon()
    {
        Debug.Log("ShowIcon");
        minimapIcon.SetActive(true);
    }

    protected override void SetRoomTypeFlag()
    {
        roomTypeFlag = Define.RoomTypeFlag.EventRoom;
    }

    public void SpawnChest()
    {
        for(int i = 0; i < spawnChestCount; ++i)
        {
            Vector3 chestPos = new Vector3(transform.position.x - 5 + (4.5f * i), transform.position.y);
            GameObject spawnChest = Instantiate(chestObj, transform);
            spawnChest.transform.position = chestPos;
            Chest chest = spawnChest.GetComponent<Chest>();
            // юс╫ц
            chest.SetChestRating(Define.ChestRating.Epic);

            chestList.Add(chest);
        }

        StartCoroutine(CheckChestOpen());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    private IEnumerator CheckChestOpen()
    {
        int chestListCount = chestList.Count;
        int index = 0;

        while (true)
        {
            if (chestList[index].IsOpen)
            {
                for(int i = 0; i < chestListCount; ++i)
                {
                    chestList[i].IsOpen = true;
                    StartCoroutine(chestList[i].IEDestroyChest());
                }
                break;
            }

            index++;
            index %= chestListCount;
            yield return waitForEndOfFrame;
        }
    }

}
