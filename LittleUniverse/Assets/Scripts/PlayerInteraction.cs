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
    private float radius = 1;
    private bool canChop = true;
    public GameObject cube;

    [SerializeField] Collider[] colliders;
    [SerializeField] GameObject axe;
    [SerializeField] private Transform playerVisual;
    [Space(10)]
    [SerializeField] Transform _canvas;
    [SerializeField] GameObject collectablePopUpPrefab;

    List<IChopable> allChopable = new List<IChopable>();
    public void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        axe.SetActive(false);
    }

    public void Update()
    {
        //transform.LookAt(cube.transform.position);

        colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var item in colliders)
        {
            if (item.GetComponent<IChopable>() != null)
            {
                var chopable = item.GetComponent<IChopable>();
                if (chopable.IsChopable())
                {
                    transform.LookAt(new Vector3(item.transform.position.x, transform.position.y, item.transform.position.z));
                }
                if (chopable.IsChopable() && canChop)
                {
                    axe.SetActive(true);
                    playerMovement.isChoping = true;
                    canChop = false;
                    StartCoroutine(StartChopping(chopable));
                }

            }
            break;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            //SwitchRunTimeAnimatorController
            if (playerAnimation.animator.runtimeAnimatorController != playerAnimation.animatorControllerUperWater)
            {
                SetPlayerVisual(-1.4f);
                playerAnimation.SwitchController(playerAnimation.animatorControllerUperWater);
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
        playerVisual.localPosition = new Vector3(playerVisual.localPosition.x,
                                                 yValue,
                                                 playerVisual.localPosition.z);
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
        ShowCollectablePopUp();
        yield return new WaitForSeconds(.25f);
        canChop = true;
        axe.SetActive(false);
        playerMovement.isChoping = false;
    }

    public void ShowCollectablePopUp()
    {
        Vector3 pos = new Vector3(Random.Range(-100, 100), 150, 0);
        GameObject popUpIns = Instantiate(collectablePopUpPrefab, _canvas);
        popUpIns.GetComponent<CollectablePopUp>().ShowPopUp();
    }
}
