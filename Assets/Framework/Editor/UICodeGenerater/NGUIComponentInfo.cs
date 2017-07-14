using System;

namespace Unity.Framework.Editor
{
    public class NGUIComponentInfo : BaseComponentInfo
    {
        public override string GetButton()
        {
            return "UIButton";
        }

        public override string GetInputField()
        {
            return "UIInput";
        }

        public override string GetLable()
        {
            return "UILabel";
        }

        public override string GetSlider()
        {
            throw new NotImplementedException();
        }

        public override string GetSprite()
        {
            return "UISprite";
        }

        public override string GetTexture()
        {
            return "UITexture";
        }

        public override string GetToggle()
        {
            return "UIToggle";
        }
    }
}