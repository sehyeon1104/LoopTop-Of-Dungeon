using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [SerializeField]
    private Transform[] moveTransform = null;
    [SerializeField]
    private Transform[] AtkTransform = null;
    [SerializeField]
    private Transform[] InteractiveTransform = null;

    private void Awake()
    {
        Init();

    }

    private void Start()
    {
        InstantiateKeyGuide();
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

    public void InstantiateKeyGuide()
    {
        KeyManager.Instance.InstantiateKey(KeyCode.W, moveTransform[0]);
        KeyManager.Instance.InstantiateKey(KeyCode.A, moveTransform[1]);
        KeyManager.Instance.InstantiateKey(KeyCode.S, moveTransform[2]);
        KeyManager.Instance.InstantiateKey(KeyCode.D, moveTransform[3]);
        KeyManager.Instance.InstantiateKey(KeySetting.keys[KeyAction.DASH], moveTransform[4]);

        for(int i = 0; i < AtkTransform.Length; ++i)
        {
            var str = (KeyAction)Enum.Parse(typeof(KeyAction), AtkTransform[i].name.ToUpper());
            KeyManager.Instance.InstantiateKey(KeySetting.keys[str], AtkTransform[i]);
        }

        for (int i = 0; i < InteractiveTransform.Length; ++i)
        {
            var str = (KeyAction)Enum.Parse(typeof(KeyAction), InteractiveTransform[i].name.ToUpper());
            KeyManager.Instance.InstantiateKey(KeySetting.keys[str], InteractiveTransform[i]);
        }

        //AtkTransform

        //InteractiveTransform
    }
}
