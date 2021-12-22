using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class LandUnlocker : MonoBehaviour
{
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
            canUnlock = false;
            FlyCollectableAnim();
        }
    }

    private void FlyCollectableAnim()
    {
        GameObject collectableIns = Instantiate(collectablePrefab, player.transform.position, Quaternion.identity);
        collectableIns.transform.DOJump(collectableJumpTf.position, 3, 1, .25f).OnComplete(delegate
        {
            canUnlock = true;
            Destroy(collectableIns);
            unlockResAmount--;
            if (unlockResAmount <= 0)
            {
                //Land Unlocked
                landToUnlock.SetActive(true);
                this.gameObject.SetActive(false);
            }
        });

    }
}
