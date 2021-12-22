using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimation playerAnimation;

    [Space(10)]
    public float moveSpeed;
    public float turnSmoothTime = .1f;
    private float turnSmoothVelocity;

    [Space(10)]
    [Header("Joysticks")]
    public VariableJoystick joystick;

    public void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0;
        playerAnimation.PlayRunAnim(isRunning);

        if (direction.magnitude > .01f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            transform.position += moveSpeed * Time.deltaTime * direction;
            //controller.Move(moveSpeed * Time.deltaTime * direction);
        }

    }


}
