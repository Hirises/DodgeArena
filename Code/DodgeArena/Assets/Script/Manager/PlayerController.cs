using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    private Player player;
    [SerializeField]
    private JoyStickHUD joystick;

    [SerializeField]
    private float playerSpeed;

    // Update is called once per frame
    public void Update()
    {
        if(GameManager.instance.state == GameManager.GameState.Run) {
            Move();
        }
    }

    private void Move()
    {
        //Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 direction = joystick.GetMovement();
        if(direction.Equals(Vector2.zero)) {
            rigidbody.velocity = Vector2.zero;
            HUDManager.instance.ShowQuickBar();
        } else {
            Vector2 moveVector = direction * playerSpeed;
            rigidbody.velocity = moveVector;
            HUDManager.instance.HideQuickBar();
        }
    }
}
