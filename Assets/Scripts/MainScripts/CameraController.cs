using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _startposition;
    [SerializeField] private Player _player = null;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _timeOfMovenent = 1;

    private Vector3 _position;
    private Coroutine _coroutine;

    private void Update()
    {
        if (_player && (Game.LevelStarted || _position != _player.transform.position + _startposition))
        {
            _position = _player.transform.position + _startposition;
            _position.y = _startposition.y;
            _camera.transform.position = _position;

            //_camera.transform.position = _player.transform.position + _startposition;
        }
    }

    public void ChangePlayer(Player player)
    {
        _player = player;
    }

    public void SetPosition(Vector3 position)
    {
        position += _startposition;
        position.y = _startposition.y;
        _camera.transform.position =  position;
    }

    private IEnumerator MoveCameraCoroutine()
    {
        float timePassed = 0;
        float distance = (_position - _camera.transform.position).magnitude;
        Vector3 direction = (_position - _camera.transform.position).normalized;

        while (timePassed < _timeOfMovenent)
        {
            float deltaTime = timePassed + Time.deltaTime > _timeOfMovenent ? _timeOfMovenent - timePassed : Time.deltaTime;
            float speed = distance / _timeOfMovenent;

            transform.position += deltaTime * speed * direction;
            timePassed += deltaTime;

            yield return new WaitForEndOfFrame();
        }
        _camera.transform.position = _position;
        yield return true;
    }
}
