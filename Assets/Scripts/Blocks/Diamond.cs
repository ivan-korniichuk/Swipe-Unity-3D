using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Block
{
    private bool _isPickedUp = false;

    public bool IsPickedUp { get => _isPickedUp; }

    public void PickUp()
    {
        _isPickedUp = true;
        Off(false);
    }

    public new void SetBlock(float time)
    {
        if (!_isPickedUp)
        {
            base.SetBlock(time);
        }
    }
}
