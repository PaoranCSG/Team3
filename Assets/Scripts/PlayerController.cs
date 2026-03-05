using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> CameraPositions = new List<Transform>();
    public CameraPosition currentPosition;
    public List<Button> moveButtons = new List<Button>();

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
           
            return;
            
        }
        
        currentPosition = CameraPosition.middle;
        Camera.main.transform.position = CameraPositions[0].position;
        foreach (Button button in moveButtons)
        {
            button.image.color = Color.white;
            button.enabled = true;
        }

    }
    

    
}
public enum CameraPosition
{
    left, right, up, down, middle
}

