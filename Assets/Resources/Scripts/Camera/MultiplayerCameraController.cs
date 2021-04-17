using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    private Vector3 offsetPosition;
    private Vector3 startPosition;
    private float startFOV;
    private Camera cameraController;

    private Vector3 targetPosition;
    private float targetFOV;

    private const float MinFOV = 30;
    private const float MaxFOV = 70;
    private const float SmoothTime = .3f;

    private const float ZoomLimiter = 25;

    private Vector3 velocity;

    void Awake()
    {
        cameraController = GetComponent<Camera>();
        startPosition = transform.position;
        offsetPosition = new Vector3(0, 30, 0);
        startFOV = cameraController.fieldOfView;

        targetPosition = startPosition;
        targetFOV = startFOV;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targets.Count > 1)
        {
            Bounds bounds = new Bounds();
            foreach (var target in targets)
            {
                bounds.Encapsulate(target.position);
            }

            targetPosition = bounds.center + offsetPosition;

            float distance = (float) Math.Sqrt(Math.Pow(bounds.size.x, 2) + Math.Pow(bounds.size.y, 2) +
                                               Math.Pow(bounds.size.z, 2));
            targetFOV = Mathf.Lerp(MinFOV, MaxFOV, distance / ZoomLimiter);
        }

        if (targetPosition != transform.position)
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        if (Math.Abs(targetFOV - cameraController.fieldOfView) > 0.1f)
            cameraController.fieldOfView = Mathf.Lerp(cameraController.fieldOfView, targetFOV, Time.deltaTime);
    }

    public void OnRoundStart(List<Transform> playerBodies)
    {
        targets.Clear();

        foreach (var playerBody in playerBodies)
        {
            targets.Add(playerBody);
        }
    }

    public void AddCameraTarget(Transform target)
    {
        targets.Add(target);
    }

    public void RemoveCameraTarget(Transform target)
    {
        targets.Remove(target);
    }

    public void OnPlayerDefeat(Transform playerBody)
    {
        targets.Remove(playerBody);

        var players = targets.FindAll(t => t.CompareTag("Player"));
        if (players.Count < 1)
        {
            targetPosition = startPosition;
            targetFOV = startFOV;
            targets.Clear();
        }
        else if (players.Count == 1)
        {
            targetPosition = offsetPosition;
            targetFOV = MaxFOV;
            targets.RemoveAll(t => !t.CompareTag("Player"));
        }
    }
}
