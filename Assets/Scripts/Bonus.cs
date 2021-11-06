using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField]private int _countToAdd;

    public int CountToAdd { get => _countToAdd; set => _countToAdd = value; }
}
