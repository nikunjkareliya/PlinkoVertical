using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegView : MonoBehaviour
{
    public void Interact()
    {
        GameEvents.OnScoreAdded.Execute(10);
    }
}
