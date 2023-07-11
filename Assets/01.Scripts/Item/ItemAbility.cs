using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbility : MonoBehaviour
{
    public class Default : ItemBase
    {
        public override Define.ItemType itemType => Define.ItemType.Default;
        public override Define.ItemRating itemRating => Define.ItemRating.Default;

        public override bool isPersitantItem => false;

        public override void Init()
        {

        }

        public override void Use()
        {

        }

        public override void Disabling()
        {

        }
    }

    public static ItemBase[] Items = new ItemBase[]
    {
        new Default(),      // 0번 아이템 ( 효과x )

        // common
        new WingShoes(),
        new GiantGlove(),
        new DullSword(),

        // rare
        new ChampionBelt(),
        new TornPaper(),
        new InquisitorsRing(),
        new SharpSword(),

        // epic
        new VampireFangs(),
        
        // legendary
        new AllRoundHalfGlove(),
        new BerserkerSword(),

        // ETC
        new SmallHpPotion(),
        new MediumHpPotion(),
        new LargeHpPotion(),
        new ExtraLargeHpPotion(),

        // Special
        new NailedShoes(),
        new CloudyGlasses(),
        new TurtleHat(),
        new CursedRing(),
        new RollingHourglass(),
    };
}