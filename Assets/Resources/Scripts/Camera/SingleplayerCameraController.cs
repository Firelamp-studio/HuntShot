using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SingleplayerCameraController : MonoBehaviour
{
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private GameObject player;

    private const float PositionSmoothTime = .1f;

    private Vector3 _offsetPosition;
    private Vector3 _targetPosition;

    private Vector3 _positionVelocity;

    void Start()
    {
        var startPosition = transform.position;

        _targetPosition = startPosition;
        _offsetPosition = startPosition;

        // if (player.GetComponent<PlayerController>().LocalIndex != 0)
        //     Destroy(GetComponent<StudioListener>());

        GetComponent<StudioListener>().ListenerNumber = player.GetComponent<PlayerController>().LocalIndex;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        _targetPosition = player.transform.position +
                          player.transform.forward * _offsetPosition.z +
                          player.transform.up * _offsetPosition.y +
                          player.transform.right * _offsetPosition.x;
        if (_targetPosition != transform.position)
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _positionVelocity,
                PositionSmoothTime);

        transform.rotation = player.transform.rotation * Quaternion.Euler(90, 0, 0);
    }
}