using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitweapon : MonoBehaviour
{
    public int charge = 10, delay = 10, cooldown = 10 , nominalrange = 2; 
    public Unit owner;
    public List<GameObject> projobjectlist = new List<GameObject>();


    public enum statename : int
    {
        idle = 1, charge, cooldown
    }
    public statename state = statename.idle;
    public int statetime = 0;
    public float x = 0, y = 0;


    private void Start()
    {
        if(owner == null)
        {
            owner = gameObject.GetComponent<Unit>();
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case statename.idle:
                break;

            case statename.charge:
                {
                    if(statetime-- <= 0)
                    {
                        createproj(x, y);
                        state = statename.cooldown;
                        statetime = cooldown;
                    }
                }
                break;

            case statename.cooldown:
                {
                    if(statetime-- <= 0)
                    {
                        state = statename.idle;
                        statetime = 0;
                    }
                }
                break;
        }
        
    }

    public bool createproj(float dx, float dy)
    {
        if(owner == null)
        {
            return false;
        }


        GameObject objbuf;

        foreach(GameObject projobj in projobjectlist)
        {
            if(projobj == null)
            {
                continue;
            }

            objbuf = GameObject.Instantiate(projobj);

            projectile prj = objbuf.GetComponent<projectile>();
            if(prj != null)
            {                
                prj.owner = owner;
                prj.team = owner.team;
                prj.x = owner.x;
                prj.y = owner.y;
                prj.direction = projectile.getdirec_2points(prj.x, prj.y, dx, dy);
                Debug.Log("proj created : " + prj.x + " , " + prj.y);
            }
            else //자동으로 찾아서 할까? 뭔가 불필요한 짓을 하는거같아서 일단 보류
            {

            }

        }



        return true;
    }


    public bool launch(float dx, float dy)
    {
        if(state != statename.idle)
        {
            return false;
        }

        state = statename.charge;
        statetime = charge;
        x = dx;
        y = dy;
        return true;
    }

    public void stoplaunch() //자발적 중단 쿨은 적용안되게
    {
        state = statename.idle;
        statetime = 0;
    }
}
