//Developed by Pavel Kravtsov.
using System.Collections;
using UnityEngine;
using Core.EventSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Core
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class UIHUD<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        protected enum AnimType
        {
            Alpha,
            OnOff
        }
        protected enum BlockType
        {
            Raycaster,
            Object
        }
        #region Fields
        #region Showed in inspector
        [SerializeField, Header("UI_HUD")]
        protected GameObject[] hudObjects;
        [SerializeField]
        protected CanvasGroup topContent;
        [SerializeField]
        protected CanvasGroup botContent;
        [SerializeField]
        protected BlockType blockType;
        [SerializeField, Header("Animations")]
        protected AnimType animType;
        [SerializeField]
        protected AnimationCurve closeAnimationCurve;
        [SerializeField]
        protected AnimationCurve openAnimationCurve;
        #endregion Showed in inspector
        protected Canvas canvas;
        protected GraphicRaycaster graphicRaycaster;
        #endregion Fields

        #region Unity Methods
        protected virtual void Awake()
        {
            if (!canvas)
                canvas = GetComponent<Canvas>();
            if (!graphicRaycaster)
                graphicRaycaster = GetComponent<GraphicRaycaster>();
            SubscribeEvents();
        }

        protected override void OnDestroy()
        {
            UnsubscribeEvents();
            base.OnDestroy();
        }
        #endregion Unity Methods

        #region Methods
        protected virtual void SubscribeEvents()
        {
            EventManager.Subscribe(Events.InputEvents.BLOCK_HUD, BlockHUD);
            EventManager.Subscribe(Events.InputEvents.UNBLOCK_HUD, UnblockHUD);
        }
        protected virtual void UnsubscribeEvents()
        {
            EventManager.Unsubscribe(Events.InputEvents.BLOCK_HUD, BlockHUD);
            EventManager.Unsubscribe(Events.InputEvents.UNBLOCK_HUD, UnblockHUD);
        }

        protected virtual GameObject[] GetChildren()
        {
            GameObject[] gameObjects = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObjects[i] = transform.GetChild(i).gameObject;
            }
            return gameObjects;
        }        

        protected virtual void HideHUD(object sender, GameEventArgs args)
        {
            switch(animType)
            {
                case AnimType.OnOff:
                    canvas.enabled = false;
                    graphicRaycaster.enabled = false;
                    break;
                case AnimType.Alpha:
                    StartCoroutine(AlphaAnimationCoroutine(closeAnimationCurve, topContent));
                    StartCoroutine(AlphaAnimationCoroutine(closeAnimationCurve, botContent, () => { canvas.enabled = false; graphicRaycaster.enabled = false; }));
                    break;                    
            }
        }

        protected virtual void ShowHUD(object sender, GameEventArgs args)
        {
            switch (animType)
            {
                case AnimType.OnOff:
                    canvas.enabled = true;
                    graphicRaycaster.enabled = true;
                    break;
                case AnimType.Alpha:
                    canvas.enabled = true;
                    StartCoroutine(AlphaAnimationCoroutine(openAnimationCurve, topContent));
                    StartCoroutine(AlphaAnimationCoroutine(openAnimationCurve, botContent,() => { graphicRaycaster.enabled = true; }));
                    break;
            }
        }
        #region Animation coroutines
        protected virtual IEnumerator AlphaAnimationCoroutine(AnimationCurve curve, Image img, System.Action action = null)
        {
            img.gameObject.SetActive(true);
            float endValue = Mathf.Clamp01(curve.keys[curve.keys.Length - 1].value);
            if (img)
            {
                float time = 0f;
                while (img.color.a != endValue)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, curve.Evaluate(time));
                    time += Time.deltaTime;
                    yield return null;
                }
                img.color = new Color(img.color.r, img.color.g, img.color.b, endValue);
            }
            img.gameObject.SetActive(img.color.a > 0);
            action?.Invoke();
        }
        protected virtual IEnumerator AlphaAnimationCoroutine(AnimationCurve curve, GameObject obj, System.Action action = null)
        {
            obj.SetActive(true);
            float endValue = Mathf.Clamp01(curve.keys[curve.keys.Length - 1].value);
            Image image = obj.GetComponent<Image>();
            if (image)
            {
                float time = 0f;
                while (image.color.a != endValue)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, curve.Evaluate(time));
                    time += Time.deltaTime;
                    yield return null;
                }
                image.color = new Color(image.color.r, image.color.g, image.color.b, endValue);
            }
            obj.SetActive(image.color.a > 0);
            action?.Invoke();
        }
        protected virtual IEnumerator AlphaAnimationCoroutine(AnimationCurve curve, CanvasGroup canvasGroup, System.Action action = null)
        {
            canvasGroup.gameObject.SetActive(true);
            float endValue = Mathf.Clamp01(curve.keys[curve.keys.Length - 1].value);
            float time = 0f;
            while (canvasGroup.alpha != endValue)
            {
                canvasGroup.alpha = curve.Evaluate(time);
                time += Time.deltaTime;
                yield return null;
            }
            canvasGroup.gameObject.SetActive(canvasGroup.alpha > 0);
            action?.Invoke();
        }
        protected virtual IEnumerator AlphaAnimationCoroutine(AnimationCurve curve, TMPro.TextMeshProUGUI text, System.Action action = null)
        {
            text.gameObject.SetActive(true);
            float endValue = Mathf.Clamp01(curve.keys[curve.keys.Length - 1].value);
            float time = 0f;
            while (text.color.a != endValue)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, curve.Evaluate(time));
                time += Time.deltaTime;
                yield return null;
            }
            text.gameObject.SetActive(text.color.a > 0);
            action?.Invoke();
        }
        #endregion Animation coroutines
        #endregion Methods

        #region Handlers
        protected virtual void BlockHUD(object sender, GameEventArgs args)
        {
            switch (blockType)
            {
                case BlockType.Object:
                    foreach (var o in hudObjects)
                    {
                        var graphic = o.gameObject.GetComponent<Graphic>();
                        if (graphic)
                            graphic.raycastTarget = false;

                        var selectable = o.gameObject.GetComponent<Selectable>();
                        if (selectable)
                            selectable.interactable = false;

                        var button = o.gameObject.GetComponent<Button>();
                        if (button)
                            button.interactable = false;

                        var eventTrigger = o.gameObject.GetComponent<EventTrigger>();
                        if (eventTrigger)
                            eventTrigger.enabled = false;
                    }
                    break;
                case BlockType.Raycaster:
                    graphicRaycaster.enabled = false;
                    break;
            }
        }

        protected virtual void UnblockHUD(object sender, GameEventArgs args)
        {
            switch (blockType)
            {
                case BlockType.Object:
                    foreach (var o in hudObjects)
                    {
                        var graphic = o.gameObject.GetComponent<Graphic>();
                        if (graphic)
                            graphic.raycastTarget = true;

                        var selectable = o.gameObject.GetComponent<Selectable>();
                        if (selectable)
                            selectable.interactable = true;

                        var button = o.gameObject.GetComponent<Button>();
                        if (button)
                            button.interactable = true;

                        var eventTrigger = o.gameObject.GetComponent<EventTrigger>();
                        if (eventTrigger)
                            eventTrigger.enabled = true;
                    }
                    break;
                case BlockType.Raycaster:
                    graphicRaycaster.enabled = true;
                    break;
            }
        }
        #endregion Handlers
    }
}