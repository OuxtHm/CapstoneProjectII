using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("1��������")]
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

    //[Header("2��������")]


    //[Header("3��������")]

    private void Awake()
    {
        audioSource = this.gameObject.GetComponentInParent<AudioSource>();
    }

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

    public void Sounds(string sounds)
    {
        switch (sounds)
        {
            case "slimeMove":
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
        }

        audioSource.Play();
    }
}