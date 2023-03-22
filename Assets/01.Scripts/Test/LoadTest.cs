using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadTest : MonoBehaviour
{
    // ��巹������ Label�� ���� �� �ִ� �ʵ�.
    public AssetLabelReference assetLabel;

    private IList<IResourceLocation> _locations;
    // ������ ���ӿ�����Ʈ�� Destroy�ϱ� ���� �������� ĳ���Ѵ�.
    [SerializeField]
    private List<GameObject> _gameObjects = new List<GameObject>();

    [SerializeField]
    private GameObject[] ghostNormalEnemyPrefabs;// = new GameObject[10];
    [SerializeField]
    private GameObject[] ghostEliteEnemyPrefabs;// = new GameObject[10];


    private void Start()
    {
        GetLocations();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log(_locations.Count);
            SetEnemyPre();
        }
    }

    public void SetEnemyPre()
    {
        Instantiate();

    }

    public void GetLocations()
    {
        // ����Ÿ���� ��θ� �����´�.
        // ����̱� ������ �޸𸮿� ������ �ε���� �ʴ´�.
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
            };

    }

    public void Instantiate()
    {
        var location = _locations[Random.Range(0, _locations.Count)];

        // ��θ� ���ڷ� GameObject�� �����Ѵ�.
        // ������ �޸𸮿� GameObject�� �ε�ȴ�.
        for(int i = 0; i < _locations.Count; ++i)
        {
            location = _locations[i];
            Debug.Log(location);

            Addressables.InstantiateAsync(location, Vector3.one, Quaternion.identity).Completed +=
            (handle) =>
            {
                // ������ ��ü�� ������ ĳ��
                _gameObjects.Add(handle.Result);
            };
        }
        StartCoroutine(ReleaseEnemy());
    }

    public IEnumerator ReleaseEnemy()
    {
        WaitUntil waitUntil = new WaitUntil(() => _gameObjects.Count == _locations.Count);
        yield return waitUntil;

        Debug.Log("ReleaseEnemy");

        for (int i = 0; i < _gameObjects.Count; ++i)
        {
            Debug.Log(_gameObjects[i]);
            _gameObjects[i].gameObject.SetActive(false);
        }

        ghostNormalEnemyPrefabs = _gameObjects.ToArray();
        ghostEliteEnemyPrefabs = _gameObjects.ToArray();
    }

    public void Release()
    {
        if (_gameObjects.Count == 0)
            return;

        var index = _gameObjects.Count - 1;
        // InstantiateAsync <-> ReleaseInstance
        // Destroy�Լ��ν� ref count�� 0�̸� �޸� ���� ������ ��ε��Ѵ�.
        Addressables.ReleaseInstance(_gameObjects[index]);
        _gameObjects.RemoveAt(index);
    }
}
