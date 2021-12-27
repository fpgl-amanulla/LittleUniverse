using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private PlayerInteraction playerInteraction;

    [Space(10)]
    public float moveSpeed;
    public float turnSmoothTime = .1f;
    private float turnSmoothVelocity;
    [HideInInspector] public bool isChoping = false;

    [Space(10)]
    [Header("Joysticks")]
    public VariableJoystick joystick;

    [Space(10)]
    [SerializeField] ParticleSystem walkEffect;
    private float delatTime = .5f;
    private float time = 0;

    public void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0;
        playerAnimation.PlayRunAnim(isRunning);

        time += Time.deltaTime;
        if (time > delatTime)
        {
            time = 0;
            if (isRunning && playerInteraction.IsInGround())
                walkEffect.Play();
        }

        if (direction.magnitude > .01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if (!isChoping)
            {
                transform.rotation = Quaternion.Euler(-5f, angle, 0f);
            }

            transform.position += moveSpeed * Time.deltaTime * direction;
            //controller.Move(moveSpeed * Time.deltaTime * direction);
        }

    }


}
