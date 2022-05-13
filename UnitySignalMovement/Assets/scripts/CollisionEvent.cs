using UnityEngine;
using UnityEngine.Events;


public class CollisionEvent : MonoBehaviour
{
    //Add to a gameObject that has a RigidBody and Collider.

    //none of the colliders can be triggers, both objects need a rigidbody

    bool activatedCol;
    public UnityEvent RunFunction;


    public void OnCollisionEnter(Collision collision)
    {
        if (activatedCol == false)
        {
            activatedCol = true;
            RunFunction.Invoke();
            activatedCol = false;
        }
    }
}