using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> CameraPositions = new List<Transform>();
    public CameraPosition currentPosition;
    public List<Button> moveButtons = new List<Button>();
    public List<CameraPosition> cameraHistory = new List<CameraPosition>();
    public MoveEffect nod;
    public MoveEffect shakeHead;

    private void Start()
    {
        currentPosition = CameraPosition.middle;
        foreach (Button button in moveButtons)
        {
            int i = moveButtons.IndexOf(button);
            button.onClick.AddListener(() => PressedButton(i));
            button.enabled = true;
        }

    }
    
    public void PressedButton(int index)
    {
        
        
        foreach (Button button in moveButtons)
        {
            button.enabled = false;
            button.image.color = Color.black;
            
        }
        if (currentPosition == CameraPosition.middle)
        {
            if (index == 0)
            {
                moveButtons[2].enabled = true;
                moveButtons[2].image.color = Color.white;
                currentPosition = CameraPosition.right;
                Camera.main.transform.position = CameraPositions[1].position;
            }
            else if (index == 1) 
            {
                moveButtons[3].enabled = true;
                moveButtons[3].image.color = Color.white;
                currentPosition = CameraPosition.down;
                Camera.main.transform.position = CameraPositions[2].position;
            }
            else if (index == 2)
            {
                moveButtons[0].enabled = true;
                moveButtons[0].image.color = Color.white;
                currentPosition = CameraPosition.left;
                Camera.main.transform.position = CameraPositions[3].position;

            }
            else if (index == 3)
            {
                moveButtons[1].enabled = true;
                moveButtons[1].image.color = Color.white;
                currentPosition = CameraPosition.up;
                Camera.main.transform.position = CameraPositions[4].position;

            }
            cameraHistory.Add(currentPosition);
            if (cameraHistory.Count > 3)
            {
                cameraHistory.RemoveAt(0);
            }
            CheckCameraHistory();
            return;
            
        }
        
        currentPosition = CameraPosition.middle;
        Camera.main.transform.position = CameraPositions[0].position;
        foreach (Button button in moveButtons)
        {
            button.image.color = Color.white;
            button.enabled = true;
        }
        cameraHistory.Add(currentPosition);
        if(cameraHistory.Count > 3)
        {
            cameraHistory.RemoveAt(0);
        }
        CheckCameraHistory();
        
    }
    public void CheckCameraHistory()
    {
        int depth = 3;
        if(cameraHistory.Count < depth)
        {
            Debug.Log("Not enought camera History");
            return;
        }

        CheckMovement(nod, 3);
        CheckMovement(shakeHead, 3);
        /*foreach(CameraMove moveEffect in nod.cameraMoves)
        {
            int i = 0;
            int check= 0;
            foreach (CameraPosition cameraPosition in moveEffect.movePositions)
            {
                Debug.Log(cameraPos.Count);
                Debug.Log(i);
                if (cameraPosition == cameraPos[i])
                {
                    check++;
                }
                else
                {
                    break;
                }
                if(check == depth)
                {
                    Debug.Log("Nodding detected");
                }
                    i++;
            }
        }*/
    }
    public void CheckMovement(MoveEffect effect,int depth)
    {
        List<CameraPosition> cameraPos = new List<CameraPosition>();
        for (int i = 0; i < cameraHistory.Count; i++)
        {
            cameraPos.Add(cameraHistory[i]);
        }
        foreach (CameraMove moveEffect in effect.cameraMoves)
        {
            int i = 0;
            int check = 0;
            foreach (CameraPosition cameraPosition in moveEffect.movePositions)
            {
                Debug.Log(cameraPos.Count);
                Debug.Log(i);
                if (cameraPosition == cameraPos[i])
                {
                    check++;
                }
                else
                {
                    break;
                }
                if (check == depth)
                {
                    Debug.Log(effect.moveName);
                }
                i++;
            }
        }
    }

    
}
public enum CameraPosition
{
    left, right, up, down, middle
}
[System.Serializable]
public class MoveEffect
{
    public List<CameraMove> cameraMoves = new List<CameraMove>();
    public string moveName;
}
[System.Serializable]
public class CameraMove
{
    public List<CameraPosition> movePositions = new List<CameraPosition>();
}

