using UnityEngine;

public class PipoSkinLoader : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (SkinManager.instance != null)
        {
            animator.runtimeAnimatorController =
                SkinManager.instance.GetSelectedSkin();
        }
    }
}