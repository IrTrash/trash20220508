using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionadd : MonoBehaviour
{
    public Unit.action.typename type = 0;
    public int[] i;
    public float[] f;
    public string[] s;
    public Unit u = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (u == null)
        {
            u = gameObject.GetComponent<Unit>();
        }

        if (u != null && type != 0)
        {
            u.addaction(type, i, f, s);
            Destroy(this);
        }
    }
}
