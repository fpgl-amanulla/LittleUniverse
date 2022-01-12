using UnityEngine;

public interface IChopable
{
    Transform GetObjTransForm();
    void Chop(GameObject player = null);
    bool IsChopable();
}