using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerFootStep : MonoBehaviour
{
    public Transform leftFootTr;
    public Transform rightFootTr;

    [FormerlySerializedAs("footSetpEffect")] 
    public GameObject footStepEffect;

    public void LeftFootStep() => GenerateFootPrint(leftFootTr);
    public void RightFootStep() => GenerateFootPrint(rightFootTr);

    private void GenerateFootPrint(Transform footTr)
    {
        Vector3 footPos = footTr.position;
        GameObject footStepIns = Instantiate(footStepEffect, footPos, Quaternion.identity, footTr);
        footTr.transform.DetachChildren();
        Destroy(footStepIns, 1.0f);
    }
}
