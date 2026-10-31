using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JeiLangkung.Pixel
{
    /// <summary>Tombol menu bergaya layar retro: kursor "&gt;" dan latar lebih terang menandai fokus keyboard atau mouse.</summary>
    public sealed class PixelButton : Button
    {
        private TMP_Text label;
        private Image background;
        private string caption;

        public void Setup(TMP_Text text, Image image, string value)
        {
            label = text;
            background = image;
            transition = Transition.None;
            SetCaption(value);
        }

        public void SetCaption(string value)
        {
            caption = value;
            DoStateTransition(currentSelectionState, true);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (IsInteractable()) Select();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (label == null || background == null) return;
            bool focused = state == SelectionState.Highlighted || state == SelectionState.Selected || state == SelectionState.Pressed;
            label.text = (focused ? "> " : "  ") + caption;
            label.color = state == SelectionState.Disabled ? PixelPalette.Dark : state == SelectionState.Pressed ? PixelPalette.Black : PixelPalette.White;
            background.color = state == SelectionState.Pressed ? PixelPalette.Light : focused ? PixelPalette.Dark : PixelPalette.Black;
        }
    }
}
