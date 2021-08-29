using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 체력을 닳게해도 되고, 밖으로 날려도 이기는?

// 스테이지 시작시 아군, 적 생성 // 스테이지 여러개 생성.. 카메라 설정
// 적 처치후 스테이지 이동
// 동료, 적 에셋 추가
// 동료 추가, 밀리는걸 팀 단위로,, 그냥 개별적으로 움직이게?
// 스테이지 전환시 연출
// 스테이지 진입전 로비씬
// 체력바, 피격시 데미지 폰트

public enum TeamType
{
    Ally, Enemy,
}

public class Character : MonoBehaviour
{
    [System.Serializable]
    private struct Effects
    {
        public GameObject deathEffect;
        public GameObject hitEffect;
    }
    [SerializeField]
    private Effects effects;
    
    [SerializeField]
    private TeamType teamType;
    public TeamType TeamType
    {
        get { return teamType; }
        set
        {
            teamType = value;

            bool isRight = ( teamType == TeamType.Ally );
            animator.SetBool( AnimParams.IsRight, isRight );
            moveDirection = ( isRight ? Vector2.right.x : Vector2.left.x );
        }
    }

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

                if ( effects.deathEffect != null )
                {
                    GameObject effect = Instantiate( effects.deathEffect );
                    if ( effect != null )
                    {
                        effect.transform.position = transform.position;
                    }
                }
                
                Destroy( gameObject );
            }
        }
    }

    public float strength;
    public float moveSpeed;

    protected Animator animator;
    protected Rigidbody2D rigid;

    protected static class AnimParams
    {
        public static int IsRight = Animator.StringToHash( "IsRight" );
    }

    private float moveDirection;
    private float hitDelay = 0.0f;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        TeamType = teamType;
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

    protected void OnHit( Collision2D collision )
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

        if ( enemy == null )
        {
            Debug.LogError( "enemy is null" );
            return;
        }

        float power = 100.0f + ( enemy.strength * 10f );
        hitDelay = 0.15f;

        Vector2 force = new Vector2( power * -moveDirection, power );
        rigid.AddForce( force );

        Health -= enemy.strength;

        if ( effects.hitEffect != null )
        {
            Instantiate( effects.hitEffect, collision.GetContact( 0 ).point, Quaternion.identity );
        }
    }

    protected void OnCollisionEnter2D( Collision2D collision )
    {
        OnHit( collision );
    }

    protected void OnCollisionStay2D( Collision2D collision )
    {
        if ( hitDelay > 0.0f )
        {
            return;
        }

        OnHit( collision );
    }
}
