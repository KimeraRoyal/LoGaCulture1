using UnityEngine;
using UnityEngine.Serialization;

[OrderInfo("Narrative",
             "Dialogue",
             "Writes text into a Dialogue Box - box can be customised.")]
[AddComponentMenu("")]
public class Dialogue : Order
{
    [Tooltip("Character that is speaking")]
    [SerializeField] protected Character character;
    [Tooltip("Portrait that represents speaking character")]
    [SerializeField] protected int characterPortraitIndex;
    [Tooltip("Character name to display in the box")]
    [SerializeField] protected string characterName;
    [Tooltip("Colour of the character name text ")]
    [SerializeField] protected Color32 characterNameColour;
    [TextArea(5, 10)]
    [SerializeField] protected string storyText = "";
    [FormerlySerializedAs("typingSpeed")]
    [Tooltip("Type each letter for this number of seconds")]
    [SerializeField] protected float letterDuration = 0.04f;
    [Tooltip("Type each punctuation mark for this number of seconds")]
    [SerializeField] protected float punctuationDuration = 0.04f;
    [Tooltip("Type this text in the previous Dialogue box")]
    [SerializeField] protected bool extendPrevious = false;
    [Tooltip("Voiceover audio to play when writing the text")]
    [SerializeField] protected AudioClip voiceOverClip;
    [Tooltip("Always show this text when the order is executed multiple times")]
    [SerializeField] protected bool showAlways = true;
    [Tooltip("Number of times to show this text when the order is executed multiple times")]
    [SerializeField] protected int showCount = 1;
    [Tooltip("Fade out the box when writing has finished and not waiting for input")]
    [SerializeField] protected bool fadeWhenDone = false;
    [Tooltip("Allow players to skip to end of text")]
    [SerializeField] protected bool allowLineSkip = true;
    [Tooltip("Players can progress on click rather than automatically showing")]
    [SerializeField] protected bool waitForClick = true;
    [Tooltip("How long the next text will wait before showing")]
    [SerializeField] protected float timeToWait = 1f;
    [Tooltip("Stop playing voiceover when text finishes writing")]
    [SerializeField] protected bool stopVoiceover = true;
    [Tooltip("Wait for the Voice Over to complete before continuing")]
    [SerializeField] protected bool waitForVO = false;
    [Tooltip("Sets the active dialogue box with a reference to a box object in the scene. All story text will now display using this box.")]
    [SerializeField] protected DialogueBox setDialogueBox;

    protected int executionCount;

    public virtual Character _Character { get { return character; } }
    public virtual int PortraitIndex { get => characterPortraitIndex; set => characterPortraitIndex = value; }
    public virtual Sprite Portrait => PortraitIndex >= 0 ? character.GetPortrait(PortraitIndex) : null;
    public virtual bool ExtendPrevious { get { return extendPrevious; } }

    public override void OnEnter()
    {
        if (!showAlways && executionCount >= showCount)
        {
            Continue();
            return;
        }

        executionCount++;

        if (setDialogueBox != null)
        {
            DialogueBox.ActiveDialogueBox = setDialogueBox;
        }

        var dialogueBox = DialogueBox.GetDialogueBox();
        if (dialogueBox == null)
        {
            Continue();
            return;
        }

        dialogueBox.SetStoryText(storyText);

        dialogueBox.SetActive(true);

        dialogueBox.SetCharacter(character);
        dialogueBox.SetCharacterImage(PortraitIndex);

        // dialogueBox.SetCharacterImage(characterPortrait);
        // dialogueBox.SetCharacterName(characterName, characterNameColour);

        string displayText = storyText;
        //using the text above, we can use active custom tags to change the text (e.g. <color=red>red text</color>) -- TO DO

        //lastly, we display the text in the box and when the action is complete, we continue
        dialogueBox.StartDialogue(letterDuration, punctuationDuration, extendPrevious, timeToWait, allowLineSkip, waitForClick, fadeWhenDone, Continue);
    }

    public override string GetSummary()
    {
        string namePrefix = "";
        if (character != null)
        {
            namePrefix = character.CharacterName + ": ";
        }
        if (extendPrevious)
        {
            namePrefix = "Extended: ";
        }
        return namePrefix + "\"" + storyText + "\"";
    }

    public override Color GetButtonColour()
    {
        return new Color32(224, 159, 22, 255);
    }

    public override void OnReset()
    {
        executionCount = 0;
    }

    public override void OnStopExecuting()
    {
        var dialogueBox = DialogueBox.GetDialogueBox();
        if (dialogueBox == null)
        {
            return;
        }

        dialogueBox.Stop();
    }

    #region ILocalizable implementation

    public virtual string GetStandardText()
    {
        return storyText;
    }

    public virtual void SetStandardText(string standardText)
    {
        storyText = standardText;
    }

    public virtual string GetStringId()
    {
        // String id for Say commands is SAY.<Localization Id>.<Command id>.[Character Name]
        string stringId = "SAY." + GetEngineLocalizationId() + "." + itemId + ".";
        if (character != null)
        {
            stringId += character.CharacterName;
        }

        return stringId;
    }

    #endregion
}