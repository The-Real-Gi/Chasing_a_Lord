using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArcherScript : MonoBehaviour
{

    public float health=100;

    public Animator anim;
    public Rigidbody2D rb;

    [SerializeField] private float idleDuration = 2f;
    public float IdleDuration => idleDuration;

    public int facDir = 1;
    public bool isFacingRight = true;
    

    ArcherStateMachine stateMachine;
    public ArcherIdle idle {get;private set;}
    public ArcherMove move {get;private set;}
    public ArcherShoot shoot {get;private set;}
    public ArcherGetHit getHit {get;private set;}
    public ArcherDeath death {get;private set;}
    public ArcherBattleState battleState {get;private set;}

    public float moveSpeed = 3f;

    [SerializeField] private float getHitDuration = 0.35f;
    private float getHitTimer;
    [SerializeField] private float hitKnockbackForce = 5f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheck2;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask whatIsGround;
    public bool isGrounded;

    [SerializeField] private float playerDetectionRadius ;
    [SerializeField] private float tooCloseRange ;
    [SerializeField] private float shootingRange ;
    [SerializeField] private float shootingCooldown = 1f;
    private float shootingCooldownTimer;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float arrowSpeed = 10f;
    [SerializeField] private LayerMask whatIsPlayer;
    public bool shooting;

    public GameObject arrow;

    public GameObject walkingParticles;
    public GameObject getHitParticles;
    public GameObject DeathParticles;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    void Awake()
    {
        if (moveSpeed <= 0f)
        {
            moveSpeed = 3f;
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine = new ArcherStateMachine();
        idle = new ArcherIdle (this,stateMachine,"Idle");
        move = new ArcherMove (this,stateMachine,"Move");
        shoot = new ArcherShoot(this, stateMachine,"Shoot");
        getHit = new ArcherGetHit (this,stateMachine,"GetHit");
        death = new ArcherDeath(this,stateMachine,"Die");

        battleState= new ArcherBattleState(this,stateMachine,"Move");
        stateMachine.Initialize(idle);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {       hpSlider.value=health;
    hpText.text= health.ToString()+" / 100";

        shootingCooldownTimer = Mathf.Max(0f, shootingCooldownTimer - Time.deltaTime);

        if (health <= 0f)
        {   
            hpSlider.gameObject.SetActive(false);
            if (stateMachine.currentState != death)
            {
                stateMachine.ChangeState(death);
            }
            UpdateParticleEffects();
            return;
        }

        if (stateMachine.currentState == getHit)
        {
            stateMachine.currentState.Update();
            UpdateParticleEffects();
            return;
        }

        bool isCombatState = stateMachine.currentState == battleState
            || stateMachine.currentState == move
            || stateMachine.currentState == shoot;
        if (isCombatState && !IsPlayerInRange())
        {
            stateMachine.ChangeState(idle);
            UpdateParticleEffects();
            return;
        }

        stateMachine.currentState.Update();
        bool isPatrolling = stateMachine.currentState == idle;
        if (isPatrolling && IsPlayerInRange())
        {
            stateMachine.ChangeState(battleState);
        }
        UpdateParticleEffects();
        Debug.Log(stateMachine.currentState);
    }

    private void UpdateParticleEffects()
    {
        bool isWalking = stateMachine.currentState == move
            || stateMachine.currentState == battleState;

        SetParticleState(walkingParticles, isWalking);
        SetParticleState(getHitParticles, stateMachine.currentState == getHit);
        SetParticleState(DeathParticles, stateMachine.currentState == death);
    }

    private void SetParticleState(GameObject particleEffect, bool shouldPlay)
    {
        if (particleEffect != null && particleEffect.activeSelf != shouldPlay)
        {
            particleEffect.SetActive(shouldPlay);
        }
    }

    void FixedUpdate()
    {
        Checks();
        stateMachine.currentState.FixedUpdate();
    }

    private void Checks()
    {
        bool firstGroundCheck = groundCheck != null
            && Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        bool secondGroundCheck = groundCheck2 != null
            && Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, whatIsGround);
        isGrounded = firstGroundCheck && secondGroundCheck;
    }

    public void Flip()
    {
        facDir *= -1;
        isFacingRight = facDir >= 0;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facDir;
        transform.localScale = scale;

        if (hpSlider != null && hpSlider.transform.IsChildOf(transform))
        {
            Vector3 sliderScale = hpSlider.transform.localScale;
            sliderScale.x = Mathf.Abs(sliderScale.x) * facDir;
            hpSlider.transform.localScale = sliderScale;
        }

        if (walkingParticles != null)
        {
            Vector3 particleScale = walkingParticles.transform.localScale;
            particleScale.x = Mathf.Abs(particleScale.x) * facDir;
            walkingParticles.transform.localScale = particleScale;
        }
    }

    public void TakeDamage(int damage, Transform attacker = null)
    {
        health -= damage;
        if (health > 0f && stateMachine.currentState != getHit)
        {
            HitByPlayer(attacker);
        }
    }

    public void HitByPlayer(Transform attacker = null)
    {
        if (stateMachine.currentState == death)
        {
            return;
        }

        getHitTimer = getHitDuration;
        Vector2 knockbackDirection = attacker == null
            ? Vector2.left
            : ((Vector2)transform.position - (Vector2)attacker.position).normalized;
        if (knockbackDirection == Vector2.zero)
        {
            knockbackDirection = Vector2.left;
        }

        rb.linearVelocity = new Vector2(knockbackDirection.x * hitKnockbackForce, rb.linearVelocity.y);
        stateMachine.ChangeState(getHit);
    }

    public void UpdateGetHitState()
    {
        getHitTimer -= Time.deltaTime;
        if (getHitTimer <= 0f)
        {
            stateMachine.ChangeState(idle);
        }
    }

    public bool IsPlayerInRange()
    {
        return FindPlayerInRange(playerDetectionRadius) != null;
    }

    public bool IsPlayerInShootingRange()
    {
        return FindPlayerInRange(shootingRange) != null;
    }

    public bool IsAtShootingRangeLimit()
    {
        Transform player = FindPlayerInRange(playerDetectionRadius);
        if (player == null)
        {
            return false;
        }

        return Vector2.Distance(transform.position, player.position) >= shootingRange * 0.9f;
    }

    public bool IsPlayerTooClose()
    {
        return FindPlayerInRange(tooCloseRange) != null;
    }

    public void FacePlayer()
    {
        Transform player = FindPlayerInRange(playerDetectionRadius);
        if (player == null)
        {
            return;
        }

        float horizontalDistance = player.position.x - transform.position.x;
        if (Mathf.Abs(horizontalDistance) > 0.05f && Mathf.Sign(horizontalDistance) != facDir)
        {
            Flip();
        }
    }

    public void FaceAwayFromPlayer()
    {
        Transform player = FindPlayerInRange(playerDetectionRadius);
        if (player == null)
        {
            return;
        }

        float horizontalDistance = transform.position.x - player.position.x;
        int awayDirection = Mathf.Abs(horizontalDistance) <= 0.05f
            ? facDir
            : horizontalDistance > 0f ? 1 : -1;

        if (awayDirection != facDir)
        {
            Flip();
        }
    }

    private Transform FindPlayerInRange(float radius)
    {
        Collider2D[] playerHits = Physics2D.OverlapCircleAll(transform.position, radius, whatIsPlayer);

        foreach (Collider2D playerHit in playerHits)
        {
            PlayerScript playerScript = playerHit.GetComponentInParent<PlayerScript>();
            if (playerScript != null && playerScript.health > 0f)
            {
                return playerScript.transform;
            }
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsPlayerInRange() ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        Gizmos.color = IsPlayerInShootingRange() ? Color.blue : Color.white;
        Gizmos.DrawWireSphere(transform.position, shootingRange);

        Gizmos.color = IsPlayerTooClose() ? Color.red : Color.gray;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);

        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
        if (groundCheck2 != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheck2.position, groundCheck2.position + Vector3.down * groundCheckDistance);
        }
    }

    public void ShootSingal()
    {
        if (CanShoot && IsPlayerInShootingRange())
        {
            shooting = true;
        }
    }

    public bool CanShoot => shootingCooldownTimer <= 0f;

    public void ResetTooCloseShootTimer()
    {
        battleState.ResetTooCloseShootTimer();
    }

    public void ShootArrow()
    {
        if (!shooting || arrow == null)
        {
            return;
        }

        shooting = false;
        Transform player = FindPlayerInRange(shootingRange);
        if (player == null)
        {
            return;
        }

        Transform spawnPoint = arrowSpawnPoint != null ? arrowSpawnPoint : transform;
        Vector2 direction = (Vector2)player.position - (Vector2)spawnPoint.position;
        if (IsPlayerTooClose())
        {
            Vector2 playerDirection = (Vector2)player.position - (Vector2)transform.position;
            if (playerDirection.sqrMagnitude > 0.0001f)
            {
                direction = playerDirection;
            }
        }

        if (direction.sqrMagnitude <= 0.0001f)
        {
            direction = Vector2.right * (facDir == 0 ? 1 : facDir);
        }

        int directionSign = direction.x >= 0f ? 1 : -1;
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

        GameObject spawnedArrow = Instantiate(arrow, spawnPoint.position, Quaternion.identity);
        ArrowController arrowController = spawnedArrow.GetComponent<ArrowController>();
        if (arrowController != null)
        {
            arrowController.IgnoreCollisionWith(gameObject);
            arrowController.SetDirection(directionSign, angle, arrowSpeed);
        }

        shootingCooldownTimer = shootingCooldown;
    }
}
