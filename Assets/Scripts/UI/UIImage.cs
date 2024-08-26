using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImage : UIComponent
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();

        MainColor = new Color(_image.color.r, _image.color.g, _image.color.b, _image.color.a);
        AColor = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
    }

    public override void TurnOff(float time = 0)
    {
        StopChange();
        IsOn = false;
        if (time == 0)
        {
            _image.color = AColor;
            return;
        }

        ImageChangeCoroutine = StartCoroutine(ChangeColorCoroutine(_image, AColor, time));

        if (TryGetComponent(out Button button))
        {
            button.enabled = false;
        }
    }

    public override void TurnOn(float time = 0)
    {
        StopChange();
        IsOn = true;
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
    }
}
