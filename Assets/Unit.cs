using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum direc : int
    {
        left = 1, right, up , down = 4
    }


    public class action
    {
        public enum typename : int
        {
            stop = 1, wait, move, move1tile, useweapon, movedest, approachdest,
        }

        public action(typename _type, int[] _i, float[] _f,string[] _s)
        {
            type = _type;
            if(_i != null)
            {
                i = (int[])_i.Clone();
            }
            if(_f != null)
            {
                f = (float[])_f.Clone();
            }
            if(_s != null)
            {
                s = (string[])_s.Clone();
            }
        }

        public bool started = false;
        public typename type;
        public int[] i;
        public float[] f;
        public string[] s;
    }



    public enum statename : int
    {
        idle = 0, move, charge, cast, delay
    }



    public Rigidbody2D rb;



    public int maxhp = 10, hp = 10;
    public float x, y, z = 0, speed = 1;
    public statename state = statename.idle;
    public bool canact = true, moving = false;
    public direc direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void FixedUpdate()
    {
        proc();
        actionproc();


        gameObject.transform.position = new Vector3(x, y, z); //이게 낫나?
    }


    private void proc() //스탯 처리 등
    {
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }

        state = statename.idle;
        if(canact)
        {
            if(moving && speed > 0)
            {

                float deltaspeed = speed * Time.fixedDeltaTime;

                float xs = 0, ys = 0;
                switch (direction)
                {
                    case direc.left:
                        xs = -deltaspeed;
                        break;
                    case direc.right:
                        xs = deltaspeed;
                        break;
                    case direc.up:
                        ys = deltaspeed;
                        break;
                    case direc.down:
                        ys = -deltaspeed;
                        break;
                }
                x += xs;
                y += ys;
                moving = false;
                state = statename.move;
            }

        }



    }

    public action currentaction = null, pushedaction = null;
    public List<action> actionlist = new List<action>();
    private void actionproc() //행동 처리
    {
        if(currentaction != null)
        {
            bool complete = false;
            switch (currentaction.type)
            {
                case action.typename.stop:
                    {
                        currentaction = null;
                        pushedaction = null;
                        actionlist.Clear();
                        complete = true;
                    }
                    break;
                case action.typename.wait:
                    {
                        if(currentaction.i == null)
                        {
                            complete = true;
                            break;
                        }
                        else if(currentaction.i[0]-- <= 0)
                        {
                            complete = true;
                            break;
                        }
                    }
                    break;


                case action.typename.move: //방향중 하나로 일정 프레임동안이동
                    {
                        
                        if(!currentaction.started)
                        {
                            if(currentaction.i == null)
                            {
                                complete = true;
                                break;
                            }
                            else if(currentaction.i.Length < 2)
                            {
                                complete = true;
                                break;
                            }
                            Debug.Log("move-started");
                        }
                        else
                        {
                            if(currentaction.i[1]-- <= 0)
                            {
                                complete = true;
                                break;
                            }
                            else
                            {
                                moving = true;                                
                                direction = (direc)currentaction.i[0];                                
                            }
                        }
                    }
                    break;

                case action.typename.move1tile: //일단 이 행동 자체는 가급적이면 타일의 중앙까지 이동하도록 함... 그 와중에 뭐에 막히거나 중지해야하면 바로 할수있도록
                    {
                        if(!currentaction.started) //필요 정보 : 방향(integer)
                        {
                            if(currentaction.i == null)
                            {
                                complete = true;
                                break;
                            }
                            else if(speed <= 0 || currentaction.i[0] < 0 || currentaction.i[0] > (int)direc.down)
                            {
                                complete = true;
                                break;
                            }                            

                            currentaction.i = new int[] { system.tilex(x), system.tiley(y) , currentaction.i[0]};
                            
                        }
                        else
                        {
                            if (system.tiledistance(system.tilex(x), system.tiley(y), currentaction.i[0], currentaction.i[1]) > 1)
                            {
                                complete = true;
                                break;
                            }

                            if(system.tilex(x) != currentaction.i[0] || system.tiley(y) != currentaction.i[1])
                            {

                                float deltaspeed = speed * Time.fixedDeltaTime;
                                direc d = (direc)currentaction.i[2];
                                if(d == direc.left || d == direc.right)
                                {
                                    complete = Mathf.Abs(system.centerdistance_x(x)) < deltaspeed;
                                }
                                else if(d == direc.down || d == direc.up)
                                {
                                    complete = Mathf.Abs(system.centerdistance_y(y)) < deltaspeed;
                                }                               

                                if(complete)
                                {
                                    break;
                                }    
                            }


                            moving = true;
                            direction = (direc)currentaction.i[2];
                        }
                            
                    }
                    break;
            }

            if(!currentaction.started)
            {
                currentaction.started = true;
            }

            if (complete == true)
            {
                currentaction = null;
            }
        }
        else
        {
            if(pushedaction != null) //임시저장(잠시 미뤄둔 것)이 있으면
            {
                currentaction = pushedaction;
                pushedaction = null;
            }
            else if(actionlist.Count > 0) //후입 선출
            {
                action buf = actionlist[actionlist.Count - 1];
                currentaction = buf;
                actionlist.Remove(buf);
            }
        }
    
    }
    
    public bool addaction(action dest)
    {
        if(dest == null)
        {
            return false;
        }
        else if(dest.type == 0)
        {
            return false;
        }

        if (actionlist.Count < 1)
        {
            if (currentaction == null && pushedaction == null)
            {
                currentaction = dest;
            }
            else
            {
                actionlist.Add(dest);
            }
        }
        else
        {
            actionlist.Add(dest);
        }

        return true;
    }

    public bool addaction(action.typename atype, int[] i, float[] f, string[] s) => addaction(new action(atype, i, f, s));

}
