using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform targetTranform;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Transform cameraTranform;
    [SerializeField] LayerMask collisionLayers;

    [SerializeField] float cameraCollisionOffset = 0.2f;
    [SerializeField] float minimumCollisionOffset = 0.2f;
    [SerializeField] float cameraCollisionRadius = 2f;
    [SerializeField] float cameraFollowSpeed = 0.2f;
    [SerializeField] float cameraLookSpeed = 2f;
    [SerializeField] float cameraPivotSpeed = 2f;

    [SerializeField] float lookAngle;
    [SerializeField] float pivotAngle;
    [SerializeField] float minimumPivotAngle = -35f;
    [SerializeField] float maximumPivotAngle = 35f;

    [SerializeField] float defaultPosition;

    private PlayerInput playerInput;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosion;

    private void Awake()
    {
        targetTranform = FindObjectOfType<PlayerContoller>().transform;
        playerInput = FindObjectOfType<PlayerInput>();
        cameraTranform = Camera.main.transform;
        defaultPosition = cameraTranform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotationCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPositon = Vector3.SmoothDamp(transform.position, targetTranform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPositon;
    }

    private void RotationCamera()
    {
        Quaternion targetRotation;
        Vector3 rotation;

        lookAngle = lookAngle + (playerInput.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (playerInput.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;

    }

    private void HandleCameraCollision()
    {
        float targetPosion = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTranform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosion), collisionLayers))
        {
            float distatnce = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosion = -(distatnce - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosion) < minimumPivotAngle)
        {
            targetPosion -= minimumCollisionOffset;
        }

        cameraVectorPosion.z = Mathf.Lerp(cameraTranform.localPosition.z, targetPosion, 0.2f);
        cameraTranform.localPosition = cameraVectorPosion;
    }
}
