using UnityEngine;

public class PipoAnimationController : MonoBehaviour
{
    private Animator animator;
    private StatsPlayer stats;

    [Header("Scale")]
    public float baseScale = 0.07f;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = StatsPlayer.instance;

        transform.localScale = Vector3.one * baseScale;
    }

    void Update()
    {
        if (stats == null) return;

        UpdateMood();
    }

    void UpdateMood()
    {
        // 0 = Neutral
        // 1 = Happy
        // 2 = Happier
        // 3 = Sick

        if (stats.limpieza < 10f)
        {
            animator.SetInteger("Mood", 3);
        }
        else if (stats.felicidad >= 85f)
        {
            animator.SetInteger("Mood", 2);
        }
        else if (stats.felicidad >= 50f)
        {
            animator.SetInteger("Mood", 1);
        }
        else
        {
            animator.SetInteger("Mood", 0);
        }
    }
}