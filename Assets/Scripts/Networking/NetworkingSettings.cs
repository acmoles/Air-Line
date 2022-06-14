using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NetworkingSettings", menuName = "Utils/NetworkingSettings")]
public class NetworkingSettings : ScriptableObject
{
    public bool isMasterClient = true;
}
