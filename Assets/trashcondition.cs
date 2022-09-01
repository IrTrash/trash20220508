using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashcondition : MonoBehaviour
{
    //조건
    public enum typename : int
    {
        time_seond
    }
    public typename type;
    public List<int> i = new List<int>();
    public List<float> f = new List<float>();
    public List<string> s = new List<string>();
    //또다른 타입이 필요할수도있음

    

    public bool check()
    {
        switch (type)
        {
            case typename.time_seond:
                {
                    if(i.Count < 1)
                    {
                        return false;
                    }

                   
                }
                break;
        }


        return false; //조건이 잘못됨
    }
    
}
