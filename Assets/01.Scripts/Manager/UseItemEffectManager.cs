using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemEffectManager : MonoSingleton<UseItemEffectManager>
{


    public void HPRelatedItemEffects()
    {
        // 광전사의 검
        ItemEffects.Items[10].Use();
    }

}
