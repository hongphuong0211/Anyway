using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDataConfig {
    public int id;
    public int baseHealthPoint;
    public int baseShieldPoint;
    public int baseEnergyPoint;
    public int increaseHP;
    public int increaseShield;
    public int attack;
    public int increaseAttack;
    public float detectRange;
    public float moveSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float accelSpeed;
    public float rechargeShieldRate;
    public string name;
    public CharacterType characterType;

    public void Init(int id, int hp, int increaseHP, int shield, int incShield, int energy, float rechargeShieldRate,float detectRange) {
        this.id = id;
        this.baseHealthPoint = hp;
        this.baseShieldPoint = shield;
        this.baseEnergyPoint = energy;
        this.increaseHP = increaseHP;
        this.increaseShield = incShield;
        this.rechargeShieldRate = rechargeShieldRate;
        this.detectRange = detectRange;
        this.characterType = (CharacterType)id;
    }
    public void InitAttack(int attack, int increaseAttack) {
        this.attack = attack;
        this.increaseAttack = increaseAttack;
    }
    public void InitMovement(float moveSpeed, float minSpeed, float maxSpeed, float accelSpeed) {
        this.moveSpeed = moveSpeed;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.accelSpeed = accelSpeed;
    }
    public BigNumber GetHP(int level) {
        BigNumber hp = baseHealthPoint + increaseHP * (level - 1);
        return hp;
    }
    public BigNumber GetAttack(int level) {
        BigNumber attack = this.attack + this.increaseAttack * (level - 1);
        return attack;
    }
}
