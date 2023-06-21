using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dmgText;
    [SerializeField] private Color DamageColor;
    [SerializeField] private Color HealColor;
    [SerializeField] private Color EtherLossColor;

    public void InitializeText(int damage, bool isHeal = false, bool isEther = false)
    {
        
        if (isHeal)
        {
            _dmgText.color = HealColor;
            _dmgText.text = "<size=65%><I>+<nobr>  <size=100%><b>" + damage.ToString();
        }
        else if (isEther)
        {
            _dmgText.color = EtherLossColor;
            _dmgText.text = "<size=40%><I>-<nobr>  <size=75%><b>" + damage.ToString();
        }
        else
        {
            _dmgText.color = DamageColor;
            _dmgText.text = "<size=65%><I>-<nobr>  <size=100%><b>" + damage.ToString();
        }
    }
}
