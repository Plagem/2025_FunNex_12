using System;
using Unity.VisualScripting;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float InitialSpeed = 5f;
    public float Speed = 20f;
    public float AccelerationDuration = 1.0f;
    public float RotateSpeed = 360f;

    private float _delayTime = 0.3f;
    private float _spawnTime;
    
    private PlayerController _owner;
    public Transform _target;
    
    private Vector2 _initialDirection;

    private void Start()
    {
        _spawnTime = Time.time;
        _initialDirection = Vector2.down;
    }
    
    public void Initialize(PlayerController owner, Transform target, Vector2 fireDirection)
    {
        _target = target;
        _owner = owner;
        _initialDirection = fireDirection.normalized;
        transform.right = _initialDirection;
    }
    
    private void Update()
    {
        float elapsed = Time.time - _spawnTime;
        bool delayPassed = elapsed >= _delayTime;
        
        float t = Mathf.Clamp01(elapsed / AccelerationDuration);
        float currentSpeed = Mathf.Lerp(InitialSpeed, Speed, t);
        
        if (_target != null && delayPassed)
        {
            Vector2 directionToTarget = ((Vector2)_target.position - (Vector2)transform.position).normalized;

            // 현재 방향에서 타겟 방향으로 점진적 회전
            float angle = Vector3.SignedAngle(transform.right, directionToTarget, Vector3.forward);
            float maxRotation = RotateSpeed * Time.deltaTime;
            float clampedAngle = Mathf.Clamp(angle, -maxRotation, maxRotation);

            transform.Rotate(0f, 0f, clampedAngle);
        }
        else if(_target == null)
        {
            Destroy(gameObject);
        }

        // 이동 (항상 전방으로)
        transform.position += transform.right * currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_target) return;
        
        if (other.gameObject == _target.gameObject)
        {
            BaseStatComponent ownerStat = _owner.GetComponent<BaseStatComponent>();
            BaseStatComponent targetStat = _target.GetComponent<BaseStatComponent>();
            if (targetStat && ownerStat)
            {
                targetStat.ApplyDamage(ownerStat.GetFinalDamage() * 0.2f);
                targetStat.GetComponent<Rigidbody2D>().AddForce(transform.right * 5f, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }
    }
}
