using UnityEngine;

public class Joystick : MonoBehaviour
{    
    public Transform Player;
    public float speed = 5.0f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointA = new Vector3(Input.mousePosition.x, Input.mousePosition.y);            
        }
        if (Input.GetMouseButton(0))
        {            
            touchStart = true;
            pointB = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }
        else
        {
            touchStart = false;
        }

    }
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, .25f);
            moveCharacter(direction);

        }
    }
    void moveCharacter(Vector2 direction)
    {
        Player.Translate(direction * speed * Time.deltaTime);        
    }
}