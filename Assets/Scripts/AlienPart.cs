using UnityEngine;

[CreateAssetMenu(fileName = "AlienPart", menuName = "Scriptable Objects/AlienPart")]
public class AlienPart : ScriptableObject
{
    public string partName;
    public AlienBodyPart bodyPart;
    public int amount;
    

}
