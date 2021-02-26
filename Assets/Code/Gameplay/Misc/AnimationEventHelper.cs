using DoodleJump.Common;
using TMPro;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    /// <summary>
    /// A behaviour that can be attached to a gameobject and used for common animation event functionalities.
    /// </summary>
    public class AnimationEventHelper : CommonBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] texts;
        [SerializeField] private ParticleSystem[] particleSystems;


        public void OnSetTextAnimationEvent(AnimationEvent animationEvent)
        {
            var text = GetComponentByIndex(texts, animationEvent.intParameter);
            var textString = animationEvent.stringParameter;

            if(text == null)
            {
                Common.Logger.Instance.LogError($"Set text animation event failed. No text with index : {animationEvent.intParameter} found.");
                return;
            }

            text.text = textString;
        }

        public void PlayParticleSystem(AnimationEvent animationEvent)
        {
            var particleSystem = GetComponentByIndex(particleSystems, animationEvent.intParameter);
            if(particleSystem == null)
            {
                Common.Logger.Instance.LogError($"Play particle system failed, no particle system with index : {animationEvent.intParameter} found.");
                return;
            }

            particleSystem.Play();
        }

        private T GetComponentByIndex<T>(T[] components, int index) where T : Component
        {
            if(index < 0 || index >= components.Length)
            {
                return null;
            }

            return components[index];
        }
    }
}
