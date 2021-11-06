using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField]private int countToAdd;

    public int CountToAdd { get => countToAdd; set => countToAdd = value; }
}
