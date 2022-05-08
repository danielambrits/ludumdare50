using UnityEngine;

[RequireComponent(typeof(Animator), typeof(ParticleSystem))]
public class HelicopterAnimator : MonoBehaviour
{
    private Animator animator;
    private ParticleSystem vfx;

    void Awake() {
        animator = GetComponent<Animator>();
        vfx = GetComponent<ParticleSystem>();
    }

    void OnEnable() {
        Helipad.OnHelicopterActivated.AddListener(PlayFlyAnimation);
    }

    void OnDisable() {
        Helipad.OnHelicopterActivated.RemoveListener(PlayFlyAnimation);
    }

    private void PlayFlyAnimation() {
        animator.SetTrigger("Fly");
    }

    public void PlayVfx() {
        vfx.Play();
    }

}
