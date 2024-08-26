using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueView : MonoBehaviour
{
    private TMP_Text _textWindow;

    private void Awake()
    {
        _textWindow = GetComponentInChildren<TMP_Text>();
    }

    protected void OnValueChanged(int value)
    {
        _textWindow.text = value.ToString();
    }

    protected void OnValueChanged(string value)
    {
        _textWindow.text = value;
    }
}
