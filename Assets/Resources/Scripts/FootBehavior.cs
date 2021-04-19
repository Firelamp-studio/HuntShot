using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootBehavior : MonoBehaviour
{
    [SerializeField] private bool handPosition;
    [SerializeField] private PlayerController playerController;

    private const float HandMovementOffset = 0.025f;

    private float _footstepTimer;

    void Start()
    {
        if (!handPosition)
            transform.position += HandMovementOffset * transform.forward;
    }

    void Update()
    {
        if (playerController.IsMoving || playerController.IsRotating)
        {
            if (_footstepTimer > 0)
            {
                _footstepTimer -= Time.deltaTime;
            }
            else if (_footstepTimer <= 0)
            {
                var playerVelocity = playerController.Velocity.magnitude;
                if (playerVelocity < playerController.RotationSpeed)
                    playerVelocity = playerController.RotationSpeed;

                _footstepTimer = 1.2f - playerVelocity;

                transform.position += HandMovementOffset * (handPosition ? 1 : -1) * transform.forward;
                handPosition = !handPosition;
            }
        }
    }
}