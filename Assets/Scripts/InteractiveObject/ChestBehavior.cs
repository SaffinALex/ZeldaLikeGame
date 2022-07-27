using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestBehavior : MonoBehaviour
{
    public EarnItemBehavior earnItem;
    public Text text;
    public string initialText;
    private bool isOpen;
    private bool playerInRange;
    public float inputDelay;
    public int maximumLetterByLine;
    private float timeLeft = 0;
    private int characterIndex;
    public float delayToDisplayLetter;
    private float lastLetterTime = 0;
    private int nextCharacter = 1;
    private int dialogueIndex = 0;
    private string displayText;
    public AudioClip letterDialogue;
    public AudioClip nextDialogue;
    public AudioClip getItemMusic;
    public AudioClip openChest;
    public float timeBeforeDisplayText;
    public bool canBeOpen;
    public SpriteRenderer placeHolder;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet") && !isOpen)
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeet"))
        {
            playerInRange = false;
        }
    }
    public void Awake()
    {
        isOpen = false;
        timeLeft = 0;
    }
    public void Start()
    {
        text = GameVariables.Instance.dialogueBox.GetComponentInChildren(typeof(Text)) as Text;
        text.text = "";
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerInRange && !isOpen && !text.gameObject.activeInHierarchy)
        {
            if (CheckIfCanBeOpen())
            {
                GameStateManager.Instance.SetState(GameStateManager.GameState.Talking);
                initialText +=  " " + earnItem.name;
                GetComponent<Animator>().SetBool("isOpen", true);
                GameVariables.Instance.gameAudioSource.PlayOneShot(openChest);
                GameVariables.Instance.pauseGame = true;
                isOpen = true;
               
            }
        }
        else if (GameVariables.Instance.dialogueBox.activeInHierarchy && playerInRange)
        {
            timeLeft += Time.deltaTime;
            DisplayNpcText();
            text.text = displayText;
        }
        else if (isOpen && canBeOpen)
        {
            timeBeforeDisplayText -= Time.deltaTime;
            if (timeBeforeDisplayText <= 0)
            {
                GameVariables.Instance.dialogueBox.SetActive(true);
                GameVariables.Instance.gameAudioSource.PlayOneShot(getItemMusic);
                GameVariables.Instance.player.SetBoolAnimator("openChest", true);
                placeHolder.sprite = earnItem.GetComponent<SpriteRenderer>().sprite;
                earnItem.GiveItem();
                placeHolder.gameObject.transform.position = GameVariables.Instance.player.transform.position + new Vector3(0, 16, 0);
                placeHolder.gameObject.SetActive(true);
                canBeOpen = false;
            }
        }
    }
    private void DisplayNpcText()
    {
        lastLetterTime += Time.deltaTime;
        if (lastLetterTime >= delayToDisplayLetter)
        {
            nextCharacter++;
            lastLetterTime = 0;
            if (nextCharacter <= initialText.Length && displayText != initialText)
            {
                displayText = initialText.Substring(0, nextCharacter);
                GameVariables.Instance.gameAudioSource.PlayOneShot(letterDialogue);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && timeLeft >= inputDelay)
        {
            if (displayText == initialText)
            {
                    GameVariables.Instance.pauseGame = false;
                GameVariables.Instance.dialogueBox.SetActive(false);
                GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
                    dialogueIndex = 0;
                
                displayText = "";
                text.text = "";
                nextCharacter = 1;
                lastLetterTime = 0;
                timeLeft = 0;
                GameVariables.Instance.gameAudioSource.PlayOneShot(nextDialogue);
                GameVariables.Instance.player.SetBoolAnimator("openChest", false);
                placeHolder.gameObject.SetActive(false);
            }
            else
            {
                displayText = initialText;
                timeLeft = 0;

            }
        }
    }
    private bool CheckIfCanBeOpen()
    {
        Vector2 dir = (GameVariables.Instance.player.transform.position - transform.position).normalized;
        if (dir.x <= -0.8)
        {
            return GameVariables.Instance.player.LookRight();
        }
        else if (dir.x >= 0.8)
        {
            return GameVariables.Instance.player.LookLeft();
        }
        else if (dir.y >= 0.8)
        {
            return GameVariables.Instance.player.LookDown();
        }
        else if (dir.y <= -0.8)
        {
            return GameVariables.Instance.player.LookUp();
        }
        else
        {
            return false;
        }
    }
}
