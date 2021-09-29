using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{        
    void Start()
    {
        InvokeRepeating("SpawnMonster",.5f, UnityEngine.Random.value);
    }

    void SpawnMonster()
    {
        
        Debug.Log("Spawn Started");
        GameObject mon = ObjectPooler.Instance.GetPooledObject();
        
        if(mon != null)
        {
            Debug.Log(mon);
            float xPos = Random.Range(Camera.main.transform.position.x - 5, Camera.main.transform.position.x + 5);
            float yPos = Random.Range(Camera.main.transform.position.y - 10, Camera.main.transform.position.y + 10);
            Debug.Log($"Spawning monster at X: {xPos} Y: {yPos}");
            mon.transform.position = new Vector3(xPos, yPos);
            mon.SetActive(true);
        }
    }
}
