using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongBlock : Block
{
    [SerializeField] private int _height;

    public override void PrepareBlock()
    {
        LocalPosition = transform.position;
        StartBlock();
        StartPosition += transform.position;
        Off(false);
    }

    protected void StartBlock()
    {
        Position = new Vector3(transform.position.x, transform.position.y - _height / 2 + 0.5f, transform.position.z);
        transform.localScale = new Vector3(1, _height, 1);
        transform.position = Position;
    }
}
