using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    const string Run_Key = "Run";
    const string Attack_Key = "Attack";

    public void PlayRunAnim(bool status)
    {
        animator.SetBool(Run_Key, status);
    }

    public void PlayAttackAnim()
    {
        animator.SetTrigger(Attack_Key);
    }
}
