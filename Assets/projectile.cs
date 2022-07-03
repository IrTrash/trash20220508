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
    public int life = 10;
    public float x, y , speed = 1, size = 0.5f, direction = 0;
    public colshapename colshape = colshapename.circle;


    private void FixedUpdate()
    {

        proc();
    }

    private void proc()
    {
        if(life-- <= 0)
        {
            Destroy(gameObject);
            return;
        }


        if(speed > 0)
        {
            float deltaspeed = Time.deltaTime * speed;
            x += Mathf.Cos(direction) * deltaspeed;
            y += Mathf.Sin(direction) * deltaspeed;
        }

        gameObject.transform.position = new Vector3(x, y, 0);


        //충돌 판정

    }
}
