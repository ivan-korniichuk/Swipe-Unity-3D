using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class VolumeSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite _on;
    [SerializeField] private Sprite _off;
    [SerializeField] private SoundController _soundController;

    private Image _image;
    private bool _isOn = false;

    private void Start()
    {
        _image = GetComponent<Image>();
        Switch();
    }

    public void Switch()
    {
        if (_isOn)
        {
            _image.sprite = _off;
            _isOn = false;
            _soundController.Stop();
        } 
        else 
        {
            _image.sprite = _on;
            _isOn = true;
            _soundController.Play();
        }
    }
}
