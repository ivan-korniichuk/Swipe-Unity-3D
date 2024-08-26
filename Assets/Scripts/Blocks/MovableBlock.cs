using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : Block
{
    [SerializeField] protected bool Setted;
    [SerializeField] protected bool DefSetted;

    public virtual void Restart(float time = 0)
    {
        if (LocalPosition != Position)
        {
            StartCoroutine(ResetBlock(time));
            Setted = DefSetted;
            if (DefSetted)
            {
                gameObject.layer = 7;
            }
            else
            {
                gameObject.layer = 8;
            }
        }
        PositionChanged?.Invoke();
    }

    protected IEnumerator ResetBlock(float time)
    {
        Started = false;
        IsActive = false;
        //base.IsActive = false;
        if (transform.position != new Vector3(transform.position.x, transform.position.y + StartPosition.y, transform.position.z) && time > 0)
        {
            yield return new WaitUntil(() => !CoroutineIsActive);
            MoveCoroutine = TryMoveBlockCorutine(new Vector3(transform.position.x, StartPosition.y, transform.position.z), time);
            StartCoroutine(MoveCoroutine);
            yield return new WaitUntil(() => !CoroutineIsActive);
        }
        transform.position = StartPosition;
        if (transform.position != Position && time > 0)
        {
            yield return new WaitUntil(() => !CoroutineIsActive);
            MoveCoroutine = TryMoveBlockCorutine(Position, time);
            StartCoroutine(MoveCoroutine);
            yield return new WaitUntil(() => !CoroutineIsActive);
        }

        Started = true;
        IsActive = true;
        LocalPosition = Position;
        transform.position = Position;
        PositionChanged?.Invoke();
    }
}