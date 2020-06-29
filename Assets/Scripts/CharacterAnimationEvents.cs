using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;
    Character target;
    [SerializeField] ParticleSystem smoke;
    [SerializeField] Transform gun;

    // Start is called before the first frame update
    void Start() 
    {
        character = GetComponentInParent<Character>();
        
    }


    void Shoot()
    {
        Destroy(Instantiate(smoke, gun),0.1f);
        Debug.Log("Shoot");
    }
    void GetHit()
    {
        if (character.target)
            target = character.target.GetComponent<Character>();
        if (target)
        {

            target.SetState(Character.State.Dead);
        }
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
        
    }

    void ShootEnd()
    {
        if (character.target)
        { 
            character.SetState(Character.State.BeginShoot);
           

        }
           
        else
            character.SetState(Character.State.Idle);
    }

    void ShootFromIdle()
    {
        if(character.attack && character.weapon==Character.Weapon.Pistol && character.enemiesLeft)
            character.SetState(Character.State.BeginShoot);
    }
}
