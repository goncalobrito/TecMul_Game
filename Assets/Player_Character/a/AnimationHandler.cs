using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [Header("The Character Animator")]
    [SerializeField]
    private Animator animator;

    [Header("Setup")]
    [SerializeField]
    private float smoothing = 0.1f;
    [SerializeField]
    private float runningSmoothing = 0.05f;

    private float currentX = 0f;
    private float currentY = 0f;

    public void UpdateAnimation(Vector2 inputDir, bool isRunning)
    {
        float currentSmoothing = isRunning ? runningSmoothing : smoothing;

        float targetX = 0f;
        float targetY = 0f;

        if (inputDir.magnitude > 0.1f)
        {
            targetX = isRunning ? inputDir.x : inputDir.x * 0.5f;
            targetY = isRunning ? inputDir.y : inputDir.y * 0.5f;
        }

        currentX = Mathf.Lerp(currentX, targetX, currentSmoothing);
        currentY = Mathf.Lerp(currentY, targetY, currentSmoothing);

        animator.SetFloat("x", currentX);
        animator.SetFloat("y", currentY);
    }
}