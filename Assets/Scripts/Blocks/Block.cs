using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Block : MonoBehaviour
{
    [SerializeField] protected Color Color;
    [SerializeField] protected string Type;
    [SerializeField] protected Vector3 StartPosition = new Vector3(0, -40, 0);
    [SerializeField] protected AnimationCurve _speedCurve;

    protected IEnumerator MoveCoroutine;
    private IEnumerator _settingCoroutine;

    protected bool CoroutineIsActive = false;

    public Vector3 Position { get; protected set; }
    public Vector3 LocalPosition { get; protected set; }
    public bool IsActive { get; protected set; } = false;
    public bool Started { get; protected set; } = false;
    public string BlockType => Type;

    public UnityAction PositionChanged;

    private void OnDisable()
    {
        TryStopMoveBlockCoroutine();
    }

    public virtual void PrepareBlock()
    {
        LocalPosition = transform.position;
        Position = transform.position;
        StartPosition += transform.position;
        //Debug.Log("LocalPosition " + LocalPosition + " Position " + Position + " StartPosition " + StartPosition);
        Off(false);
    }

    //virtual
    public void RepositionBlock(Vector3 distanceToNewPosition)
    {
        LocalPosition += distanceToNewPosition;
        Position += distanceToNewPosition;
        StartPosition += distanceToNewPosition;
    }

    public void StartBlock(float time)
    {
        Off();
        SetBlock(time);
    }

    public void Off(bool setActive = true) //must be checked again
    {
        transform.position = StartPosition;
        gameObject.SetActive(setActive);
        IsActive = false;
        Started = false;
    }

    public void Set() //must be checked again
    {
        transform.position = Position;
        IsActive = true;
        Started = true;
    }

    public void OffBlock(float time)
    {
        if (IsActive)
        {
            TryStopMoveBlockCoroutine();

            _settingCoroutine = OffBlockCoroutine(time);
            StartCoroutine(_settingCoroutine);
            IsActive = false;
        }
    }

    public void SetBlock(float time)
    {
        if (!IsActive)
        {
            TryStopMoveBlockCoroutine();

            _settingCoroutine = SetBlockCoroutine(time);
            StartCoroutine(_settingCoroutine);
            IsActive = true;
        }
    }

    private void TryStopMoveBlockCoroutine()
    {
        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
            CoroutineIsActive = false;
        }

        if(_settingCoroutine != null)
        {
            StopCoroutine(_settingCoroutine);
        }
    }

    private IEnumerator OffBlockCoroutine(float time)
    {
        if (transform.position != StartPosition && !CoroutineIsActive && IsActive)
        {
            yield return new WaitUntil(() => !CoroutineIsActive);
            MoveCoroutine = TryMoveBlockCorutine(StartPosition, time);
            StartCoroutine(MoveCoroutine);

            yield return new WaitUntil(() => !CoroutineIsActive);
            gameObject.SetActive(false);
            Started = false;
        }
    }

    private IEnumerator SetBlockCoroutine(float time)
    {
        if (transform.position != Position && !CoroutineIsActive && !IsActive)
        {
            yield return new WaitUntil(() => !CoroutineIsActive);
            MoveCoroutine = TryMoveBlockCorutine(Position, time);
            StartCoroutine(MoveCoroutine);

            yield return new WaitUntil(() => !CoroutineIsActive);
            Started = true;
            PositionChanged?.Invoke();
        }
    }

    //protected void TryMoveBlock(Vector3 newPosition, float time)
    //{
    //    StartCoroutine(TryMoveBlockCorutine(newPosition, time));
    //}

    protected IEnumerator TryMoveBlockCorutine(Vector3 newPosition, float time)
    {
        if (CoroutineIsActive)
        {
            Debug.LogError(name + " TryMoveBlockCorutine is still active " + newPosition);
            StopCoroutine(MoveCoroutine);
            CoroutineIsActive = false;
        }

        if (!CoroutineIsActive)
        {
            CoroutineIsActive = true;

            float timePassed = 0;
            float distance = (newPosition - transform.position).magnitude;
            Vector3 direction = (newPosition - transform.position).normalized;
            float speedMultiplier = CountSpeedMultiplierForCurve(_speedCurve, time, distance, 300);
            int precision = 30;

            float speed = 0;
            while (timePassed < time)
            {
                float deltaTime = timePassed + Time.deltaTime > time ? time - timePassed : Time.deltaTime;

                for (int i = 0; i < precision; i++)
                {
                    speed = (_speedCurve.Evaluate((timePassed + deltaTime * i / precision) / time) + _speedCurve.Evaluate((timePassed + deltaTime * (i + 1) / precision) / time)) / 2;
                    transform.position += speed * speedMultiplier * deltaTime / precision * direction;
                }
                timePassed += deltaTime;

                yield return new WaitForEndOfFrame();
            }
            transform.position = newPosition;
            CoroutineIsActive = false;
        }
    }

    //
    private float CountSpeedMultiplierForCurve(AnimationCurve curve, float time = 1, float distance = 1, int precision = 250)
    {
        float area = 0;
        Keyframe K1 = curve[0];
        Keyframe K2 = curve[1];

        float xValue = K2.time - K1.time;
        float deltaTime = xValue / precision;
        float startTime = deltaTime / 2 + K1.time;

        for (int i = 0; i < precision; i++)
        {
            float position = startTime + deltaTime * i;
            area += curve.Evaluate(position) * deltaTime * time;
        }
        return distance / area;
    }
}
