using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetItemList
{
    public enum SetItem
    {
        Default = 0,
        CompleteHourglass,
        MirrorOfDawn,
        EqualExchange,
        Overeager
    }

    public enum CompleteHourglass
    {
        BrokenHourglass,
        RollingHourglass
    }

    public enum MirrorOfDawn
    {
        MirrorOfSun,
        MirrorOfMoon
    }

    public enum EqualExchange
    {
        MidasTouch,
        InvisibleHand
    }

    public enum Overeager
    {
        GiantGlove,
        MagicBean,
        HeavenHarp
    }
}
