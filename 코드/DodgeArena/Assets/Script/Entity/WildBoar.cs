using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¸äµÅÁö °´Ã¼
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

    public override void OnLoad()
    {
        base.OnLoad();
        StartCoroutine("Stand");
    }

    //¼­ÀÖ±â
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

    //À§Çù
    public IEnumerator Threaten()
    {
        LookAt(GameManager.instance.player.gameObject.transform.position);
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

    //À§Çù
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

    //ÈÞ½Ä
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
