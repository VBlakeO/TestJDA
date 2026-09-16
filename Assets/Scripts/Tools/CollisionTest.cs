using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        Vector2 direction = c.GetContact(0).normal;
        if(direction.x == 1)  print("right");
        if(direction.x == -1) print("left");
        if(direction.y == 1)  print("up");
        if(direction.y == -1) print("down");
    }
}
