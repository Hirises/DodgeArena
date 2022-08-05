using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ����� ��ü
/// </summary>
public class WildBoar : LivingEntity {
    [SerializeField]
    [BoxGroup("WildBoar")]
    private Sprite normal;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private Sprite attack;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private float awarenessDistance;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private float threateningDuration;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private float dashSpeed;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private float dashDistance;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private int dashDamage;
    [SerializeField]
    [BoxGroup("WildBoar")]
    private float restDuration;
    [ShowNativeProperty]
    public State state {
        get;
        private set;
    }
    private Timer timer = new Timer();

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

    //���ֱ�
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

    //����
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

    //����
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

    public override void OnStartCollide(Entity other) {
        if(state == State.Dash && other.type == EntityType.Type.Player) {
            Player player = (Player) other;
            player.Damage(dashDamage);
        }
    }

    //�޽�
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
