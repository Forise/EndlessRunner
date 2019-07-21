//Developed by Pavel Kravtsov.
using Core.EventSystem;
using UnityEngine;

namespace Core
{
    public class UIDialoguePopUp : UIPopUp
    {
        [SerializeField]
        private MyButton okButton;
        [SerializeField]
        private MyButton cancelButton;

        private System.Action okAction;
        private System.Action cancelAction;

        protected override void Start()
        {
            okButton.onClick.AddListener(OkButtonClick);
            cancelButton.onClick.AddListener(CancelButtonClick);
            base.Start();
        }

        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(OkButtonClick);
            cancelButton.onClick.RemoveListener(CancelButtonClick);
        }

        protected override void ActiveStateUpdateHandler()
        {
            base.ActiveStateUpdateHandler();
            CheckMissClick(cancelAction);
        }

        protected override void InactiveStateInitHandler()
        {
            base.InactiveStateInitHandler();
        }

        #region Methods

        protected override void SetContent()
        {
            SetActions();
            base.SetContent();
        }

        private void SetActions()
        {
            okAction = popupData.confirmAction;
            cancelAction = popupData.declineAction;
        }
        #endregion

        #region Handlers
        protected override void Handler_BackButton(object sender, GameEventArgs e)
        {
            cancelAction?.Invoke();
            base.Handler_BackButton(sender, e);
        }

        private void OkButtonClick()
        {
            okAction?.Invoke();
            CloseThisWindow();
        }
        private void CancelButtonClick()
        {
            cancelAction?.Invoke();
            CloseThisWindow();
        }
        #endregion
    }
}