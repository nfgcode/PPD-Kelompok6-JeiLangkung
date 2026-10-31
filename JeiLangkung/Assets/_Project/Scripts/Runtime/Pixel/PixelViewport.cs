using UnityEngine;

namespace JeiLangkung.Pixel
{
    public sealed class PixelViewport : MonoBehaviour
    {
        private RectTransform content;
        private RectTransform viewport;
        public void Initialize(RectTransform value)
        {
            content = value;
            viewport = (RectTransform)transform;
            Resize();
        }
        private void OnRectTransformDimensionsChange() { Resize(); }
        private void Resize()
        {
            if (content == null || viewport == null) return;
            float scale = Mathf.Min(viewport.rect.width / 1920, viewport.rect.height / 1080);
            content.localScale = new Vector3(scale, scale, 1);
        }
    }
}
