using System;
using System.Collections;
using System.Collections.Generic;
using DialogueExtension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Display story text in a visual novel style Dialogue box.
/// </summary>
/// 
public class DialogueBox : MonoBehaviour
{
    private Shake m_portraitShake;
    
    [Tooltip("The continue button UI object")]
    [SerializeField] protected Button continueButton;
    [Tooltip("The text UI object")]
    [SerializeField] protected TextMeshProUGUI textDisplay;
    [Tooltip("The name text UI object")]
    [SerializeField] protected TextMeshProUGUI nameText;
    [Tooltip("Duration to fade dialogue in/out")]
    [SerializeField] protected float fadeDuration = 0.25f;
    [SerializeField] protected string storyText = "";
    [Tooltip("Close any other open dialogue boxes when this one is active")]
    [SerializeField] protected bool closeOtherDialogues;
    [Tooltip("The character UI object")]
    [SerializeField] protected Image characterImage;
    [Tooltip("The character's talking speed")]
    [SerializeField] protected float talkSpeed = 1.0f;
    [Tooltip("Adjust width of story text when Character Image is displayed (to avoid overlapping)")]
    [SerializeField] protected bool fitTextWithImage = true;
    public virtual Image CharacterImage { get { return characterImage; } }
    protected TextWriter writer;
    protected DialogueParser m_parser;
    protected CanvasGroup canvasGroup;
    protected bool fadeWhenDone = true;
    protected float targetAlpha = 0f;
    protected float fadeCoolDownTimer = 0f;
    // Cache active boxes to avoid expensive scene search
    protected static List<DialogueBox> activeDialogueBoxes = new List<DialogueBox>();
    protected Sprite currentCharacterImage;
    protected float startStoryTextWidth;
    protected float startStoryTextInset;
    protected static Character speakingCharacter;

    public Shake PortraitShake
    {
        get
        {
            if(!m_portraitShake) { m_portraitShake = characterImage.GetComponent<Shake>(); }
            return m_portraitShake;
        }
    }

    protected DialogueParser Parser
    {
        get
        {
            if (!m_parser) { m_parser = GetComponent<DialogueParser>(); }
            return m_parser;
        }
    }

    public virtual RectTransform StoryTextRectTrans
    {
        get
        {
            return storyText != null ? textDisplay.rectTransform : textDisplay.GetComponent<RectTransform>();
        }
    }

    public bool IsWaitingForInput { get; set; }

    protected virtual void Awake()
    {
        if (!activeDialogueBoxes.Contains(this))
        {
            activeDialogueBoxes.Add(this);
        }
    }

    protected virtual void OnDestroy()
    {
        activeDialogueBoxes.Remove(this);
    }

    public virtual TextWriter GetWriter()
    {
        if (writer != null)
        {
            return writer;
        }

        writer = GetComponent<TextWriter>();
        if (writer == null)
        {
            writer = gameObject.AddComponent<TextWriter>();
        }

        return writer;
    }

    public virtual TMP_Text GetTextDisplay()
    {
        if (textDisplay != null)
        {
            return textDisplay;
        }

        textDisplay = GetComponentInChildren<TextMeshProUGUI>();
        if (textDisplay == null)
        {
            textDisplay = gameObject.AddComponent<TextMeshProUGUI>(); //may need to be added onto a canvas group for correct displaying
        }

        return textDisplay;
    }

    //this will conflict with the above method, so we'll comment it out for now
    // protected virtual TextMeshProUGUI GetTextNameDisplay()
    // {
    //     if (nameText != null)
    //     {
    //         return nameText;
    //     }

    //     nameText = GetComponentInChildren<TextMeshProUGUI>();
    //     if (nameText == null)
    //     {
    //         nameText = gameObject.AddComponent<TextMeshProUGUI>(); //may need to be added onto a canvas group for correct displaying
    //     }

    //     return textDisplay;
    // }

    protected virtual CanvasGroup GetCanvasGroup()
    {
        if (canvasGroup) { return canvasGroup; }

        canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    protected virtual void Start()
    {
        // Dialogue always starts invisible, will be faded in when writing starts
        GetCanvasGroup().alpha = 0f;

        // Add a raycaster if none already exists so we can handle input
        GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }

        if (currentCharacterImage == null)
        {
            // Character image is hidden by default.
            SetCharacterImage(null);
        }
    }

    protected virtual void LateUpdate()
    {
        UpdateAlpha();

        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(IsWaitingForInput);
        }
    }

    protected virtual void UpdateAlpha()
    {
        if (GetWriter().IsTyping())
        {
            targetAlpha = 1f;
            fadeCoolDownTimer = 0.1f;

            GetCanvasGroup().blocksRaycasts = true;
        }
        else
        {
            if (fadeWhenDone && Mathf.Approximately(fadeCoolDownTimer, 0f))
            {
                targetAlpha = 0f;
                GetCanvasGroup().blocksRaycasts = false;
            }
            else
            {
                // Add a short delay before we start fading in case there's another text order in the next frame or two
                // This avoids a noticeable flicker between consecutive text orders
                fadeCoolDownTimer = Mathf.Max(0f, fadeCoolDownTimer - Time.deltaTime);
            }
        }

        CanvasGroup canvasGroup = GetCanvasGroup();
        if (fadeDuration <= 0f)
        {
            canvasGroup.alpha = targetAlpha;
        }
        else
        {
            float delta = (1f / fadeDuration) * Time.deltaTime;
            float alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, delta);
            canvasGroup.alpha = alpha;
        }
    }

    protected virtual void ClearStoryText()
    {
        storyText = "";
    }

    public virtual void SetStoryText(string text)
    {
        storyText = text;
    }

    /// Currently active Dialogue used to display Say text
    public static DialogueBox ActiveDialogueBox { get; set; }

    public static DialogueBox GetDialogueBox()
    {
        if (ActiveDialogueBox == null)
        {
            DialogueBox dialogueBox = null;
            if (activeDialogueBoxes.Count > 0)
            {
                dialogueBox = activeDialogueBoxes[0];
            }
            if (dialogueBox != null)
            {
                ActiveDialogueBox = dialogueBox;
            }
            if (ActiveDialogueBox == null)
            {
                // Create a new dialogue box
                GameObject prefab = Resources.Load<GameObject>("Prefabs/DialogueBox");
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab) as GameObject;
                    go.SetActive(false);
                    go.name = "DialogueBox";
                    ActiveDialogueBox = go.GetComponent<DialogueBox>(); ;
                }
            }
        }
        return ActiveDialogueBox;
    }

    public virtual void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public virtual void SetCharacter(Character character)
    {
        if (character == null)
        {
            if (characterImage != null)
            {
                characterImage.gameObject.SetActive(false);
            }
            if (nameText.text != null)
            {
                nameText.text = "";
            }
            speakingCharacter = null;

            talkSpeed = 1.0f;
        }
        else
        {
            string characterName = character.CharacterName;

            if (characterName == "")
            {
                // Use game object name as default
                characterName = character.GetObjectName();
            }

            SetCharacterName(characterName, character.NameColour);
            speakingCharacter = character;

            talkSpeed = character.TalkSpeed;
        }
    }

    public virtual void SetCharacterImage(int imageIndex)
    {
        var portrait = speakingCharacter ? speakingCharacter.GetPortrait(imageIndex) : null;
        SetCharacterImage(portrait);
    }

    public virtual void SetCharacterImage(Sprite sprite)
    {
        if (characterImage == null)
            return;

        if (sprite != null)
        {
            characterImage.overrideSprite = sprite;
            characterImage.gameObject.SetActive(true);
            currentCharacterImage = sprite;
        }
        else
        {
            characterImage.gameObject.SetActive(false);

            if (startStoryTextWidth != 0)
            {
                StoryTextRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, startStoryTextInset, startStoryTextWidth);
            }
        }

        if (fitTextWithImage && storyText != null && characterImage.gameObject.activeSelf)
        {
            if (Mathf.Approximately(startStoryTextWidth, 0f))
            {
                startStoryTextWidth = StoryTextRectTrans.rect.width;
                startStoryTextInset = StoryTextRectTrans.offsetMin.x;
            }

            if (StoryTextRectTrans.position.x < characterImage.rectTransform.position.x)
            {
                StoryTextRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                startStoryTextInset,
                startStoryTextWidth - characterImage.rectTransform.rect.width);
            }
            else
            {
                StoryTextRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                startStoryTextInset,
                startStoryTextWidth - characterImage.rectTransform.rect.width);
            }
        }
    }

    public virtual void SetCharacterName(string name, Color color)
    {
        if (color == null)
            color = Color.black;
        if (nameText != null)
        {
            nameText.text = name;
            nameText.color = color;
        }
    }

    public virtual void StartDialogue(float letterDuration, float punctuationDuration, bool extend, float waitTime, bool skipLine, bool waitForClick, bool fadeWhenDone, Action onComplete)
    {
        StartCoroutine(DoDialogue(onComplete, letterDuration, punctuationDuration, extend, waitTime, skipLine, waitForClick, fadeWhenDone));
    }

    public virtual IEnumerator DoDialogue(Action onComplete, float letterDuration, float punctuationDuration, bool extend, float waitTime, bool skipLine, bool waitForClick, bool fadeWhenDone)
    {
        var tw = GetWriter();

        if (writer.IsTyping())
        {
            tw.Stop();
            yield return new WaitUntil(() => !writer.IsTyping());
        }

        if (closeOtherDialogues)
        {
            for (int i = 0; i < activeDialogueBoxes.Count; i++)
            {
                var db = activeDialogueBoxes[i];
                if (db.gameObject != gameObject)
                {
                    db.SetActive(false);
                }
            }
        }
        gameObject.SetActive(true);

        this.fadeWhenDone = fadeWhenDone;

        talkSpeed = Mathf.Max(talkSpeed, 0.001f);
        var state = new DialogueState(this, letterDuration / talkSpeed, punctuationDuration / talkSpeed, skipLine);
        
        Parser.Parse(storyText, extend, waitForClick, waitTime);
        while (Parser.HasCommands && state.Enabled)
        {
            var command = Parser.GetNextCommand();
            yield return command.Execute(state);
        }
        Parser.Flush();
        
        onComplete?.Invoke();
    }

    public virtual bool FadeWhenDone { get { return fadeWhenDone; } set { fadeWhenDone = value; } }

    /// Stop the dialogue while its writing text
    public virtual void Stop()
    {
        fadeWhenDone = true;
        GetWriter().Stop();
    }

    /// Stops writing text and clears text
    public virtual void Clear()
    {
        ClearStoryText();

        // Kill any active write coroutine
        StopAllCoroutines();
    }
}
