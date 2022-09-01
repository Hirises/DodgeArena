using System.Collections;
using UnityEngine;

public class JoyStickHUD : MonoBehaviour {
    [SerializeField]
    private RectTransform canvas;
    [SerializeField]
    private RectTransform knob;
    [SerializeField]
    private RectTransform self;
    [SerializeField]
    private float limit;
    private bool actived;

    public void Enable(Vector2 screenPos) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, GameManager.instance.camera, out Vector2 localPos);
        localPos += canvas.sizeDelta / 2;
        self.anchoredPosition = localPos;
        knob.anchoredPosition = Vector2.zero;
        gameObject.SetActive(true);
        actived = true;
    }

    public void Run(Vector2 screenPos) {
        if(!actived) {
            return;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, GameManager.instance.camera, out Vector2 localPos);
        localPos += canvas.sizeDelta / 2;
        Vector2 pos = self.anchoredPosition;
        Vector2 dir = localPos - pos;
        if(dir.magnitude > limit) {
            knob.anchoredPosition = dir.normalized * limit;
        } else {
            knob.anchoredPosition = dir;
        }
    }

    public Vector2 GetMovement() {
        return knob.anchoredPosition / limit;
    }

    public void Disable() {
        if(!actived) {
            return;
        }
        actived = false;
        self.anchoredPosition = Vector2.zero;
        knob.anchoredPosition = Vector2.zero;
        gameObject.SetActive(false);
    }
}