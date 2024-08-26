using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class HPView : ValueView
{
    private Timer _timer;
    private string _text = "";
    private int _hp = 0;

    private void Start()
    {
        _hp = Player.Hp;
        base.OnValueChanged(_hp + " " + _text);
    }

    private void OnEnable()
    {
        _timer = GetComponent<Timer>();
        Player.HpChanged += OnValueChanged;
        _timer.TimerChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        Player.HpChanged -= OnValueChanged;
        _timer.TimerChanged -= OnValueChanged;
    }

    private new void OnValueChanged(int hp)
    {
        _hp = hp;
        base.OnValueChanged(_hp + " " + _text);
    }

    private new void OnValueChanged(string text)
    {
        _text = text;
        base.OnValueChanged(_hp + " " + _text);
    }
}
