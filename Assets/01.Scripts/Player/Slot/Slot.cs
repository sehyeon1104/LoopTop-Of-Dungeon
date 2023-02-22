using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoSingleton<Slot>
{
    private void Start()
    {
        _maxSlotLevel = 10;
    }

    private int _slotLevel;

    public int SlotLevel
    {
        get
        {
            return _slotLevel;
        }
        set
        {
            _slotLevel = value;
            if(_slotLevel > MaxSlotLevel)
            {
                _slotLevel = MaxSlotLevel;
            }


        }
    }

    private int _maxSlotLevel;
    public int MaxSlotLevel { get { return _maxSlotLevel; } }
}
