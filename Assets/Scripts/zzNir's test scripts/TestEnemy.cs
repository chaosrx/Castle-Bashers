﻿using UnityEngine;
using System.Collections;

public class TestEnemy : Enemy
{
    public GameObject attackCollider;
    private GameObject attCol;
    public Type classification;

    // Use this for initialization
    void Start()
    {
        base.Start();
        speed = 4;
    }

    // Update is called once per frame
    void Update()
    {


        if (target != null)
        {


            if (!isStunned)
            {
                Act(classification);

                if (distL <= attackRange || distR <= attackRange)
                Attack();
            }
        }
        else
        {
            if (FindObjectOfType<Player>())
                target = FindObjectOfType<Player>().gameObject;
            else
            {
                //player lost
                //Destroy(gameObject);
            }
        }
        if (stunTimer > 0)
            stunTimer -= Time.deltaTime;
        else
            isStunned = false;

        if (invTime <= 0)
            isInvincible = false;

        invTime -= Time.deltaTime;
    }

    private void Attack()
    {
        bool facing = distL <= distR;
        isStunned = true;
        stunTimer = 1f;
        if (facing)
        {
            attCol = Instantiate(attackCollider, transform.position + right, transform.rotation) as GameObject;
        }
        else
        {
            attCol = Instantiate(attackCollider, transform.position + left, transform.rotation) as GameObject;
        }
        Destroy(attCol, 0.5f);

    }

}
