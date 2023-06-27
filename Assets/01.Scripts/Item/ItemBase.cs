using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class ItemBase : MonoBehaviour
{
    public abstract Define.ItemType itemType { get; }
    public abstract Define.ItemRating itemRating { get; }
    public abstract bool isPersitantItem { get; }
    public abstract void Init();
    public abstract void Use();
    public abstract void Disabling();
    public virtual void LastingEffect()
    {

    }
    public virtual bool isOneOff { get; } = false;

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
    };
}
