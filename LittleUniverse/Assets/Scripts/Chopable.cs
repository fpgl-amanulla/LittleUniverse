using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class Chopable : MonoBehaviour, IChopable
{
    public List<GameObject> Parts = new List<GameObject>();

    [SerializeField] int index = 0;

    private GameObject player;
    private bool isGenarated = false;

    public virtual void Chop(GameObject _player)
    {
        if (_player) player = _player;
        Parts[index].SetActive(false);
        index++;
        Shake();
    }

    public bool IsChopable()
    {
        if (index >= Parts.Count && !isGenarated)
        {
            isGenarated = true;
            StartCoroutine(Regenerate(5f));
        }
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

    public void Shake()
    {
        foreach (var item in Parts)
        {
            if (item.activeSelf)
            {
                item.transform.DOShakeScale(.15f, .25f);
            }
        }
    }

    [ContextMenu("Regenerate")]
    public IEnumerator Regenerate(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = Parts.Count - 1; i >= 0; i--)
        {
            GameObject item = Parts[i];
            Vector3 originScale = item.transform.localScale;
            item.transform.localScale = Vector3.zero;
            item.SetActive(true);
            item.transform.DOScale(originScale, .25f);
            yield return new WaitForSeconds(.05f);
        }
        isGenarated = false;
        index = 0;
    }
}
