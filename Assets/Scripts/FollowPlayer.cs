using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject Player;
    public float xOffset = .5f;
    public float yOffset = .5f;

    void LateUpdate()
    {
        transform.position = new Vector3(Player.transform.position.x + xOffset, Player.transform.position.y + yOffset, -3);
    }
}
