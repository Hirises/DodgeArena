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
    private float dashDistance;
    [SerializeField]
    private float restDuration;
    private Timer timer = new Timer();
    public State state
    {
        get;
        private set;
    }

    public enum State
    {
        Stand,
        Threaten,
        Dash,
        Rest
    }

    public override bool OnLoad()
    {
        if(!base.OnLoad()){
            return false;
        }

        StartCoroutine("Stand");

        return true;
    }

    public override bool OnUnload()
    {
        if (!base.OnUnload())
        {
            return false;
        }

        StopAllCoroutines();

        return true;
    }

    //¼­ÀÖ±â
    public IEnumerator Stand()
    {
        state = State.Stand;
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
        state = State.Threaten;
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

    //µ¹Áø
    public IEnumerator Dash()
    {
        state = State.Dash;
        Vector3 dir = transform.right;
        float distance = Util.ToVector(transform.position, GameManager.instance.player.gameObject.transform.position).magnitude + dashDistance;
        float dashDuration = distance / dashSpeed;
        timer.Reset();
        while (true)
        {
            rigidbody.velocity = dir * dashSpeed;
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
        state = State.Rest;
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
