using System.Collections;
using UnityEngine;

public class JoyStickHUD : MonoBehaviour {
    [SerializeField]
    public GameObject knob;
    [SerializeField]
    public float limit;
    private bool enabled;

    public void Enable(Vector2 screenPos) {
        Vector2 pos = GameManager.instance.camera.ScreenToWorldPoint(screenPos);
        transform.position = pos;
        knob.transform.position = this.transform.position;
        gameObject.SetActive(true);
        enabled = true;
    }

    public void Run(Vector2 screenPos) {
        if(!enabled) {
            return;
        }
        Vector2 touchPos = GameManager.instance.camera.ScreenToWorldPoint(screenPos);
        Vector2 pos = transform.position;
        Vector2 dir = touchPos - pos;
        if(dir.magnitude > limit) {
            touchPos = pos + dir.normalized * limit;
        }
        knob.transform.position = touchPos;
    }

    public Vector2 GetMovement() {
        Vector2 dir = knob.transform.position - transform.position;
        return dir.normalized;
    }

    public void Disable() {
        enabled = false;
        knob.transform.position = this.transform.position;
        gameObject.SetActive(false);
    }
}