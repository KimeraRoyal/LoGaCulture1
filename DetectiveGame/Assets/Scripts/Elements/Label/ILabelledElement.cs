namespace KR.Elements.Label
{
    public interface ILabelledElement
    {
        public string GetLabel();
        public void SetLabel(string _label);

        public float GetFontSize();
        public void SetFontSize(float _fontSize);
    }
}
