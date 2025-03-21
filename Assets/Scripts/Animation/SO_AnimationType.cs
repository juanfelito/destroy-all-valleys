using UnityEngine;

[CreateAssetMenu(fileName = "so_AnimationType", menuName = "Scriptable Object/Animation/Animation Type")]
public class SO_AnimationType : ScriptableObject {
    public AnimationClip animationClip;
    public AnimationName animationName;
    public CharacterPartAnimator characterPart;
    public PartVariantColour partVariantColour;
    public PartVariantType partVariantType;
}