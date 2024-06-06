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
    public AudioClip skeleton2s_Attack;
    public AudioClip skeleton2s_Die;
    public AudioClip ramses_Move;
    public AudioClip knight_Attack1;
    public AudioClip knight_Attack2;
    public AudioClip knight_After;
    public AudioClip knight_Light;


    [Header("3스테이지")]
    public AudioClip priest_Punch;
    public AudioClip priest_Rock;
    public AudioClip skeleton3s_Attack;
    public AudioClip skeleton3s_Die;
    public AudioClip flyeye_Die;
    public AudioClip mushroom_Die;
    public AudioClip plant_Shot;
    public AudioClip plant_Slash;
    public AudioClip demon_Attack;
    public AudioClip demon_FireBarrier;
    public AudioClip demon_FireBolt;
    public AudioClip demon_Die;
    public AudioClip demon_Summoning;


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
    public void Skeleton2s_Attack()
    {
        Sounds("skeleton2_Attack");
    }
    public void Skeleton2s_Die()
    {
        Sounds("skeleton2_Die");
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

    //3스테이지
    public void Priest_PunchAttack()
    {
        Sounds("priest_Punch");
    }
    public void Priest_RockAttack()
    {
        Sounds("priest_Rock");
    }
    public void Skeleton3s_Attack()
    {
        Sounds("skeleton3s_Attack");
    }
    public void Skeleton3s_Die()
    {
        Sounds("skeleton3s_Die");
    }
    public void FlyEye_Die()
    {
        Sounds("flyeye_Die");
    }
    public void Mushroom_Die()
    {
        Sounds("mushroom_Die");
    }
    public void ShotPlant_Attack()
    {
        Sounds("plant_Shot");
    }
    public void SlashPlant_Attack()
    {
        Sounds("plant_Slash");
    }
    public void Demon_Attack()
    {
        Sounds("demon_Attack");
    }
    public void Demon_FireBarrier()
    {
        Sounds("demon_FireBarrier");
    }
    public void Demon_FireBolt()
    {
        Sounds("demon_FireBolt");
    }
    public void Demon_Die()
    {
        Sounds("demon_Die");
    }
    public void Demon_Summoning()
    {
        Sounds("demon_Summoning");
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
            case "skeleton2s_Attack":
                audioSource.clip = skeleton2s_Attack;
                break;
            case "skeleton2s_Die":
                audioSource.clip = skeleton2s_Die;
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
            case "priest_Punch":    //3스테이지
                audioSource.clip = priest_Punch;
                break;
            case "priest_Rock":
                audioSource.clip = priest_Rock;
                break;
            case "skeleton3s_Attack":
                audioSource.clip = skeleton3s_Attack;
                break;
            case "skeleton3s_Die":
                audioSource.clip = skeleton3s_Die;
                break;
            case "flyeye_Die":
                audioSource.clip = flyeye_Die;
                break;
            case "mushroom_Die":
                audioSource.clip = mushroom_Die;
                break;
            case "plant_Shot":
                audioSource.clip = plant_Shot;
                break;
            case "plant_Slash":
                audioSource.clip = plant_Slash;
                break;
            case "demon_Attack":
                audioSource.clip = demon_Attack;
                break;
            case "demon_FireBarrier":
                audioSource.clip = demon_FireBarrier;
                break;
            case "demon_FireBolt":
                audioSource.clip = demon_FireBolt;
                break;
            case "demon_Die":
                audioSource.clip = demon_Die;
                break;
            case "demon_Summoning":
                audioSource.clip = demon_Summoning;
                break;
        }

        audioSource.Play();
    }
}
