using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private float zOffset;

    private void LateUpdate()
    {
        if(GameManager.instance.state == GameManager.GameState.Run) {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + zOffset);
        }
    }
}
