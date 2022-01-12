using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public abstract class Choppable : MonoBehaviour, IChopable
{
    public List<GameObject> Parts = new List<GameObject>();
    [SerializeField] int index = 0;

    private Collider objCollider;
    private GameObject player;
    private bool isGenerated = false;

    public void Start() => objCollider = GetComponent<Collider>();

    public virtual void Chop(GameObject _player)
    {
        if (_player) player = _player;
        if (!IsChopable()) return;
        Parts[index].SetActive(false);
        index++;
        Shake();
    }

    public bool IsChopable()
    {
        if (index < Parts.Count || isGenerated) return index < Parts.Count;
        objCollider.enabled = false;
        isGenerated = true;
        StartCoroutine(Regenerate(5f));
        return index < Parts.Count;
    }

    public Transform GetObjTransForm() => this.transform;

    protected void CollectCollectable(GameObject collectable)
    {
        if (player == null) return; //Guard Clause
        if (!IsChopable()) return;

        GameObject collectableIns =
            Instantiate(collectable, Parts[index - 1].transform.position, collectable.transform.rotation);

        collectableIns.transform.DOJump(player.transform.position, 6, 1, .8f).SetEase(Ease.OutSine)
            .OnComplete(delegate { Destroy(collectableIns); });
    }

    private void Shake()
    {
        foreach (GameObject item in Parts.Where(item => item.activeSelf))
            item.transform.DOShakeScale(.15f, .35f);
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

        objCollider.enabled = true;
        isGenerated = false;
        index = 0;
    }
}