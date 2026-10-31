using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace JeiLangkung.Pixel
{
    /// <summary>
    /// Panggung bersama menu, Scene 1, dan Scene 2: kanvas 1920 x 1080 berletterbox, gambar piksel di belakang,
    /// teks TextMeshPro dan tombol di atasnya.
    /// </summary>
    internal sealed class PixelUi
    {
        /// <summary>Satu piksel gambar 480 x 270 sama dengan empat satuan UI.</summary>
        public const float Unit = 4;
        private readonly TMP_FontAsset font;
        private readonly List<RaycastResult> hits = new List<RaycastResult>();
        public RectTransform Root { get; private set; }

        public PixelUi(TMP_FontAsset font) { this.font = font; }

        public void BuildStage(Transform owner, Texture picture, string pictureName)
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(owner, false);
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = .5f;
            Box(canvasObject.transform, -3000, -3000, 9000, 9000, PixelPalette.Black);
            RectTransform viewport = Group("Panggung", canvasObject.transform);
            viewport.anchorMin = viewport.anchorMax = viewport.pivot = Vector2.one * .5f;
            viewport.anchoredPosition = Vector2.zero;
            var fitter = viewport.gameObject.AddComponent<AspectRatioFitter>();
            fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fitter.aspectRatio = 16f / 9f;
            // Koordinat tetap 1920 x 1080 pada rasio layar apa pun; sisanya menjadi letterbox.
            Root = Group("Komposisi", viewport);
            viewport.gameObject.AddComponent<PixelViewport>().Initialize(Root);

            var image = Group(pictureName, Root).gameObject.AddComponent<RawImage>();
            image.texture = picture;
            // Piksel digambar dari atas ke bawah, sedangkan tekstur Unity dimulai dari bawah.
            image.uvRect = new Rect(0, 1, 1, -1);
            image.raycastTarget = false;

            if (EventSystem.current == null)
            {
                var eventObject = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
                eventObject.transform.SetParent(owner, false);
                eventObject.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
            }
        }

        public static RectTransform Group(string name, Transform parent, float x = 0, float y = 0, float w = 1920, float h = 1080)
        {
            var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.zero;
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(w, h);
            return rect;
        }

        public static Image Box(Transform parent, float x, float y, float w, float h, Color color)
        {
            var image = Group("Bidang", parent, x, y, w, h).gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        public TMP_Text Text(Transform parent, string value, float x, float y, float w, float h, float size, Color color,
            TextAlignmentOptions align = TextAlignmentOptions.MidlineLeft)
        {
            var text = Group("Teks", parent, x, y, w, h).gameObject.AddComponent<TextMeshProUGUI>();
            text.font = font;
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.alignment = align;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.raycastTarget = false;
            return text;
        }

        public PixelButton Button(Transform parent, string id, string caption, float x, float y, float w, float h, Action action, float size = 38)
        {
            Image image = Box(parent, x, y, w, h, PixelPalette.Black);
            image.name = id;
            image.raycastTarget = true;
            TMP_Text label = Text(image.transform, "", 16, 0, w - 32, h, size, PixelPalette.White);
            PixelButton button = image.gameObject.AddComponent<PixelButton>();
            button.targetGraphic = image;
            button.Setup(label, image, caption);
            button.onClick.AddListener(() => action());
            return button;
        }

        /// <summary>Slider datar: rel gelap, pegangan persegi yang terang saat fokus.</summary>
        public Slider Slider(Transform parent, string id, float x, float y, float w, float h, float value, Action<float> changed)
        {
            RectTransform rect = Group(id, parent, x, y, w, h);
            var hitArea = rect.gameObject.AddComponent<Image>();
            hitArea.color = Color.clear;
            Box(rect, 0, h / 2 - 4, w, 8, PixelPalette.Dark);
            Image handle = Box(rect, 0, 0, 24, h, Color.white);
            handle.raycastTarget = true;
            var slider = rect.gameObject.AddComponent<Slider>();
            slider.handleRect = handle.rectTransform;
            slider.targetGraphic = handle;
            ColorBlock colors = slider.colors;
            colors.normalColor = new Color(.65f, .65f, .65f);
            colors.highlightedColor = colors.selectedColor = Color.white;
            colors.pressedColor = new Color(.8f, .8f, .8f);
            slider.colors = colors;
            slider.value = value;
            slider.onValueChanged.AddListener(v => changed(v));
            return slider;
        }

        /// <summary>Posisi pointer layar dalam koordinat gambar 480 x 270.</summary>
        public bool ToPicture(Vector2 screen, out Vector2 point)
        {
            point = default;
            if (Root == null || !RectTransformUtility.ScreenPointToLocalPointInRectangle(Root, screen, null, out Vector2 local)) return false;
            point = new Vector2(local.x / Unit, 270 - local.y / Unit);
            return point.x >= 0 && point.x < 480 && point.y >= 0 && point.y < 270;
        }

        /// <summary>Klik pada tombol tidak dihitung sebagai klik ke gambar.</summary>
        public bool PointerOverButton(Vector2 screen)
        {
            if (EventSystem.current == null) return false;
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = screen }, hits);
            foreach (RaycastResult hit in hits) if (hit.gameObject.GetComponentInParent<Selectable>() != null) return true;
            return false;
        }
    }
}
