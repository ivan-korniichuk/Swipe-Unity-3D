using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortableBlock : MovableBlock
{
    [SerializeField] private LayerMask _staticBlocks;

    private void OnEnable()
    {
        Player.ActionEnded += TrySet;
    }

    private void OnDisable()
    {
        Player.ActionEnded -= TrySet;
    }

    private void TrySet()
    {
        if (!Game.LevelStarted )
            return;

        if (!Setted)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 1);
            if (!hit.collider || hit.collider.TryGetComponent(out SandBlock _))
            {
                if (transform.position.y <= 1)
                {
                    gameObject.layer = 7;
                    Setted = true;
                }
                TryMove(Vector3.down, 0.2f);
            }
        }
    }

    public bool TryMove(Vector3 position, float time, float distance = 1)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, position, out hit, distance);

        if (!CoroutineIsActive && (!hit.collider || hit.collider.TryGetComponent(out SandBlock _)))
        {
            //restart errors might appear because of this
            MoveCoroutine = TryMoveBlockCorutine(transform.position + position, time);
            StartCoroutine(MoveCoroutine);
            LocalPosition += position;
            return true;
        }
        return false;
    }
}
