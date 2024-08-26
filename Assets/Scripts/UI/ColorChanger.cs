using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Material _fogMaterial;
    [SerializeField] private Theme _mainTheme;
    [SerializeField] private Camera _camera;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private UIImage _colorButton;

    [SerializeField] private List<UIImage> _imageList;

    private List<Theme> _themes;

    private void Awake()
    {
        _themes = transform.GetChild(0).GetComponentsInChildren<Theme>().ToList();

        foreach (var theme in _themes)
        {
            theme.ButtonClicked += OnButtonClicked;
        }
    }

    private void Start()
    {
        SetTheme(_mainTheme);
    }

    private void OnButtonClicked(Theme theme)
    {
        if (theme.IsBought)
        {
            SetTheme(theme);
        }
        else
        {
            if (theme.TrySellTheme())
            {
                SetTheme(theme);
            }
        }
    }

    private void SetTheme(Theme theme)
    {
        if (_mainTheme.Particle)
        {
            _mainTheme.Particle.Stop();
            _mainTheme.Particle.gameObject.SetActive(false);
        }
        _mainTheme = theme;
        if (_mainTheme.Particle)
        {
            _mainTheme.Particle.gameObject.SetActive(true);
            _mainTheme.Particle.Play();
        }

        _camera.backgroundColor = _mainTheme.CameraColor;
        _fogMaterial.color = _mainTheme.CameraColor;
        _material.color = _mainTheme.MaterialColor;
        

        foreach (var text in FindObjectsOfType<UIText>())
        {
            text.ChangeColor(_mainTheme.TextColor);
        }

        foreach (var image in _imageList)
        {
            image.ChangeColor(_mainTheme.ImageColor);
        }

        _colorButton.ChangeColor(_mainTheme.MaterialColor);
    }
}
