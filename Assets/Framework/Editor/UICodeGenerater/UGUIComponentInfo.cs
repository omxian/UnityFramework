namespace Unity.Framework.Editor
{
    public class UGUIComponentInfo : BaseComponentInfo
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