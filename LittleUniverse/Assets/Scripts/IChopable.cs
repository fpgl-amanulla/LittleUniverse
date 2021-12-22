using System;
using UnityEngine;

public interface IChopable
{
    void Chop(GameObject player = null);
    bool IsChopable();
}
