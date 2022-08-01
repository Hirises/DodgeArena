using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite player_normal;
    [SerializeField]
    private Sprite player_hit;

    [SerializeField]
    private float playerSpeed;

    // Update is called once per frame
    public void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction.Normalize();
        Vector2 moveVector = direction * playerSpeed;
        rigidbody.velocity = moveVector;
    }
}
