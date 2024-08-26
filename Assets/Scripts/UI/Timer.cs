using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _minutes;

    private TimeSpan _setedTime;
    private TimeSpan _timeLeft;
    private bool _setted = false;
    private Coroutine _coroutine;

    public UnityAction<string> TimerChanged;

    private void OnEnable()
    {
        Player.HpChanged += TrySetTimer;
    }

    private void OnDisable()
    {
        Player.HpChanged -= TrySetTimer;
    }

    public void TrySetTimer(int hp)
    {
        if (hp < Player.LimitHp && !_setted)
        {
            _setedTime = TimeSpan.FromTicks(DateTime.Now.Ticks);
            _timeLeft = TimeSpan.FromMinutes(_minutes);
            _setted = true;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(TimeCorutine());
        }
    }

    private IEnumerator TimeCorutine()
    {
        TimerChanged?.Invoke(String.Format(" {0:d2}:{1:d2}", _timeLeft.Minutes, _timeLeft.Seconds));

        while (true)
        {
            yield return new WaitForSeconds(1);
            _timeLeft -= TimeSpan.FromSeconds(1);
            TimerChanged?.Invoke(String.Format(" {0:d2}:{1:d2}", _timeLeft.Minutes, _timeLeft.Seconds));
            if(_timeLeft <= TimeSpan.Zero)
            {
                TimerChanged?.Invoke("");
                _setted = false;
                StopCoroutine(_coroutine);
                Player.GiveLifes(1);
            }
        }
    }
}
