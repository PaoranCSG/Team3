
using UnityEngine;
using System.Collections.Generic;

public class MorseCode : MonoBehaviour
{
    public static MorseCode instance;
    
   public List<MorseLetter> morseLetters = new List<MorseLetter>();
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }
}

[System.Serializable]
public class MorseLetter
{
    public string letter;
    public List<bool> morseList = new List<bool>();
}
