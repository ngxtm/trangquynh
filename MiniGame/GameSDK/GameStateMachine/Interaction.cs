using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

    public enum InteractionType
    {
        Interaction2D,
        Interaction3D
    }
    InteractionType _InteractionType = InteractionType.Interaction2D;

    public class Data
    { 
        public Vector2 ScreenPosition;
        public int touchCount;
        public int touch_ID;
    }
    public Data CheckInteraction (Vector2 _Position, int ID, int count)
    {
        return new Data
        {
            ScreenPosition = _Position,
            touchCount = count,
            touch_ID = ID
        };
    }
}
