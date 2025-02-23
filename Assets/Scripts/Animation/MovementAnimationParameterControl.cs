using UnityEngine;

public class MovementAnimationParameterControl : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.MovementEvent += SetAnimationParameters;
    }

    private void OnDisable()
    {
        EventHandler.MovementEvent -= SetAnimationParameters;
    }

    private void SetAnimationParameters(MovementParameters parameters)
    {
        animator.SetFloat(Settings.xInput, parameters.inputX);
        animator.SetFloat(Settings.yInput, parameters.inputY);
        animator.SetBool(Settings.isWalking, parameters.isWalking);
        animator.SetBool(Settings.isRunning, parameters.isRunning);

        animator.SetInteger(Settings.toolEffect, (int)parameters.toolEffect);

        
        if (parameters.isUsingToolRight)
            animator.SetTrigger(Settings.isUsingToolRight);
        if (parameters.isUsingToolLeft)
            animator.SetTrigger(Settings.isUsingToolLeft);
        if (parameters.isUsingToolUp)
            animator.SetTrigger(Settings.isUsingToolUp);
        if (parameters.isUsingToolDown)
            animator.SetTrigger(Settings.isUsingToolDown);

        if (parameters.isLiftingToolRight)
            animator.SetTrigger(Settings.isLiftingToolRight);
        if (parameters.isLiftingToolLeft)
            animator.SetTrigger(Settings.isLiftingToolLeft);
        if (parameters.isLiftingToolUp)
            animator.SetTrigger(Settings.isLiftingToolUp);
        if (parameters.isLiftingToolDown)
            animator.SetTrigger(Settings.isLiftingToolDown);

        if (parameters.isPickingRight)
            animator.SetTrigger(Settings.isPickingRight);
        if (parameters.isPickingLeft)
            animator.SetTrigger(Settings.isPickingLeft);
        if (parameters.isPickingUp)
            animator.SetTrigger(Settings.isPickingUp);
        if (parameters.isPickingDown)
            animator.SetTrigger(Settings.isPickingDown);

        if (parameters.isSwingingToolRight)
            animator.SetTrigger(Settings.isSwingingToolRight);
        if (parameters.isSwingingToolLeft)
            animator.SetTrigger(Settings.isSwingingToolLeft);
        if (parameters.isSwingingToolUp)
            animator.SetTrigger(Settings.isSwingingToolUp);
        if (parameters.isSwingingToolDown)
            animator.SetTrigger(Settings.isSwingingToolDown);

        if (parameters.idleUp)
            animator.SetTrigger(Settings.idleUp);
        if (parameters.idleDown)
            animator.SetTrigger(Settings.idleDown);
        if (parameters.idleLeft)
            animator.SetTrigger(Settings.idleLeft);
        if (parameters.idleRight)
            animator.SetTrigger(Settings.idleRight);
    }

    private void AnimationEventPlayFootstepSound()
    {
        
    }
}
