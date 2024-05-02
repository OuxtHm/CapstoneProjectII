using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("1스테이지")]
    public AudioClip slimeMove;
    public AudioClip slimeDie;
    public AudioClip canineAttack;
    public AudioClip canineDie;
    public AudioClip goblinAttack;
    public AudioClip goblinDie;
    public AudioClip frogAttack1;
    public AudioClip frogIdle;
    public AudioClip frogDie;
    public AudioClip rabbitAttack;
    public AudioClip rabbitDie;
    public AudioClip ranger_Attack;
    public AudioClip ranger_Arrow;
    public AudioClip ranger_ArrowRain;
    public AudioClip ranger_SpAttack;

    [Header("2스테이지")]
    public AudioClip frog_Attack2;
    public AudioClip bat_Die;
    public AudioClip golem_Attack;
    public AudioClip golem_Die;
    public AudioClip skeleton_Attack;
    public AudioClip skeleton_Die;
    public AudioClip ramses_Move;
    public AudioClip knight_Attack1;
    public AudioClip knight_Attack2;
    public AudioClip knight_After;
    public AudioClip knight_Light;


    //[Header("3스테이지")]

    private void Awake()
    {
        audioSource = this.gameObject.GetComponentInParent<AudioSource>();
    }

    //1스테이지
    public void SlimeMove()
    {
        Sounds("slimeMove");
    }
    public void SlimeDie()
    {
        Sounds("slimeDie");
    }
    public void CanineAttack()
    {
        Sounds("canineAttack");
    }
    public void CanineDie()
    {
        Sounds("canineDie");
    }
    public void GoblinAttack()
    {
        Sounds("goblinAttack");
    }
    public void GoblinDie()
    {
        Sounds("goblinDie");
    }
    public void FrogAttack1()
    {
        Sounds("frogAttack1");
    }
    public void FrogIdle()
    {
        Sounds("frogIdle");
    }
    public void FrogDie()
    {
        Sounds("frogDie");
    }
    public void RabbitAttack()
    {
        Sounds("rabbitAttack");
    }
    public void RabbitDie()
    {
        Sounds("rabbitDie");
    }
    public void Rnager_Attack()
    {
        Sounds("ranger_Attack");
    }
    public void Ranger_Arrow()
    {
        Sounds("ranger_Arrow");
    }
    public void Ranger_ArrowRain()
    {
        Sounds("ranger_ArrowRain");
    }
    public void Ranger_SpAttack()
    {
        Sounds("ranger_SpAttack");
    }

    //2스테이지
    public void Frog_Attack2()
    {
        Sounds("frog_Attack2");
    }
    public void Bat_Die()
    {
        Sounds("bat_Die");
    }
    public void Golem_Attack()
    {
        Sounds("golem_Attack");
    }
    public void Golem_Die()
    {
        Sounds("golem_Die");
    }
    public void Skeleton_Attack()
    {
        Sounds("skeleton_Attack");
    }
    public void Skeleton_Die()
    {
        Sounds("skeleton_Die");
    }
    public void Ramses_Move()
    {
        Sounds("ramses_Move");
    }
    public void Knight_Attack1()
    {
        Sounds("knight_Attack1");
    }
    public void Knight_Attack2()
    {
        Sounds("knight_Attack2");
    }
    public void Knight_After()
    {
        Sounds("knight_After");
    }
    public void Knight_Light()
    {
        Sounds("knight_Light");
    }


    public void Sounds(string sounds)
    {
        switch (sounds)
        {
            case "slimeMove":   //1스테이지
                audioSource.clip = slimeMove;
                break;
            case "slimeDie":
                audioSource.clip = slimeDie;
                break;
            case "canineAttack":
                audioSource.clip = canineAttack;
                break;
            case "canineDie":
                audioSource.clip = canineDie;
                break;
            case "goblinAttack":
                audioSource.clip = goblinAttack;
                break;
            case "goblinDie":
                audioSource.clip = goblinDie;
                break;
            case "frogAttack1":
                audioSource.clip = frogAttack1;
                break;
            case "frogIdle":
                audioSource.clip = frogIdle;
                break;
            case "frogDie":
                audioSource.clip = frogDie;
                break;
            case "rabbitAttack":
                audioSource.clip = rabbitAttack;
                break;
            case "rabbitDie":
                audioSource.clip = rabbitDie;
                break;
            case "ranger_Attack":
                audioSource.clip = ranger_Attack;
                break;
            case "ranger_Arrow":
                audioSource.clip = ranger_Arrow;
                break;
            case "ranger_ArrowRain":
                audioSource.clip = ranger_ArrowRain;
                break;
            case "ranger_SpAttack":
                audioSource.clip = ranger_SpAttack;
                break;
            case "frog_Attack2":    //2스테이지
                audioSource.clip = frog_Attack2;
                break;
            case "bat_Die":
                audioSource.clip = bat_Die;
                break;
            case "golem_Attack":
                audioSource.clip = golem_Attack;
                break;
            case "golem_Die":
                audioSource.clip = golem_Die;
                break;
            case "skeleton_Attack":
                audioSource.clip = skeleton_Attack;
                break;
            case "skeleton_Die":
                audioSource.clip = skeleton_Die;
                break;
            case "ramses_Move":
                audioSource.clip = ramses_Move;
                break;
            case "knight_Attack1":
                audioSource.clip = knight_Attack1;
                break;
            case "knight_Attack2":
                audioSource.clip = knight_Attack2;
                break;
            case "knight_After":
                audioSource.clip = knight_After;
                break;
            case "knight_Light":
                audioSource.clip = knight_Light;
                break;
        }

        audioSource.Play();
    }
}
