using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemEffectManager : MonoSingleton<UseItemEffectManager>
{


    public void HPRelatedItemEffects()
    {
        // �������� ��
        ItemEffects.Items[10].Use();
    }

}
