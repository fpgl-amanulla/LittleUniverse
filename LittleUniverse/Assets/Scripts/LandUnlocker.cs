using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class LandUnlocker : MonoBehaviour
{
    //public GameObject unlockEffect;
    public GameObject landToUnlock;
    public Transform collectableJumpTf;
    public int unlockResAmount = 20;
    public GameObject collectablePrefab;

    private GameObject player;
    private bool canUnlock = true;

    public void UnlockLand(GameObject _player)
    {

        player = _player;

        if (canUnlock)
        {
            //canUnlock = false;
            StartCoroutine(FlyCollectableAnim());
        }
    }

    private IEnumerator FlyCollectableAnim()
    {
        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            GameObject collectableIns = Instantiate(collectablePrefab, player.transform.position, Quaternion.identity);
            Vector3 pos = Random.insideUnitSphere + (Vector3.up * 4) + transform.position;
            collectableIns.transform.DOMove(pos, .25f).OnComplete(delegate
                   {

                       collectableIns.transform.DOMove(collectableJumpTf.position, .5f).OnComplete(delegate
                        {
                            Destroy(collectableIns);
                            unlockResAmount--;
                            //canUnlock = true;
                            if (unlockResAmount <= 0)
                            {
                                //Land Unlocked
                                //Instantiate(unlockEffect, landToUnlock.transform.localPosition, Quaternion.identity);
                                landToUnlock.SetActive(true);
                                this.gameObject.SetActive(false);
                            }
                        });
                   });
            yield return new WaitForSeconds(.1f);
        }
    }
}
