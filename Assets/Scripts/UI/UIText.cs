using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class UIText : UIComponent
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        MainColor = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a);
        AColor = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
    }

    public override void TurnOff(float time = 0)
    {
        StopChange();
        ImageChangeCoroutine = StartCoroutine(ChangeColorCoroutine(_text, AColor, time));
        IsOn = false;
    }

    public override void TurnOn(float time = 0)
    {
        StopChange();
        ImageChangeCoroutine = StartCoroutine(ChangeColorCoroutine(_text, MainColor, time));
        IsOn = true;
    }
}
