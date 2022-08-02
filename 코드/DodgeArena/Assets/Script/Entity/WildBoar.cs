using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ��ü
/// </summary>
public class WildBoar : LivingEntity
{
    [SerializeField]
    private Sprite normal;
    [SerializeField]
    private Sprite attack;
    [SerializeField]
    private float awarenessDistance;
    [SerializeField]
    private float threateningDuration;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDuration;
    [SerializeField]
    private float restDuration;
    private Timer timer = new Timer();

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnLoad()
    {
        base.OnLoad();
        StartCoroutine("Stand");
    }

    //���ֱ�
    public IEnumerator Stand()
    {
        while (true)
        {
            if (CheckPlayerDistance(awarenessDistance))
            {
                StartCoroutine("Threaten");
                break;
            }

            yield return null;
        }
    }

    //����
    public IEnumerator Threaten()
    {
        Vector3 player = GameManager.instance.player.gameObject.transform.position;
        Vector3 self = transform.position;
        Vector2 dir = new Vector2(player.x - self.x, player.y - self.y);
        Debug.Log(dir.ToString());
        dir.Normalize();
        float angle = Vector2.SignedAngle(Vector2.right, dir);
        Debug.Log(angle.ToString());
        transform.rotation = Quaternion.Euler(0, 0, angle);
        spriteRenderer.flipY = transform.rotation.eulerAngles.x <= 0;
        spriteRenderer.sprite = attack;
        timer.Reset();
        while (true)
        {
            yield return null;
            timer.Tick();
            if (timer.Check(threateningDuration))
            {
                break;
            }
        }
        StartCoroutine("Dash");
    }

    //����
    public IEnumerator Dash()
    {
        rigidbody.velocity = transform.right * dashSpeed;
        timer.Reset();
        while (true)
        {
            yield return null;
            timer.Tick();
            if (timer.Check(dashDuration))
            {
                break;
            }
        }
        StartCoroutine("Rest");
    }

    //�޽�
    public IEnumerator Rest()
    {
        rigidbody.velocity = Vector2.zero;
        spriteRenderer.sprite = normal;
        timer.Reset();
        while (true)
        {
            yield return null;
            timer.Tick();
            if (timer.Check(restDuration))
            {
                break;
            }
        }
        StartCoroutine("Stand");
    }
}
