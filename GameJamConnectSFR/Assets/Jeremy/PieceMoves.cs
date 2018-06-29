using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMoves : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D body;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
    }

    public void HandleMovement(float horizontal)
    {
        body.velocity = new Vector2(horizontal * moveSpeed, body.velocity.y);
    }
}