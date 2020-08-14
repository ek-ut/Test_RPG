using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Being : Basic_Element
{
    // основные характеристики персонажа
    protected float speedMove; //скорость передвижения
    protected int MaxXP; //количество здоровя персонажа
    protected int currentXP; //текущее количество здоровя персонажа
    protected bool Dead; // индекатор что юнит убит
    protected float jampPower; //сила прыжка
    public Texture2D StripXP;
    protected int SpeedRegeneration = 100;// скорость регинерации здоровья
    protected int currentRegeneration = 0;// текущее количество регинерации здоровья
    protected int SpeedDead = 100;// таймер для перераждения
    protected int currentDead = 0;// текущее значение для перераждения
    protected int maxDamag = 8; // максимальный урн который может нанести


    protected bool AgentMuve = false; //совершаеться движение к точке
    protected int AFCTimer = 0;// таймер для бездействия

    //параметры геймплея персонажа
    protected float gravityForce; //гравитация персонажа
    protected Vector3 moveVector;// вектор движения персонажа

    //Ссылки на компоненты
    protected Animator ch_animator;// ссылка на элемент анимации

    protected NavMeshAgent nmAgent; // агент для перемещения

    protected GameObject Targrt; //цель обекта
    protected bool Attack = false;
    protected int attack_speed;
    protected int current_attack;

    protected Camera MCam;


    //части тела для ударов
    protected  const int miss = 0;
    protected  const int head = 6;
    protected  const int body = 3;
    protected  const int left_hand = 1;
    protected  const int right_hand = 2;
    protected  const int left_leg = 4;
    protected  const int right_leg = 5;

   
        public bool GetDead()
    {
        return Dead;
    }

    protected virtual void Regeneration()
    {
        if(!GetDead() && currentXP < MaxXP)
        {
            currentRegeneration++;
            if(currentRegeneration == SpeedRegeneration)
            {
                currentXP++;
                currentRegeneration = 0;
            }
        }
    }
    protected virtual void CheckDead()
    {
        if (GetDead() )
        {
            currentDead++;
            if (currentDead == SpeedDead)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnGUI()
    {
        if (MCam)
        {
            Vector3 posScr = MCam.WorldToScreenPoint(GetPosition());
            GUI.Box(new Rect(posScr.x - 15, Screen.height - posScr.y - 50, 30, 5), "");
            GUI.DrawTexture(new Rect(posScr.x - 15, Screen.height - posScr.y - 50, (currentXP * 30) / MaxXP, 5), StripXP);
        }
    }


}
