using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;

namespace Core
{
    public class UIProcessPopUp : UIPopUp
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private bool blockInputWhileOpened;
        [SerializeField]
        private float speed = 1f;
        private string[] closeEvents;

        protected override void ActiveStateUpdateHandler()
        {
            base.ActiveStateUpdateHandler();
            RotateImage();
        }

        protected override void OpenAnimationStateInitHandler()
        {
            base.OpenAnimationStateInitHandler();
            if (blockInputWhileOpened)
            {
                EventManager.Notify(this, new GameEventArgs(Events.InputEvents.BLOCK_INPUT));
                EventManager.Notify(this, new GameEventArgs(Events.InputEvents.BLOCK_HUD));
            }
        }

        protected override void CloseAnimationStateCloseHandler()
        {
            base.CloseAnimationStateCloseHandler();
            if (blockInputWhileOpened)
            {
                EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UNBLOCK_INPUT));
                EventManager.Notify(this, new GameEventArgs(Events.InputEvents.UNBLOCK_HUD));
            }
        }

        #region Methods
        private void SubscribeAllCloseEvents()
        {
            foreach(var e in closeEvents)
            {
                EventManager_Window.Subscribe(e, Close_Handler);
                EventManager.Subscribe(e, Close_Handler);
            }
        }
        private void UnsubscribeAllCloseEvents()
        {
            foreach (var e in closeEvents)
            {
                EventManager_Window.Unsubscribe(e, Close_Handler);
                EventManager.Unsubscribe(e, Close_Handler);
            }
        }

        private void RotateImage()
        {
            image.transform.Rotate(Vector3.back * Time.deltaTime * speed);
        }

        protected override void SetContent()
        {
            closeEvents = popupData.closeEvents;
            SubscribeAllCloseEvents();
            base.SetContent();
        }
        #endregion

        #region Handlers
        public void Close_Handler(object sender, GameEventArgs e)
        {
            UnsubscribeAllCloseEvents();
            CloseThisWindow();        
        }
        #endregion
    }
}