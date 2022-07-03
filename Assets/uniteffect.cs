using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uniteffect : MonoBehaviour
{
    public enum typename : int
    {
        damage = 1, heal
    }

    public typename type;
    public int delay = 0, time = 1, interval = 0; //선딜, 적용 횟수, 적용간 시간(time이 2이상일떄 유효)
    public bool active = false;
    public int d, t, it; //저장용
    public int[] i;
    public float[] f;
    public string[] s;
    public Unit u = null;

    public uniteffect(typename _type, int _dealy, int _time, int _interval, int[] _i, float[] _f, string[] _s)
    {
        type = _type;
        delay = _dealy;
        time = _time;
        interval = _interval;
        if(_i != null)
        {
            i = (int[])_i.Clone();
        }
        if (_f != null)
        {
            f = (float[])_f.Clone();
        }
        if (_s != null)
        {
            s = (string[])_s.Clone();                
        }
    }




    private void FixedUpdate()
    {
        if (active)
        {
            if (time <= 0)
            {
                Destroy(this);
                return;
            }

            if (delay <= 0) //발동 처리
            {
                affect(u, type, i, f, s);

                delay = interval;
                time--;
            }
            else
            {
                delay--;
            }
        }
    }


    public bool add(Unit dest) //자신을 복사해서 대상 유닛에 추가
    {
        if (dest == null)
        {
            return false;
        }

        //복사+추가
        uniteffect buf = dest.gameObject.AddComponent<uniteffect>();
        buf.copy(this);
        buf.u = dest;
        return true;
    }

    public void activate()
    {
        d = delay;
        t = time;
        it = interval;
        active = true;
    }

    public static bool affect(Unit dest, typename type, int[] i, float[] f, string[] s) //내용만 적용하는
    {
        if (dest == null)
        {
            return false;
        }



        switch (type)
        {
            case typename.damage:
                {
                    if (i == null)
                    {
                        return false;
                    }
                    int dmg = i[0];
                    if (dmg > 0)
                    {
                        dest.hp -= dmg;
                    }

                }
                break;


            default:
                return false;
        }


        return true;
    }

    public void copy(uniteffect dest)
    {
        if(dest == null)
        {
            return;
        }

        type = dest.type;
        i = (int[])dest.i.Clone();
        f = (float[])dest.f.Clone();
        s = (string[])dest.s.Clone();
        delay = dest.delay;
        interval = dest.interval;
        time = dest.time;
    }
}
