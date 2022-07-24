using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiledata : ScriptableObject // 쓸줄몰라서 스테ㅐ스트용
{
    // [SerializeField] //private 쓸때 인 스펙터에서 수정가능도록 하는역할이라고 하던데 직렬화?를 위해ㅣ서던가 잘 모르겠음

    public bool hostile;
    public int life , colife, coldelay;
    public float speed, colsize, xplus, yplus, dxplus, dyplus, direcplus;
    public string spritepath;
    
}
