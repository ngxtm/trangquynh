using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAsset : MonoBehaviour
{
    public static Sprite GetSprite(int ab, string ab_name)
    => Resources.Load<Sprite>("assetbundle_" + ab + "/UI/" + ab_name);

}
