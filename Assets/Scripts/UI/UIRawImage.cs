using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIRawImage : UIComponent
{
    private RawImage _image;

    private void Awake()
    {
        _image = GetComponent<RawImage>();

        MainColor = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a);
        AColor = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }

    public override void TurnOff(float time = 0)
    {
        StopChange();
        if (time == 0)
        {
            _image.color = AColor;
            return;
        }

        ImageChangeCoroutine = StartCoroutine(ChangeColorCoroutine(_image, AColor, time));

        IsOn = false;
        if (TryGetComponent(out Button button))
        {
            button.enabled = false;
        }
    }

    public override void TurnOn(float time = 0)
    {
        StopChange();
        if (time == 0)
        {
            _image.color = MainColor;
            return;
        }

        if (TryGetComponent(out Button button))
        {
            button.enabled = true;
        }
        ImageChangeCoroutine = StartCoroutine(ChangeColorCoroutine(_image, MainColor, time));
        IsOn = true;
    }
}
