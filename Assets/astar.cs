using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class astar
{
    public class pos
    {
        public pos(int _x, int _y)
        {
            x = _x;
            y = _y;
            g = 0;
            h = 0;

        }
        public int x, y;
        public float  g, h;
        public bool blocked = false;
        public pos parent = null;

        public float f => g + h;
        

        public void clear()
        {
            g = 0;
            h = 0;
            parent = null;
        }
    }

    List<pos> tiles;

    public astar(int left, int right, int top, int bottom)
    {
        if (left >= right || top <= bottom)
        {
            return;
        }
        tiles = new List<pos>();
        for(int n = left; n < right; n++)
        {
            for(int m = bottom; m < top; m++)
            {
                tiles.Add(new pos(n, m));                
            }
        }
    }

    public bool setblocked(int dx, int dy)
    {
        pos t = getpos(dx, dy);
        if(t == null)
        {
            return false;
        }


        t.blocked = true;
        return true;
    }


    public pos getpos(int dx,int dy)
    {
        foreach(pos t in tiles)
        {
            if(t.x == dx && t.y == dy)
            {
                return t;
            }
        }

        return null;
    }

    public Unit.direc[] findway(int sx, int sy, int dx, int dy)
    {

        pos start = getpos(sx, sy), end = getpos(dx, dy);
        if(start == null || end == null)
        {
            return null;
        }
        else if(start.blocked || end.blocked)
        {
            return null;
        }


        List<pos> closed = new List<pos>(), opened = new List<pos>
        {
            start
        };

        pos current;
        float g;

        while(opened.Count > 0)
        {
            current = null;
            foreach(pos o in opened)
            {
                if(current == null)
                {
                    current = o;
                }
                else
                {
                    if(current.f > o.f)
                    {
                        current = o;
                    }
                }
            }

            opened.Remove(current);
            closed.Add(current);

            if(closed.Contains(end))
            {
                break;
            }


            pos[] candidate = { getpos(current.x - 1, current.y), getpos(current.x + 1, current.y), getpos(current.x, current.y + 1), getpos(current.x, current.y - 1) };
            g = current.g + 10;
            foreach(pos c in candidate)
            {
                if(c == null)
                {
                    continue;
                }

                if(closed.Contains(c) || c.blocked)
                {
                    continue;
                }

                if(!opened.Contains(c))
                {
                    c.g += g;
                    c.h = (Mathf.Abs(dx - c.x) + Mathf.Abs(dy - c.y)) * 10;
                    c.parent = current;
                    opened.Add(c);
                }
                else
                {
                    if(g + c.h < c.f)
                    {
                        c.g = g;
                        c.parent = current;
                    }
                }
                
            }
        }


        List<Unit.direc> r = new List<Unit.direc>();
        current = end;
        while(current != null)
        {
            if(current.parent == null)
            {
                break;
            }
            int x1 = current.x, y1 = current.y, x2 = current.parent.x, y2 = current.parent.y;

            Unit.direc direc = 0;
            if(x1 == x2)
            {
                if (y1 > y2)
                {
                    direc = Unit.direc.up;
                }
                else if (y1 < y2)
                {
                    direc = Unit.direc.down;
                }
                else
                {
                    direc = 0;
                }
            }
            else
            {
                if(x1 > x2)
                {
                    direc = Unit.direc.right;
                }
                else
                {
                    direc = Unit.direc.left;
                }
            }

            r.Add(direc);
            current = current.parent;
        }

        
        foreach(pos p in tiles)
        {
            p.clear();
        }

        r.Reverse();
        return r.ToArray();
    }
}
