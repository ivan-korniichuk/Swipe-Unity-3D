using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondsView : ValueView
{
    private void Start()
    {
        OnValueChanged(Player.Diamonds);
    }

    private void OnEnable()
    {
        Player.DiamondsChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        Player.DiamondsChanged -= OnValueChanged;
    }
}
