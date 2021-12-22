using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class Chopable : MonoBehaviour, IChopable
{
    public List<GameObject> Parts = new List<GameObject>();

    [SerializeField] int index = 0;

    private GameObject player;

    public virtual void Chop(GameObject _player)
    {
        if (_player) player = _player;
        Parts[index].SetActive(false);
        index++;
    }

    public bool IsChopable()
    {
        return index < Parts.Count;
    }

    public void CollectCollectable(GameObject collectable)
    {
        if (player == null) return; //Guard Clause
        if (!IsChopable()) return;

        GameObject collectableIns = Instantiate(collectable, Parts[index - 1].transform.position, collectable.transform.rotation);

        collectableIns.transform.DOJump(player.transform.position, 3, 1, .5f).OnComplete(delegate
        {
            Destroy(collectableIns);
        });
    }
}
