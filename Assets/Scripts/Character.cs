using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 서로 적에게 돌진
/// 공격력, 체력, 이속, 날라가는거리,
/// 

/// 체력을 닳게해도 되고, 밖으로 날려도 이기는?

public enum TeamType
{
    Ally, Enemy,
}

public class Character : MonoBehaviour
{
    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
        set 
        {
            health = value;
            if ( Health <= 0.0f )
            {
                Debug.LogError( "쥬금.. " + name );
            }
        }
    }

    public float strength;
    public float moveSpeed;

    protected float moveDirection;

    protected Animator animator;
    protected Rigidbody2D rigid;

    public TeamType teamType;

    private float hitDelay = 0.0f;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        moveDirection = ( teamType == TeamType.Ally ? Vector2.right.x : Vector2.left.x );
    }

    protected void Update()
    {
        if ( hitDelay > 0.0f )
        {
            hitDelay -= Time.deltaTime;
            return;
        }

        rigid.velocity = new Vector2( moveSpeed * moveDirection, rigid.velocity.y );
    }

    protected void OnCollisionEnter2D( Collision2D collision )
    {
        Character enemy = collision.gameObject?.GetComponent<Character>();
        if ( enemy == null )
        {
            return;
        }

        if ( teamType == enemy.teamType )
        {
            Debug.Log( "is Ally" );
            return;
        }

        float power = 50.0f + ( enemy.strength * 5.0f );

        hitDelay = 0.002f * power; // 발이 땅에 닿으면 풀리게?

        Vector2 force = new Vector2( power * -moveDirection, power );
        rigid.AddForce( force );

        Health -= enemy.strength;

        Debug.Log( collision.gameObject.name );
    }
}
