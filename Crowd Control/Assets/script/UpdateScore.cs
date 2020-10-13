using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    private void OnDestroy()
    {
        //increase score thread safe
        lock (Canvas_T.b)
        {
            Canvas_T.score += 1;
        }
    }
}
