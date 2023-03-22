using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadTest : MonoBehaviour
{
    // 어드레서블의 Label을 얻어올 수 있는 필드.
    public AssetLabelReference assetLabel;

    private IList<IResourceLocation> _locations;
    // 생성된 게임오브젝트를 Destroy하기 위해 참조값을 캐싱한다.
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
        // 빌드타겟의 경로를 가져온다.
        // 경로이기 때문에 메모리에 에셋이 로드되진 않는다.
        Addressables.LoadResourceLocationsAsync(assetLabel.labelString).Completed +=
            (handle) =>
            {
                _locations = handle.Result;
            };

    }

    public void Instantiate()
    {
        var location = _locations[Random.Range(0, _locations.Count)];

        // 경로를 인자로 GameObject를 생성한다.
        // 실제로 메모리에 GameObject가 로드된다.
        for(int i = 0; i < _locations.Count; ++i)
        {
            location = _locations[i];
            Debug.Log(location);

            Addressables.InstantiateAsync(location, Vector3.one, Quaternion.identity).Completed +=
            (handle) =>
            {
                // 생성된 개체의 참조값 캐싱
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
        // Destroy함수로써 ref count가 0이면 메모리 상의 에셋을 언로드한다.
        Addressables.ReleaseInstance(_gameObjects[index]);
        _gameObjects.RemoveAt(index);
    }
}
