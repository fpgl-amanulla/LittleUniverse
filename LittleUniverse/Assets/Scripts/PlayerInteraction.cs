using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private PlayerMovement playerMovement;
    private float radius = 1.5f;
    private bool canChop = true;

    [SerializeField] Collider[] colliders;
    [SerializeField] GameObject axe;
    [SerializeField] private Transform playerVisual;

    [Space(10)]
    [SerializeField] Transform _canvas;
    [SerializeField] GameObject collectablePopUpPrefab;
    [SerializeField] List<GameObject> allChopable = new List<GameObject>();

    int startIndex = 0;
    public void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        axe.SetActive(false);
    }

    public void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);

        allChopable.Clear();

        foreach (var item in colliders)
        {
            IChopable chopableItem = item.GetComponent<IChopable>();
            if (chopableItem != null)
            {
                //if (!allChopable.Contains(item.gameObject))
                //    allChopable.Remove(item.gameObject);
                //else
                allChopable.Add(item.gameObject);
            }
        }

        if (allChopable.Count > 0)
        {
            var chopable = allChopable[startIndex].GetComponent<IChopable>();
            if (chopable.IsChopable())
            {
                transform.LookAt(new Vector3(chopable.GetObjTransForm().position.x,
                                             transform.position.y,
                                             chopable.GetObjTransForm().transform.position.z));
            }
            if (chopable.IsChopable() && canChop)
            {
                axe.SetActive(true);
                playerMovement.isChoping = true;
                canChop = false;
                StartCoroutine(StartChopping());
            }
        }
    }

    private IEnumerator StartChopping()
    {
        playerAnimation.PlayAttackAnim();
        yield return new WaitForSeconds(.15f);
        foreach (var chopable in allChopable)
        {
            chopable.GetComponent<IChopable>().Chop(this.gameObject);
        }
        ShowCollectablePopUp();
        yield return new WaitForSeconds(.25f);
        canChop = true;
        axe.SetActive(false);
        playerMovement.isChoping = false;
    }

    private void ShowCollectablePopUp()
    {
        Vector3 pos = new Vector3(Random.Range(-100, 100), 150, 0);
        GameObject popUpIns = Instantiate(collectablePopUpPrefab, _canvas);
        popUpIns.GetComponent<CollectablePopUp>().ShowPopUp();
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            //SwitchRunTimeAnimatorController
            if (playerAnimation.animator.runtimeAnimatorController != playerAnimation.animatorControllerUpperWater)
            {
                SetPlayerVisual(-1.4f);
                playerAnimation.SwitchController(playerAnimation.animatorControllerUpperWater);
            }
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            //SwitchRunTimeAnimatorController
            if (playerAnimation.animator.runtimeAnimatorController != playerAnimation.animatorControllerGround)
            {
                SetPlayerVisual(-1.15f);
                playerAnimation.SwitchController(playerAnimation.animatorControllerGround);
            }
        }
    }

    public bool IsInGround() => playerAnimation.animator.runtimeAnimatorController == playerAnimation.animatorControllerGround;

    private void SetPlayerVisual(float yValue)
    {
        var localPosition = playerVisual.localPosition;
        localPosition = new Vector3(localPosition.x,
                                                 yValue,
                                                 localPosition.z);
        playerVisual.localPosition = localPosition;
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
}
