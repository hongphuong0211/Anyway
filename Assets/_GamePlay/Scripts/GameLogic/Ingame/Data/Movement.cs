using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Movement {
    public float moveSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float slowRate;
    public float increaseRate;
    public float accel;
    public float bonusSpeed;
    public void Init(float moveSpeed, float maxSpeed,float minSpeed, float accel) {
        this.moveSpeed = moveSpeed;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.slowRate = 0;
        this.increaseRate = 0;
        this.accel = 0;
    }
    public float GetMoveSpeed() {
        if (slowRate > 0 || bonusSpeed > 0) {
            return moveSpeed - moveSpeed * (slowRate / 100f) + moveSpeed * (bonusSpeed / 100f);
        } else {
            return moveSpeed;
        }
    }
    public void AddBonusSpeed(float bonus) {
        bonusSpeed += bonus;
    }
    public void SubtractBonusSpeed(float bonus) {
        bonusSpeed -= bonus;
    }
    public void SetBonusSpeed(float bonus) {
        bonusSpeed = bonus;
    }
    public void SetSlowRate(float amount) {
        slowRate = amount;
    }
    public void AddSlowRate(float amount) {
        slowRate += amount;
    }
    public void SubtractSlowRate(float amount) {
        slowRate -= amount;
    }

}