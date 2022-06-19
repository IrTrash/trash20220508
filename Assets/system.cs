using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class system : MonoBehaviour
{
    public int left = -20, top = 20, right = 20, bottom = -20;


    astar _astar;



    private void Start()
    {
        _astar = new astar(left, top, right, bottom);
        _astar.setblocked(1, 0);
        _astar.setblocked(1, 1);
        _astar.setblocked(2, 1);
        _astar.setblocked(2, 2);

        Unit.direc[] d = _astar.findway(3, 2, 0, 0);


        int n = 1;
        if (d != null)
        {
            foreach(Unit.direc direc in d)
            {
                system.print(n++ + " : " + direc);
            }
        }

    }




    private void FixedUpdate()
    {
        
    }




    public static int tileval(float f)
    {
        int i = (int)f;
        float dis = i - i;

        if(dis >= 0.5f)
        {
            return i + 1;
        }
        else if(dis <= - 0.5f)
        {
            return i - 1;
        }


        return i;
    }

    public static int tilex(float x) => tileval(x);
    public static int tiley(float y) => tileval(y);

    public static int tiledistance(int x1, int y1, int x2, int y2) => Mathf.Abs(tilex(x1) - tilex(x2)) + Mathf.Abs(tilex(y1) - tilex(y2));


    public static float centerdistance_x(float x) => tilex(x) - x;
    public static float centerdistance_y(float y) => tiley(y) - y;

    public static bool isin(float x, float y, float x1, float y1, float x2, float y2) => (x1 - x) * (x2 - x) <= 0 && (y1 - y) * (y2 - y) <= 0;
    
}
