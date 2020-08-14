using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monsters : Being
{
    protected float patrolRadius = 10.0f;
    protected Vector3 patrolPoint;
    protected int patrolTimer = 0;
    protected float SizeAgr = 10.0f;
    protected float SizeAttacs = 2.5f;
    protected float angleSckan = 70.0f;
    private MainGame mgScript;


    private Hero TargetSkript;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        init();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            AFC();
            CheckNewPatrol();
            FinishPatrol();

            if (current_attack < attack_speed)
            {
                current_attack++;
            }
            Scanning();
            Regeneration();
        }else
        {
            CheckDead();
        }
    }

    protected virtual void init()
    {
        MCam = Camera.main;
        ch_animator = GetComponent<Animator>();
        AgentMuve = false;
        AgentMuve = false;
        nmAgent = GetComponent<NavMeshAgent>();
        Dead = false;
        nmAgent.stoppingDistance = 2.5f;
        mgScript = Camera.main.GetComponent<MainGame>();
        attack_speed = 250;
        current_attack = attack_speed;
        MaxXP = 20;
        currentXP = MaxXP;
        SetRespPoint(GetPosition());
        Targrt = GameObject.FindGameObjectsWithTag("Player")[0];
        TargetSkript = Targrt.GetComponent<Hero>();
    }

    protected virtual void CreatePatrolPoint()
    {
        patrolPoint = new Vector3(RespPoint.x + Random.Range(-patrolRadius, patrolRadius), 0, RespPoint.z + Random.Range(-patrolRadius, patrolRadius));
    }

    

    protected virtual void AFC()
    {
        
        if (!AgentMuve)//анимация для бездействия
        {
            AFCTimer++;
            if(AFCTimer == 1000) 
            {
                ch_animator.SetTrigger("TriggerAFC"); 
            }
            if (AFCTimer > 1100)
            {
                AFCTimer = 0;
            }
        }
    }

    protected virtual void StartPatrol()
    {
        nmAgent.SetDestination(patrolPoint);// перемищение персонажа
        ch_animator.SetBool("Move", true);
        AgentMuve = true;
        AFCTimer = 0;
        patrolTimer = 0;
    }

    protected virtual bool  FinishPatrol()
    {
        if (AgentMuve && nmAgent.remainingDistance > 0.0f && nmAgent.remainingDistance < 1.0f)
        {
            AgentMuve = false;
            ch_animator.SetBool("Move", false);
            return true;
        }
        else return false;
    }
    protected virtual void CheckNewPatrol()
    {
        if(!AgentMuve&& AFCTimer < 1000)
        {
            patrolTimer++;
            if(patrolTimer>2000)
            {
                CreatePatrolPoint();
                StartPatrol();
            }
        }
    }

    protected virtual void SetTarget(GameObject target)
    {
        Targrt = target;
    }

    private void Scanning()
    {
        if (Targrt)
        {
            float dis = Vector3.Distance(Targrt.transform.position, GetPosition());
            if(dis < SizeAgr)
            {
                Quaternion look = Quaternion.LookRotation(GetPosition() - Targrt.transform.position);
                float angle = Quaternion.Angle(transform.rotation, look);
                if (180 - angle < angleSckan)
                {
                    if (dis > SizeAttacs)
                    {
                        AttackMuve();
                        Attack = false;
                    }else
                    {
                        AttacStart();
                        AttacStay();
                    }
                }
            }
        }
    }

    protected virtual void AttackMuve()
    {
       
            AgentMuve = true;
            ch_animator.SetBool("Move", true);
            nmAgent.SetDestination(Targrt.transform.position);
        
    }

    protected virtual void AttacStay()
    {
        if (Attack)
        {
            transform.LookAt(Targrt.transform.position);
            if (TargetSkript.GetDead())
            {
                print("монстер - нас стало больше!!!");
                Attack = false;
                ch_animator.SetBool("attack", false);
                return;
            }
            if (current_attack == attack_speed)
            {

                int strike_target = (int)Random.Range(miss, head + 1);
                int Damag = (int)Random.Range(0, maxDamag);
                ch_animator.SetTrigger("hit");
                TargetSkript.SetDamag(strike_target, Damag);
                current_attack = 0;
            }

        }
    }

   

    private void AttacStart()
    {
        if (!Attack)
        {
            Attack = true;
            AgentMuve = false;
            ch_animator.SetBool("attack", true);
            ch_animator.SetBool("move", false);
        }

    }

    public void SetDamag(int strike_target, int Damag)
    {
        int dam = Damag;
        Quaternion look = Quaternion.LookRotation(GetPosition() - Targrt.transform.position);
        float angle = Quaternion.Angle(transform.rotation, look);
        if (angle < 90)
        {
            dam = dam * 3;
        }
            transform.LookAt(Targrt.transform.position);

       
        switch (strike_target)
        {
            case miss:

                break;
            case left_hand:

                break;
            case right_hand:

                break;
            case body:

                break;
            case left_leg:

                break;
            case right_leg:

                break;
            case head:
                dam = dam * 2;
                current_attack = 0;
                break;
            default:

                break;
        }

        currentXP = currentXP - dam;

        if (currentXP < 1)
        {
            currentXP = 0;
            Dead = true;
            Attack = false;
            AgentMuve = false;
            mgScript.SetKilledText();
        }
    }
}