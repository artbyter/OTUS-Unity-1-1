using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        Dead,
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist,
    }

    public Weapon weapon;
    public float runSpeed;
    public float distanceFromEnemy;
    public Transform target;
    State state;
    Animator animator;
    Animator targetAnimator;
    Vector3 originalPosition;
    Quaternion originalRotation;
    public bool enemiesLeft;
    public bool attack;
    List<GameObject> enemies=new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if(target)
            targetAnimator=target.GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        if (enemies.Count > 0)
            enemiesLeft = true;
    }

    public void SetState(State newState)
    {
        state = newState;
    }



    [ContextMenu("Attack")]
    public void AttackEnemy()
    {
        attack = true;
        switch (weapon) {
            case Weapon.Fist:
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;

            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
        }
    }

    

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void ShootEnemy()
    {
        if (enemies.Count > 0)
        {
           
            int rand = Random.Range(0, enemies.Count);

            target = enemies[rand].transform;
            enemies.RemoveAt(rand);
            Vector3 distance = target.position - transform.position;
            Vector3 direction = distance.normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            originalRotation = transform.rotation;

        }
        else
        {
            enemiesLeft = false;
            
            target = null;
            
        }
            
    }

   

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                animator.SetFloat("Speed", 0.0f);
                transform.rotation = originalRotation;
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
         
                if (RunTowards(target.position, distanceFromEnemy))
                    state = State.BeginAttack;
                break;

            case State.BeginAttack:
                if(weapon==Weapon.Bat)
                    animator.SetTrigger("MeleeAttack");
                else if(weapon == Weapon.Fist)
                    animator.SetTrigger("FistAttack");
                state = State.Attack;
                
                break;

            case State.Attack:
                

                break;

            case State.BeginShoot:
                if(enemiesLeft)
                    ShootEnemy();
                animator.SetTrigger("Shoot");
                state = State.Shoot;
                animator.SetTrigger("goToIdle");
                break;

            case State.Shoot:
                
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

           

            case State.Dead:
                animator.SetTrigger("isDead");
                state = State.Dead;
                GetComponentInChildren<Character>().enabled=false;
                break;
        }
    }
}
