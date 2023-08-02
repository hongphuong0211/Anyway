using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDataConfig {
    public int id;
    public float detectRange;
    public float moveSpeed;
    public float minSpeed;
    public float maxSpeed;
    public float obstacleSpeed;
    public float decodeSpeed;
    public float healSpeed;
    public float attackSpeed;
    public string name;
    public CharacterType characterType;
    public CharacterClass classChar;
    public CharacterDataConfig()
    {
        this.id = 0;
    }

    public CharacterDataConfig(int id, float detectRange, CharacterType type, CharacterClass classChar = CharacterClass.DEFAULT ) {
        this.id = id;
        this.detectRange = detectRange;
        this.characterType = type;
        this.classChar = classChar;
    }
    public void InitMovement(float moveSpeed, float minSpeed, float maxSpeed, float obstacleSpeed) {
        this.moveSpeed = moveSpeed;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.obstacleSpeed = obstacleSpeed;
    }
}
