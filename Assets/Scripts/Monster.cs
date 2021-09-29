using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterBehavior MonsterBehavior = null;

    private int playerNotFoundFrames = 0;
    private Vector2 direction;
    private Collider2D myCollider;
    private RaycastHit2D[] results;

    void Update()
    {        
        LookForPlayer();        
    } 

    private void LateUpdate()
    {
        MoveOrDespawn();
    }

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        
        direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        Debug.Log(direction);
    }
    private void MoveOrDespawn()
    {        
        transform.Translate(direction * MonsterBehavior.Speed * Time.deltaTime);
        if (playerNotFoundFrames > 100) this.gameObject.SetActive(false);
    }

    private void LookForPlayer()
    {
        Quaternion angle = Quaternion.AngleAxis(22.5f, Vector3.forward);
        Vector3 rayDirection = Vector3.up;        
        playerNotFoundFrames += 1;
        for (int i = 0; i < 16; i++)
        {
            rayDirection = angle * rayDirection;
            Debug.DrawRay(transform.position, rayDirection, Color.green, 1);

            results = new RaycastHit2D[35];

            var collisions = myCollider.Cast(rayDirection, results, MonsterBehavior.AggroRadius);
            if(collisions > 0)
            {
                Debug.Log($"Collisions: {collisions}");
               if(results[0].collider.tag == "Player")
               {
                    Debug.Log("Hit Player");
                    playerNotFoundFrames = 0;
                    direction = rayDirection;
                    break;
               }               
            }
        }           
    }
}
