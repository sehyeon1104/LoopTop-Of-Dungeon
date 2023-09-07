using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoObjSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform dummySpawnPos = null;
    [SerializeField]
    private Transform statuePos = null;
    [SerializeField]
    private Transform dropItemPos = null;

    private GameObject dummyPrefab = null;
    private GameObject statuePrefab = null;
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
        dummyPrefab = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/Enemy/Dummy.prefab");
    }

    public void InstantiateAllObj()
    {
        InstantiateDummy();
        InstantiateStatue();
        InstantiateDropItem();
    }

    public void InstantiateDummy()
    {
        GameObject dummyObj = Instantiate(dummyPrefab);
        Vector3 pos = dummySpawnPos.position;
        pos.y += 1;
        dummyObj.transform.position = pos;
    }

    public void InstantiateStatue()
    {

    }
    
    public void InstantiateDropItem()
    {
        dropItemObj = Managers.Resource.Instantiate("Assets/03.Prefabs/2D/DropItem.prefab");
        dropItemObj.transform.position = dropItemPos.position;
        dropItemObj.GetComponent<DropItem>().SetItem(Define.ChestRating.Common);
    }
}
