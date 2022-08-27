using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 멧돼지 객체
/// </summary>
public class WildBoar : LivingEntity {
    [SerializeField]
    [BoxGroup("WildBoar")]
    protected float awarenessDistance;
    [SerializeField]
    [BoxGroup("WildBoar")]
    protected float threateningDuration;
    [SerializeField]
    [BoxGroup("WildBoar")]
    protected float dashSpeed;
    [SerializeField]
    [BoxGroup("WildBoar")]
    protected float dashDistance;
    [SerializeField]
    [BoxGroup("WildBoar")]
    protected int dashDamage;
    [SerializeField]
    [BoxGroup("WildBoar")]
    protected float restDuration;
    [ShowNativeProperty]
    public State state {
        get;
        private set;
    }
    protected Timer timer = new Timer();
    protected int collide;

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

        collide = 0;
        timer.count = Timer.Count.Up;
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
        collide = 0;

        return true;
    }

    //서있기
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

    //위협
    public IEnumerator Threaten()
    {
        state = State.Threaten;
        LookAt(GameManager.instance.player.gameObject.transform.position);
        spriteRenderer.sprite = originData.GetSprite("attack");
        timer.Reset();
        timer.target = threateningDuration;
        while (true)
        {
            yield return null;
            timer.Tick();
            if (timer.Check())
            {
                break;
            }
        }
        StartCoroutine("Dash");
    }

    //돌진
    public IEnumerator Dash()
    {
        if(collider.collided.Count == 0) {
            state = State.Dash;
            Vector3 dir = transform.right;
            float distance = Util.ToVector(transform.position, GameManager.instance.player.gameObject.transform.position).magnitude + dashDistance;
            float dashDuration = distance / dashSpeed;
            timer.Reset();
            timer.target = dashDuration;
            while(true) {
                rigidbody.velocity = dir * dashSpeed;
                yield return null;
                timer.Tick();
                if(timer.Check()) {
                    break;
                }
            }
        }
        StartCoroutine("Rest");
    }

    public override void OnStartCollide(Entity other, Collision2D collision) {
        if(state == State.Dash) {
            StopCoroutine("Dash");
            StartCoroutine("Rest");
        }
    }

    public override void OnStartTrigger(Entity other, Collider2D collider) {
        if(state == State.Dash) {
            if(other.type == EntityTypeEnum.Player) {
                Player player = (Player) other;
                player.Damage(dashDamage);
            }
        }
    }

    //휴식
    public IEnumerator Rest()
    {
        state = State.Rest;
        rigidbody.velocity = Vector2.zero;
        spriteRenderer.sprite = originData.GetSprite("normal");
        timer.Reset();
        timer.target = restDuration;
        while (true)
        {
            yield return null;
            timer.Tick();
            if (timer.Check())
            {
                break;
            }
        }
        StartCoroutine("Stand");
    }
}
