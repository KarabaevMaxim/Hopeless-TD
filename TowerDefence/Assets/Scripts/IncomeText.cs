using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeText : MonoBehaviour
{
    void Close()
    {
        Destroy(transform.parent.gameObject);
    }
}
