using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoObjSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform dummySpawnPos = null;
    [SerializeField]
    private Transform chestSpawnPos = null;
    [SerializeField]
    private Transform dropItemPos = null;

    private GameObject dummyPrefab = null;
    private GameObject chestPrefab = null;
    private GameObject dropItemObj = null;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        LoadObjPrefab();
        InstantiateAllObj();
    }

    private void LoadObjPrefab()
    {
        chestPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Chest.prefab");
        dummyPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/Dummy.prefab");
    }

    public void InstantiateAllObj()
    {
        InstantiateDummy();
        InstantiateChest();
        InstantiateDropItem();
    }

    public void InstantiateDummy()
    {
        GameObject dummyObj = Instantiate(dummyPrefab);
        Vector3 pos = dummySpawnPos.position;
        pos.y += 1;
        dummyObj.transform.position = pos;
    }

    public void InstantiateChest()
    {
        GameObject chestObj = Instantiate(chestPrefab);
        Vector3 pos = chestSpawnPos.position;
        chestObj.transform.position = pos;
        Chest chest = chestObj.GetComponent<Chest>();
        chest.SetChestRating(Define.ChestRating.Rare);
    }
    
    public void InstantiateDropItem()
    {
        dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
        dropItemObj.transform.position = dropItemPos.position;
        dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Common);
    }
}
