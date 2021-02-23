using UnityEngine;
using TMPro;

namespace DoodleJump.UI
{
    public class KeyValueUI : UIComponent
    {
        [SerializeField] private TextMeshProUGUI valueText;

        public void SetValue(string value)
        {
            valueText.text = value;
        }

        public void SetValue(int value)
        {
            SetValue(value.ToString());
        }
    }
}