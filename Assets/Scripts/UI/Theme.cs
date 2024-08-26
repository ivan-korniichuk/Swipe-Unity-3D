using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Theme : MonoBehaviour
{
    [SerializeField] private Color _cameraColor;
    [SerializeField] private Color _materialColor;
    [SerializeField] private Color _textColor;
    [SerializeField] private Color _imageColor;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private int _price;
    [SerializeField] private bool _isBought = false;//
    [SerializeField] private GameObject _lock;

    //private bool _isBought = false;
    private Button _button;

    public UnityAction<Theme> ButtonClicked;
    public int Price => _price;
    public bool IsBought => _isBought;
    public Color CameraColor => _cameraColor;
    public Color MaterialColor => _materialColor;
    public Color TextColor => _textColor;
    public Color ImageColor => _imageColor;
    public ParticleSystem Particle => _particle;

    private void Awake()
    {
        _button = GetComponent<Button>();
        if (_isBought)
        {
            _lock.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        ButtonClicked?.Invoke(this);
    }

    public bool TrySellTheme()
    {
        if (Player.BuyTheme(this))
        {
            _isBought = true;
            _lock.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
