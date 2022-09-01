using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashevent : MonoBehaviour
{
    //쓰레기에서 일어나는 일

    //조건부
   


    //실행부
    public enum typename : int
    {
        spawnunit = 1, setunitstat , orderunit , spawnprojectile, setprojectilestat
    }


    public typename type;
    public List<int> i = new List<int>();
    public List<float> f = new List<float>();
    public List<string> s = new List<string>();
 

    public void proc()
    {

    }
    

    public bool execute() //조건상관없이 바로실행
    {
        return true;
    }
}
