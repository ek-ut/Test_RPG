using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hero : Being
{
    private JoistikController jc_con;
    private bool Muve = false ;
    public LayerMask whatCanBeClickedOn;
    private Skeleton TargetSkript;
    private MainGame mgScript;
    //private Camera MCam;



    // Start is called before the first frame update
    void Start()
    {
        MCam = Camera.main;
        ch_animator = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        jc_con = GameObject.FindGameObjectWithTag("Joy").GetComponent<JoistikController>(); //поиск скрипта на другом обекте
        mgScript = MCam.GetComponent<MainGame>();
        gravityForce = 0;
        attack_speed = 250;
        current_attack = attack_speed;
        nmAgent.stoppingDistance = 0.7f;
        speedMove = 3.5f;
        MaxXP = 20;
        currentXP = MaxXP;

        SetRespPoint(GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            HeroMove();
            AFC();
            if (current_attack < attack_speed)
            {
                current_attack++;
            }
            AttacStay();
            Regeneration();
        }
        else
        {

            CheckDead();
        }
    }
    protected override void CheckDead()
    {
        if (GetDead())
        {
            currentDead++;
            if (currentDead == SpeedDead)
            {
                currentDead = 0;
                Dead = false;
                currentXP = MaxXP;
                ch_animator.SetBool("dead", false);
                Resp();
                nmAgent.SetDestination(GetPosition());
            }
        }
    }
    private void MoveTach()// перемищения по клику на карту
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray tRay = MCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(tRay, out hitInfo, 1000, whatCanBeClickedOn))
            {
                nmAgent.SetDestination(hitInfo.point);// перемищение персонажа
                ch_animator.SetBool("move", true);
                AgentMuve = true;
                AFCTimer = 0;
                if (Attack)
                {
                    Attack = false;
                    ch_animator.SetBool("attack", false);
                }
            }
        }
    }
    private void MoveJoy()// перемищения джойстиком
    {
        Vector3 pos = GetPosition();
        moveVector = Vector3.zero;
        moveVector.x = jc_con.Horizontal();
        moveVector.z = jc_con.Vertical();
        if (moveVector.x != 0 || moveVector.z != 0)
        {
            
            AgentMuve = false;
            moveVector.x = moveVector.x + pos.x;
            moveVector.z = moveVector.z + pos.z;
            Muve = true;
            AFCTimer = 0;
            ch_animator.SetBool("move", Muve);
            if(Attack)
            {
                Attack = false;
                ch_animator.SetBool("attack", false);
            }

            moveVector.y = gravityForce;
            nmAgent.SetDestination(moveVector);// перемищение персонажа


        }
        else if (Muve)
        {
            Muve = false;
            ch_animator.SetBool("move", Muve);
        }
    }
    private void HeroMove() //метод для перемещения героя
    {

        MoveTach();
        MoveJoy();

        if (AgentMuve && nmAgent.remainingDistance > 0.0f && nmAgent.remainingDistance < nmAgent.stoppingDistance)
        {
            AgentMuve = false;
            ch_animator.SetBool("move", false);
        }

        
    }
    private void AFC()
    {
        if (!Muve && !AgentMuve && !Attack)
        {
            AFCTimer++;
            if (AFCTimer == 1000)
            {
                ch_animator.SetTrigger("TriggerAFC");
            }
            if (AFCTimer > 1030)
            {
                AFCTimer = 0;

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Monster" && !Attack)
        {
            print("Hero - Кто-то в рыло хочет?");
            Targrt = other.gameObject;
            AttacStart();
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == "Monster" && Attack && Targrt == other.gameObject)
        {
            print("Hero - Эх упустил");
            Attack = false;
            ch_animator.SetBool("attack", false);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster" && !Attack && Targrt != other.gameObject)
        {
            print("Hero - Кто на новенького?");
            Targrt = other.gameObject;
            AttacStart();
        }
    }

    private void AttacStart()
    {

        Attack = true;
        AgentMuve = false;
        Muve = false;
        ch_animator.SetBool("attack", true);
        ch_animator.SetBool("move", false);
        nmAgent.SetDestination(transform.position);
        TargetSkript = Targrt.GetComponent<Skeleton>();
        
    }
    private void AttacStay()
    {
        
        if (Attack )
        {
            Vector3 targetDirection = Targrt.transform.position - GetPosition();
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speedMove, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            if(TargetSkript.GetDead())
            {
                print("Hero - Сдох как собака!!!");
                Attack = false;
                ch_animator.SetTrigger("victori");
                ch_animator.SetBool("attack", false);
                return;
            }

            if (current_attack == attack_speed)
            {
                int strike_target = (int)Random.Range(miss, head+1);
                int Damag = (int)Random.Range(0, maxDamag);

                switch (strike_target)
                {
                    case miss:
                        ch_animator.SetTrigger("hit");
                        break;
                    case left_hand:
                        ch_animator.SetTrigger("hit");
                        break;
                    case right_hand:
                        ch_animator.SetTrigger("hit");
                        break;
                    case body:
                        ch_animator.SetTrigger("hit");
                        break;
                    case left_leg:
                        ch_animator.SetTrigger("hit");
                        break;
                    case right_leg:
                        ch_animator.SetTrigger("hit");
                        break;
                    case head:
                        ch_animator.SetTrigger("HitCrit");
                        break;
                    default:
                        ch_animator.SetTrigger("hit");
                        break;
                }
                TargetSkript.SetDamag(strike_target, Damag);
                current_attack = 0;
            }

        }
    }

    public void SetDamag(int strike_target, int Damag)
    {
        int dam = Damag;

        switch (strike_target)
        {
            case miss:
                ch_animator.SetTrigger("mis");
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
                ch_animator.SetTrigger("crit");
                dam = dam *2;
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
            ch_animator.SetBool("dead", true);
            Attack = false;
            Muve = false;
            AgentMuve = false;
            mgScript.SetDeathsText();
        }
    }

}
