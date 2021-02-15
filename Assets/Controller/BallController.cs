using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 speed = new Vector3();

        if (Input.GetKey(KeyCode.LeftArrow))
            speed.x -= 6f;
        if (Input.GetKey(KeyCode.RightArrow))
            speed.x += 6f;
        if (Input.GetKey(KeyCode.UpArrow))
            speed.z += 6f;
        if (Input.GetKey(KeyCode.DownArrow))
            speed.z -= 6f;
        if (Input.GetKey(KeyCode.Space))
            speed.y += 10f;

        rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, speed, 2f* Time.deltaTime);
    }
}
