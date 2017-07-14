namespace Unity.Framework.Editor
{
    /// <summary>
    /// Define Component Name
    /// </summary>
    public abstract class BaseComponentInfo
    {
        public abstract string GetTexture();
        public abstract string GetLable();
        public abstract string GetSprite();
        public abstract string GetButton();
        public abstract string GetToggle();
        public abstract string GetInputField();
        public abstract string GetSlider();
        public string GetGameObject()
        {
            return "GameObject";
        }

        public string GetTransform()
        {
            return "Transform";
        }
    }
}