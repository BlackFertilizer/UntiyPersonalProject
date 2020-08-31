using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager
{
    Animator animator;

    public AnimatorManager(Animator animator)
    {
        this.animator = animator;
    }

    public void playAnimationFade(string animationName, float fadeTime)
    {
        if(animator != null)
            animator.CrossFade(animationName, fadeTime);
    }
}
