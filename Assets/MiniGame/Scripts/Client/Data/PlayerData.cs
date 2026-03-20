using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Profile profile;

    [Serializable]
    public class Profile
    {
        public int uid;
        public string name;
        
    }
}
