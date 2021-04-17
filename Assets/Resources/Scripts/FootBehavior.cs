using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootBehavior : MonoBehaviour
{
    [SerializeField] private bool handPosition;
    [SerializeField] private PlayerController playerController;

    private const float HandMovementOffset = 0.025f;

    void Start()
    {
        if (!handPosition)
            transform.position += HandMovementOffset * transform.forward;

        StartCoroutine(MoveHand());
    }

    void Update()
    {
    }

    IEnumerator MoveHand()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (playerController.IsMoving || playerController.IsRotating)
            {
                transform.position += HandMovementOffset * (handPosition ? 1 : -1) * transform.forward;
                handPosition = !handPosition;
            }
        }
    }
}