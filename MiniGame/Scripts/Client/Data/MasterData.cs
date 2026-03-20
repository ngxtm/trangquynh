using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterData : MonoBehaviour
{
    public Piece piece;

    [Serializable]
    public class Piece
    {
        public Quan quan;
        public Dan dan;

        public class Quan
        {
            public int score;
        }
        public class Dan
        {
            public int score;
        }
    }

}
