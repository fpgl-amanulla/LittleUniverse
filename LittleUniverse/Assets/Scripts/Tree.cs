using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Choppable
{
    public GameObject collectablePrefab;
    public override void Chop(GameObject _player)
    {
        base.Chop(_player);
        CollectCollectable(collectablePrefab);
    }
}
