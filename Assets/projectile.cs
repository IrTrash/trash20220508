using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public enum colshapename : int
    {
        circle = 1
    }


    //투사체
    public bool hostile = true;//적대적이냐(공격이냐)
    public int life = 20 , team=1;
    public float x, y, speed = 1, size = 0.5f, direction = 0;
    public int coldelay = 1 , collife = 1;
    public colshapename colshape = colshapename.circle;
    public Rigidbody2D rb;
    public Unit owner;
    Dictionary<Unit, int> collist = new Dictionary<Unit, int>();
    public List<uniteffect> effectlist = new List<uniteffect>();


    private void Start()
    {
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;            
        }

        Debug.Log(x + " : " + y);
    }


    private void FixedUpdate()
    {

        proc();
    }

    private void proc()
    {
        if (life-- <= 0)
        {
            Destroy(gameObject);
            return;
        }

        
        float dt = Time.deltaTime;
        if (speed > 0 && dt > 0)
        {            
            float deltaspeed = dt * speed;
            float xd = Mathf.Cos(direction), yd = Mathf.Sin(direction);
            x += deltaspeed * xd / (Mathf.Abs(xd) + Mathf.Abs(yd));
            y += deltaspeed * yd / (Mathf.Abs(xd) + Mathf.Abs(yd));
        }

        try
        {            
            gameObject.transform.position = new Vector3(x, y, 0);
        }
        catch
        {
            Debug.LogError(x + " , " + y + " , " + speed);
        }
        
        updatedirec();

        //충돌 판정        
        List<Unit> units = new List<Unit>();
        switch (colshape)
        {
            case colshapename.circle:
                Unit[] uarray = circlecheck(size);
                if(uarray != null)
                {
                    units = new List<Unit>(uarray);
                }                
                break;
        }

        foreach(KeyValuePair<Unit,int> kv in collist)
        {            
            if(--collist[kv.Key] <= 0)
            {
                collist.Remove(kv.Key);
            }
        }


        foreach (Unit u in units)
        {
            if(collist.ContainsKey(u))
            {
                continue;
            }

            collist.Add(u, coldelay);
            //처리
            
            foreach(uniteffect ue in effectlist)
            {
                ue.add(u);
            }
            
            if(--collife  <= 0)
            {
                Destroy(gameObject);
            }
        }




    }


    void updatedirec()
    {
        if(rb != null)
        {
            rb.rotation = direction * Mathf.Rad2Deg - 90;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        
    }

    public Unit[] circlecheck(float size)
    {
        if(size <= 0)
        {
            return null;
        }

        List<Collider2D> clist = new List<Collider2D>(Physics2D.OverlapCircleAll(new Vector2(x, y), size));
        if(clist.Count <= 0)
        {
            return null;
        }

        List<Unit> r = new List<Unit>();
        foreach(Collider2D c in clist)
        {
            Unit u = c.gameObject.GetComponent<Unit>();

            if(u != null && owner != u && c.gameObject != gameObject)
            {
                if ((!hostile && u.team != team) || (hostile && u.team == team))
                {
                    continue;
                }                
                r.Add(u);
            }
        }

        return r.ToArray();
    }


    public static float getdirec_2points(float x1, float y1, float x2, float y2) => Mathf.Atan2(y2 - y1, x2 - x1);
    
}



