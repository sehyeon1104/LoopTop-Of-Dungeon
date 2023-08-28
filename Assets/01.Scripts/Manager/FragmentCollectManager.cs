using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentCollectManager : MonoSingleton<FragmentCollectManager>
{
    private GameObject fragmentCollect;
    private Poolable fragmentObj;

    // �������� ȹ�淮�� ����� ť
    private Queue<int> fragmentAmountQueue = new Queue<int>();
    // ť ���� ���� �� ������ target
    private float target;

    // ī���ÿ� �ɸ��� �ð� ����. 
    [SerializeField]
    private float duration = 0.5f;
    // ������
    private float offset;
    // ���� ������
    private float current;

    private Coroutine coroutine = null;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private WaitForSeconds waitIncreaseFragment = new WaitForSeconds(1.5f);

    private bool isAddAll = false;

    private int oneTimeIncrease = 1;
    public int OneTimeIncrease {
        get
        {
            return oneTimeIncrease;
        }
        set 
        {
            if(value < 5)
            {
                oneTimeIncrease = value; 
            }
        } 
    }


    private void Awake()
    {
        fragmentCollect = Managers.Resource.Load<GameObject>("Assets/03.Prefabs/2D/Fragment.prefab");
    }

    private void Start()
    {
        Managers.Pool.CreatePool(fragmentCollect, 200);
    }

    public void DropFragment(GameObject obj, int amount, int count = 3)
    {
        for(int i = 0; i < count; ++i)
        {
            fragmentObj = Managers.Pool.Pop(fragmentCollect);
            fragmentObj.transform.position = obj.transform.position;
        }

        fragmentAmountQueue.Enqueue(Mathf.RoundToInt(amount * GameManager.Instance.Player.playerBase.FragmentAddAcq * oneTimeIncrease));
        oneTimeIncrease = 1;
    }

    public void DropFragmentByCircle(GameObject obj, int amount, int count = 3)
    {
        for (int i = 0; i < count; ++i)
        {

            fragmentObj = Managers.Pool.Pop(fragmentCollect);
            fragmentObj.transform.position = (Random.insideUnitCircle * 0.75f) + (Vector2)obj.transform.position;
            if(amount < 0)
                fragmentObj.GetComponent<Fragment>().SetIsDecrease(true);
        }

        fragmentAmountQueue.Enqueue(Mathf.RoundToInt(amount * GameManager.Instance.Player.playerBase.FragmentAddAcq * oneTimeIncrease));
        oneTimeIncrease = 1;
    }

    public void IncreaseGoods()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(IEIncreaseGoods());
        }
    }

    public IEnumerator IEIncreaseGoods()
    {
        current = GameManager.Instance.Player.playerBase.FragmentAmount;
        target = current;

        while (fragmentAmountQueue.Count != 0)
        {
            if (fragmentAmountQueue.Count >= 10)
                isAddAll = true;

            while (fragmentAmountQueue.Count != 0)
            {
                if (fragmentAmountQueue.Peek() == 0 && !isAddAll)
                {
                    target += fragmentAmountQueue.Dequeue();
                    break;
                }

                target += fragmentAmountQueue.Dequeue();
            }

            isAddAll = false;

            offset = (target - current) / duration;

            if(offset > 0)
            {
                while (current < target)
                {
                    current += offset * Time.deltaTime;

                    GameManager.Instance.Player.playerBase.FragmentAmount = (int)current;

                    yield return waitForEndOfFrame;
                }
            }
            else if(offset < 0)
            {
                while (target < current)
                {
                    current += offset * Time.deltaTime;

                    GameManager.Instance.Player.playerBase.FragmentAmount = (int)current;

                    yield return waitForEndOfFrame;
                }
            }

            current = target;

            GameManager.Instance.Player.playerBase.FragmentAmount = (int)current;
        }

        coroutine = null;
        yield break;
    }

}
