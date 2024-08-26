using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class UIComponent : MonoBehaviour
{
    protected Color AColor;
    protected Color MainColor;
    protected bool IsOn = true;
    protected Coroutine ImageChangeCoroutine;

    private bool _coroutineIsRunning = false;

    public abstract void TurnOn(float time = 0);

    public abstract void TurnOff(float time = 0);

    public void ChangeColor(Color mainClolor)
    {
        MainColor = mainClolor;
        AColor = new Color(mainClolor.r, mainClolor.g, mainClolor.b, 0);

        if (IsOn)
            TurnOn();
        else
            TurnOff();
    }
    
    protected IEnumerator ChangeColorCoroutine(Graphic UIElement, Color color, float time)
    {
        _coroutineIsRunning = true;
        Color startColor = UIElement.color;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            UIElement.color = Color.Lerp(startColor, color, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UIElement.color = color;
        _coroutineIsRunning = false;
    }

    protected void StopChange()
    {
        if (_coroutineIsRunning)
        {
            StopCoroutine(ImageChangeCoroutine);
            _coroutineIsRunning = false;
        }
    }
}
