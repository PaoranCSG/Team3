using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Runtime.Serialization.Formatters;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> CameraPositions = new List<Transform>();
    public GameObject cube;
    public GameObject gameScreen;
    public CameraPosition currentPosition;
    public List<Button> moveButtons = new List<Button>();
    public List<CameraPosition> cameraHistory = new List<CameraPosition>();
    public MoveEffect nod;
    public MoveEffect shakeHead;
    public List<bool> blinks = new List<bool>();
    public GameObject blinkFilter;
    public Button blinkButton;
    public Button wiggleButton;
    public Button jumpButton;
    public TMP_Text morseText;
    public Rigidbody rigidBody;
    public string targetMorseLetter;
    public float jumpForce = 2;
    public bool canJump = true;
    public Combination task1Combination;
    public List<CompletedAction> completedActions = new List<CompletedAction>();

    private void Start()
    {
        canJump = true;
        blinkButton.onClick.AddListener(() => Blink());
        wiggleButton.onClick.AddListener(() => SubmitWiggle());
        jumpButton.onClick.AddListener(() => Jump());
        currentPosition = CameraPosition.middle;
        foreach (Button button in moveButtons)
        {
            int i = moveButtons.IndexOf(button);
            button.onClick.AddListener(() => PressedButton(i));
            button.enabled = true;
        }


    }
    public bool isLongMorse;
    delegate void BlinkTimerDelegate();
    public void Blink()
    {

        blinkFilter.SetActive(!blinkFilter.activeSelf);
        if (blinkFilter.activeSelf)
        {
            isLongMorse = false;
            StartCoroutine(BlinkTimer());
        }
        else
        {
            completedActions.Add(CompletedAction.blink);
            blinks.Add(isLongMorse);
            StopAllCoroutines();
            MorseCodeCheck();
            if (blinks.Count > 4)
            {
                blinks.Clear();
            }

        }

    }
    public void Jump()
    {
        if (canJump)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            StartCoroutine(JumpDelay());
            completedActions.Add(CompletedAction.jump);
        }
        else
        {
            Debug.Log("can not jump");
        }
      
    }
    public IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(2);
        canJump = true;
        yield break;
    }
    public void SubmitWiggle()
    {
        completedActions.Add(CompletedAction.wiggle);
        if(morseText.text == targetMorseLetter)
        {
            Debug.Log("Correct morse Letter!");
        }
        else
        {
            Debug.Log("Wrong morse letter!");
        }
    }
    public void MorseCodeCheck()
    {

        
        bool acceptedResult = false;
        foreach (MorseLetter letter in MorseCode.instance.morseLetters)
        {
            int check = 0;
            for (int i = 0; i < letter.morseList.Count; i++)
            {
                
                if (letter.morseList.Count != blinks.Count)
                {
                    
                    break;
                }
                
                if (letter.morseList[i] == blinks[i])
                {
                    check++;
                    /*Debug.Log("MorseList " +letter.morseList[i]);
                    Debug.Log("blinkValue"+blinks[i]);
                    Debug.Log("CheckCount for Letter "+letter.letter+ " : " + check);
                    Debug.Log(blinks.Count);*/
                    if (check == blinks.Count)
                    {
                        acceptedResult = true;
                        morseText.text = letter.letter;
                           
                        break;
                    }
                }
                else
                {
                    break;
                }
                
                
            }
           
        }
        if (!acceptedResult)
        {
            morseText.text = "Not found";
        }
        
        
        
    }
    public IEnumerator BlinkTimer()
    {
        yield return new WaitForSeconds(1);
        isLongMorse = true;
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
                Camera.main.transform.Rotate(new Vector3(0, 25, 0));
                //Camera.main.transform.position = CameraPositions[1].position;
            }
            else if (index == 1) 
            {
                moveButtons[3].enabled = true;
                moveButtons[3].image.color = Color.white;
                currentPosition = CameraPosition.down;
                gameScreen.transform.Rotate(new Vector3(-8, 0, 0));
                //Camera.main.transform.position = CameraPositions[2].position;
            }
            else if (index == 2)
            {
                moveButtons[0].enabled = true;
                moveButtons[0].image.color = Color.white;
                currentPosition = CameraPosition.left;
                Camera.main.transform.Rotate(new Vector3(0, -25, 0));
                //Camera.main.transform.position = CameraPositions[3].position;

            }
            else if (index == 3)
            {
                moveButtons[1].enabled = true;
                moveButtons[1].image.color = Color.white;
                currentPosition = CameraPosition.up;
                gameScreen.transform.Rotate(new Vector3(10, 0, 0));
                //Camera.main.transform.position = CameraPositions[4].position;

            }
            cameraHistory.Add(currentPosition);
            if (cameraHistory.Count > 3)
            {
                cameraHistory.RemoveAt(0);
            }
            CheckCameraHistory();
            return;
            
        }
        if (currentPosition == CameraPosition.down) 
        {
            gameScreen.transform.Rotate(new Vector3(8, 0, 0));
        }
        else if  (currentPosition == CameraPosition.up)
        {
            gameScreen.transform.Rotate(new Vector3(-10, 0, 0));
        }
        else if (currentPosition == CameraPosition.left)
        {
            Camera.main.transform.Rotate(new Vector3(0, 25, 0));
        }
        else if (currentPosition == CameraPosition.right)
        {
            Camera.main.transform.Rotate(new Vector3(0, -25, 0));
        }
        currentPosition = CameraPosition.middle;
        //Camera.main.transform.position = CameraPositions[0].position;
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
    public void CheckCombination()
    {

    }
    public void CheckMovement(MoveEffect effect,int depth)
    {
        
        List<CameraPosition> cameraPos = new List<CameraPosition>();
        cameraPos.Clear();
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
                //Debug.Log(cameraPos.Count);
                //Debug.Log(i);
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
[System.Serializable]
public class Combination
{
    public List<CompletedAction> completedActions = new List<CompletedAction>();
}

