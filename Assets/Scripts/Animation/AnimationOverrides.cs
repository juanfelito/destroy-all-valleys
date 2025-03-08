using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour {
    [SerializeField] private GameObject character = null;
    [SerializeField] private SO_AnimationType[] soAnimationTypes = null;

    private Dictionary<AnimationClip, SO_AnimationType> animationTypeByAnimation;
    private Dictionary<string, SO_AnimationType> animationTypeByCompositeAttrKey;

    private void Start() {
        animationTypeByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();
        foreach (var item in soAnimationTypes) {
            animationTypeByAnimation.Add(item.animationClip, item);
        }

        animationTypeByCompositeAttrKey = new Dictionary<string, SO_AnimationType>();
        foreach (var item in soAnimationTypes) {
            string key = item.characterPart.ToString() + item.partVariantColour.ToString() + item.partVariantType.ToString() + item.animationName.ToString();
            animationTypeByCompositeAttrKey.Add(key, item);
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributesList) {
        //Stopwatch s1 = Stopwatch.StartNew();
        // Find animators in scene that match scriptable object animator type
        Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();

        foreach (CharacterAttribute characterAttribute in characterAttributesList) {
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip, AnimationClip>> animsKeyValuePairList = new List<KeyValuePair<AnimationClip, AnimationClip>>();

            string animatorSOAssetName = characterAttribute.characterPart.ToString();

            foreach (Animator animator in animatorsArray) {
                if (animator.name == animatorSOAssetName) {
                    currentAnimator = animator;
                    break;
                }
            }

            // Get base current animations for animator
            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);

            foreach (AnimationClip animationClip in animationsList) {
                // find animation in dictionary
                SO_AnimationType so_AnimationType;
                bool foundAnimation = animationTypeByAnimation.TryGetValue(animationClip, out so_AnimationType);

                if (foundAnimation) {
                    string key = characterAttribute.characterPart.ToString() + characterAttribute.partVariantColour.ToString() + characterAttribute.partVariantType.ToString() + so_AnimationType.animationName.ToString();

                    SO_AnimationType swapSO_AnimationType;
                    bool foundSwapAnimation = animationTypeByCompositeAttrKey.TryGetValue(key, out swapSO_AnimationType);

                    if (foundSwapAnimation) {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;

                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, swapAnimationClip));
                    }
                }
            }

            // Apply animation updates to animation override controller and then update animator with the new controller
            aoc.ApplyOverrides(animsKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }

        // s1.Stop();
        // UnityEngine.Debug.Log("Time to apply character customisation : " + s1.Elapsed + "   elapsed seconds");
    }
}