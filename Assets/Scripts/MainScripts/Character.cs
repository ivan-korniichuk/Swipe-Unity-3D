using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Character : Block
{
    [SerializeField] private float _jumpTime;
    [SerializeField] private LayerMask _staticBlocks;
    [SerializeField] private LayerMask _movableBlocks;

    protected bool IsMoving = false;
    protected bool IsFalling = false;

    public static UnityAction ActionEnded;

    public void Fall(bool isFalling = true)
    {
        IsFalling = isFalling;
        GetComponent<Rigidbody>().isKinematic = !isFalling;
    }

    protected void TryMove(Vector3 position, float time)
    {
        StartCoroutine(TryMoveCorutine(position, time));
    }

    private IEnumerator TryMoveCorutine(Vector3 position, float time)
    {
        if (!CoroutineIsActive)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, position, out hit, 1, _movableBlocks);
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent(out PortableBlock portableBlock))
                {
                    if (portableBlock.TryMove(position, time))
                    {
                        //yield return StartCoroutine(TryMoveBlockCorutine(transform.position + position, time));
                        MoveCoroutine = TryMoveBlockCorutine(transform.position + position, time);
                        StartCoroutine(MoveCoroutine);
                        yield return new WaitUntil(() => !CoroutineIsActive);
                    }
                }
            }
            else if (!Physics.Raycast(transform.position, position, 1, _staticBlocks))
            {
                //yield return StartCoroutine(TryMoveBlockCorutine(transform.position + position, time));
                MoveCoroutine = TryMoveBlockCorutine(transform.position + position, time);
                StartCoroutine(MoveCoroutine);
                yield return new WaitUntil(() => !CoroutineIsActive);
            }

            ActionEnded?.Invoke();
        }
    }
}