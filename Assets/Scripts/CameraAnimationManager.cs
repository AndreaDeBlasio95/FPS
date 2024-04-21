using UnityEngine;

public class CameraAnimationManager : MonoBehaviour
{

    [SerializeField] private CameraAnimationMovement cameraAnimationMovement;
    [SerializeField] private CameraAnimationAim cameraAnimationAim;

    private void Start()
    {
        cameraAnimationMovement.enabled = false;
    }

    // Enable & Disable Camera Movements Animation
    #region CameraMovements
    public void EnableCameraMovementAnimation()
    {
        cameraAnimationMovement.enabled = true;
    }

    public void DisableCameraMovementAnimation()
    {
        cameraAnimationMovement.enabled = false;
    }
    #endregion
    // -----------------------------------------

    // Enable & Disable Camera Aim Animation
    #region CameraAim
    public void AimAnimation(bool _aim)
    {
        cameraAnimationAim.AimAnimation(_aim);
    }
    #endregion
    // -----------------------------------------
}