using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectTextDamaged : Effect {
    public Text m_TextDamage;
    public override void OnEnable() {
        m_LifeTime = 0.5f;
    }
    public void SetTextDamage(BigNumber damage, DamageType damageType) {
        switch (damageType) {
            case DamageType.CRITICAL: {
                    m_TextDamage.color = Color.red;
                }
                break;
            case DamageType.FIRE: {
                    m_TextDamage.color = Utilss.GetColor("#FF5A00");
                }
                break;
            case DamageType.ICE: {
                    m_TextDamage.color = Utilss.GetColor("#00CFFF");
                }
                break;
            case DamageType.POISON: {
                    m_TextDamage.color = Utilss.GetColor("#00B908");
                }
                break;
            default: {
                    m_TextDamage.color = Color.white;
                }
                break;
        }
        m_TextDamage.text = "-" + damage.ToString2();
    }
    public void SetTextDamage(string info) {
        m_TextDamage.text = info;
    }
}