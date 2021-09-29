using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBehavior", menuName = "Monsters")]
public class MonsterBehavior : ScriptableObject
{
    public float Health;
    public float DamageDone;
    public float AggroRadius = 2;
    public float Speed;
}
