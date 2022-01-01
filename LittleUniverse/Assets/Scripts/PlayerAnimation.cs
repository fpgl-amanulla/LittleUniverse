using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    public RuntimeAnimatorController animatorControllerGround;
    [FormerlySerializedAs("animatorControllerUperWater")] 
    public RuntimeAnimatorController animatorControllerUpperWater;

    public Animator animator;

    const string Run_Key = "Run";
    const string Attack_Key = "Attack";

    public void Start() => SwitchController(animatorControllerGround);

    public void SwitchController(RuntimeAnimatorController _runtimeAnimatorController) => animator.runtimeAnimatorController = _runtimeAnimatorController;

    public void PlayRunAnim(bool status) => animator.SetBool(Run_Key, status);

    public void PlayAttackAnim() => animator.SetTrigger(Attack_Key);
}
