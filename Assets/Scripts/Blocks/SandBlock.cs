using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SandBlock : MovableBlock
{
    private RaycastHit _hit;

    private void Update()
    {
        if (!Game.LevelStarted)
            return;

        if (Setted)
        {
            Physics.Raycast(transform.position, Vector3.up, out _hit, 1);

            if (_hit.collider)
            { 
                if (!_hit.collider.TryGetComponent(out SandBlock _) && !_hit.collider.TryGetComponent(out Diamond _))
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.layer = 8;
                    Setted = false;
                    LocalPosition = transform.position + Vector3.down;
                    if (_hit.collider.TryGetComponent(out Player player))
                    {
                        player.Fall();
                    }
                }
            }
        }
        else if (!GetComponent<Rigidbody>().isKinematic && transform.position.y < StartPosition.y)
        {
            Off(false);
        }
    }

    public override void Restart(float time = 0)
    {
        Debug.Log("Restart");
        if (LocalPosition != Position)
        {
            Debug.Log("Restart true");
            //SetBlock(time);
            //base.IsActive = true;

            if (time > 0)
            {
                StartCoroutine(ResetBlock(time));
            }
            else
            {
                LocalPosition = Position;
                transform.position = Position;
            }

            GetComponent<Rigidbody>().isKinematic = true;

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
}
