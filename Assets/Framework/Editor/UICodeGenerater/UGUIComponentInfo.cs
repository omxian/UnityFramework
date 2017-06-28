namespace UI.Code.Generater
{
    public class UGUIComponentInfo : ComponentInfo
    {
        public override string GetButton()
        {
            return "Button";
        }

        public override string GetInputField()
        {
            return "InputField";
        }

        public override string GetLable()
        {
            return "Text";
        }

        public override string GetSlider()
        {
            return "Slider";
        }

        public override string GetSprite()
        {
            return "Image";
        }

        public override string GetTexture()
        {
            return "RawImage";
        }

        public override string GetToggle()
        {
            return "Toggle";
        }
    }
}