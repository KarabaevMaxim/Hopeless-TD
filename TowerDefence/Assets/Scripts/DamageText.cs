using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

    public GameObject target;
    Vector3 vec = new Vector3();
    static int count;
    static GameObject targetStatic;
    static float baseOffset = 50;
    static float curOffsetStatic = 0;
    float curOffset = 0;
    void Start ()
    {
        vec = target.transform.position;

        if (targetStatic == target)
        {
            curOffsetStatic += baseOffset / 3;
            curOffset = curOffsetStatic;
        }
        else
        {
            curOffset = baseOffset;
            curOffsetStatic = curOffset;
        }
        targetStatic = target;
        
    }

    void Update()
    {
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(vec);
        if (target != null)
        {
            GetComponent<RectTransform>().position = new Vector3(screenPos.x, screenPos.y + curOffset, 0);
            vec = target.transform.position;
        }
        else
            GetComponent<RectTransform>().position = new Vector3(screenPos.x, screenPos.y + curOffset, 0);
        
    }
    public void kill()
    {
        Destroy(gameObject);
    }
}
