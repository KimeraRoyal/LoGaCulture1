using TMPro;

namespace DialogueExtension
{
    public class DialogueState
    {
        private DialogueBox m_dialogueBox;
        public TextWriter Writer { get; }
        public TMP_Text Display { get; }
        public bool Enabled { get; set; }

        public float LetterDuration { get; set; }
        public float PunctuationDuration { get; set; }

        public bool AllowLineSkip { get; set; }
        
        public bool IsWaitingForInput
        {
            get => m_dialogueBox.IsWaitingForInput;
            set => m_dialogueBox.IsWaitingForInput = value;
        }

        public DialogueState(DialogueBox _dialogueBox, float _letterDuration, float _punctuationDuration, bool _allowLineSkip)
        {
            m_dialogueBox = _dialogueBox;
            Writer = m_dialogueBox.GetWriter();
            Display = m_dialogueBox.GetTextDisplay();
            Enabled = true;

            LetterDuration = _letterDuration;
            PunctuationDuration = _punctuationDuration;

            AllowLineSkip = _allowLineSkip;
        }
    }
}