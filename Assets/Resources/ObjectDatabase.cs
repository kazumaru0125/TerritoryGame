using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct ObjectEntry
{
    public ;
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 scale;
         
}

[CreateAssetMenu(fileName = "ObjectDatabase",
                 menuName = "Kondo/DateSetting"
)]

public class ObjectDatabase : ScriptableObject           
{
    public ObjectEntry[] entries;                       
}

