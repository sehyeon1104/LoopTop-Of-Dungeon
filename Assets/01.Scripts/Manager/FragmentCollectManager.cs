using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentCollectManager : MonoSingleton<FragmentCollectManager>
{
    private GameObject fragmentCollect;
    private Poolable fragmentObj;

    // 경험조각 획득량을 담아줄 큐
    private Queue<int> fragmentAmountQueue = new Queue<int>();
    // 큐 안의 값을 다 더해줄 target
    private float target;

    // 카운팅에 걸리는 시간 설정. 
    [SerializeField]
    private float duration = 0.5f;
    // 오프셋
    private float offset;
    // 현재 보유량
    private float current;

    private Coroutine coroutine = null;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private WaitForSeconds waitIncreaseFragment = new WaitForSeconds(1.5f);

    private void Awake()
    {
        fragmentCollect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Fragment.prefab");
    }

    private void Start()
    {
        Managers.Pool.CreatePool(fragmentCollect, 200);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropFragmentByCircle(GameManager.Instance.Player.gameObject, 100, 10);
        }
    }

    public void DropFragment(GameObject obj, int amount, int count = 3)
    {
        for(int i = 0; i < count; ++i)
        {
            fragmentObj = Managers.Pool.Pop(fragmentCollect);
            fragmentObj.transform.position = obj.transform.position;
        }

        // TODO : 재화 획득 처리
        fragmentAmountQueue.Enqueue(amount);
        if(coroutine == null)
        {
            coroutine = StartCoroutine(IncreaseGoods());
        }
    }

    public void DropFragmentByCircle(GameObject obj, int amount, int count = 3)
    {
        for (int i = 0; i < count; ++i)
        {
            fragmentObj = Managers.Pool.Pop(fragmentCollect);
            fragmentObj.transform.position = (Random.insideUnitCircle * 0.75f) + (Vector2)obj.transform.position;
        }

        // TODO : 재화 획득 처리
        fragmentAmountQueue.Enqueue(amount);
        if (coroutine == null)
        {
            coroutine = StartCoroutine(IncreaseGoods());
        }
    }

    public IEnumerator IncreaseGoods()
    {
        yield return waitIncreaseFragment;

        current = GameManager.Instance.Player.playerBase.FragmentAmount;
        target = current;

        while (fragmentAmountQueue.Count != 0)
        {
            while(fragmentAmountQueue.Count != 0)
            {
                target += fragmentAmountQueue.Dequeue();
            }

            offset = (target - current) / duration;

            while (current < target)
            {
                current += offset * Time.deltaTime;

                GameManager.Instance.Player.playerBase.FragmentAmount = (int)current;

                yield return waitForEndOfFrame;
            }

            current = target;

            GameManager.Instance.Player.playerBase.FragmentAmount = (int)current;
        }

        coroutine = null;
        yield break;
    }

}
