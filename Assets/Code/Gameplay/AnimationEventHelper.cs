using DoodleJump.Common;
using TMPro;
using UnityEngine;

namespace DoodleJump.Gameplay
{
    public class AnimationEventHelper : CommonBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] texts;

        public void OnSetTextAnimationEvent(AnimationEvent animationEvent)
        {
            var textIndex = animationEvent.intParameter;
            var text = GetText(textIndex);
            var textString = animationEvent.stringParameter;

            if(text == null)
            {
                Common.Logger.Instance.LogError($"Set text animation event failed. No text with index : {textIndex} found.");
                return;
            }

            text.text = textString;
        }

        private TextMeshProUGUI GetText(int index)
        {
            if(index < 0 || index >= texts.Length)
            {
                return null;
            }

            return texts[index];
        }
    }
}
