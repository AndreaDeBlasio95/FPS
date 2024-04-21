using UnityEngine;

public class CameraAnimationAim : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AimAnimation(bool _aim)
    {
        anim.SetBool("Aim", _aim);
    }

}
