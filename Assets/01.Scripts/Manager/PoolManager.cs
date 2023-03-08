using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Obj { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject obj, int count = 50)
        {
            Obj = obj;
            Root = new GameObject().transform;
            Root.name = $"{obj.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(CreateObj());
            }
        }

        Poolable CreateObj()
        {
            GameObject obj = Object.Instantiate<GameObject>(Obj);
            obj.name = Obj.name;

            Poolable component = obj?.GetComponent<Poolable>();
            if (component == null)
            {
                component = obj.AddComponent<Poolable>();
            }
            return component;
        }

        public void Push(Poolable poolable) //풀 내부로 집어 넣을 때
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent) //풀에서 꺼낼 때
        {
            Poolable poolable;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = CreateObj();

            poolable.gameObject.SetActive(true);

            if (parent == null)
            {
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;
            }
            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion
    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject obj, int count = 50)
    {
        Pool pool = new Pool();
        pool.Init(obj, count);
        pool.Root.parent = _root;

        _pool.Add(obj.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject obj, Transform parent = null)
    {
        if (_pool.ContainsKey(obj.name) == false)
        {
            CreatePool(obj);
        }

        return _pool[obj.name].Pop(parent);
    }

    public GameObject GetObject(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;
        return _pool[name].Obj;
    }

    public GameObject PoolManaging(string path, Vector2 position, Quaternion rotation)
    {
        GameObject clone;
        string name = path;
        int index = name.LastIndexOf('/');

        if (index >= 0)
        {
            name = name.Substring(index + 1);
        }

        if (GetObject(name) == null)
            clone = Managers.Resource.Instantiate(path);
        else
            clone = Pop(GetObject(name)).gameObject;

        clone.transform.position = position;
        clone.transform.rotation = rotation;

        return clone;
    }

    public GameObject PoolManaging(string path, Transform parent)
    {
        GameObject clone;
        string name = path;
        int index = name.LastIndexOf('/');

        if (index >= 0)
        {
            name = name.Substring(index + 1);
        }

        if (GetObject(name) == null)
            clone = Managers.Resource.Instantiate(path,parent);
        else
            clone = Pop(GetObject(name),parent).gameObject;

        return clone;
        
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
