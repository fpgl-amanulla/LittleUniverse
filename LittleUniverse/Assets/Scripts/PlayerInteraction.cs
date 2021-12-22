using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private float radius = 1;
    private bool canChop = true;

    [SerializeField] Collider[] colliders;

    public void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var item in colliders)
        {
            if (item.GetComponent<IChopable>() != null)
            {
                var chopable = item.GetComponent<IChopable>();

                Quaternion rotation = Quaternion.LookRotation(item.transform.position - transform.position);
                //rotation.Set(0, rotation.y, 0, 0);
                transform.DOLocalRotateQuaternion(rotation, .25f);

                if (chopable.IsChopable() && canChop)
                {
                    canChop = false;
                    StartCoroutine(StartChopping(chopable));
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<LandUnlocker>())
        {
            var landUnlocker = other.GetComponent<LandUnlocker>();
            landUnlocker.UnlockLand(this.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {

    }

    private IEnumerator StartChopping(IChopable chopable)
    {
        playerAnimation.PlayAttackAnim();
        yield return new WaitForSeconds(.15f);
        chopable.Chop(this.gameObject);
        yield return new WaitForSeconds(.25f);
        canChop = true;
    }
}
