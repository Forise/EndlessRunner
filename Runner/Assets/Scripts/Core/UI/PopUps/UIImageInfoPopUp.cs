using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Core.EventSystem;

namespace Core
{
    public class UIImageInfoPopUp : UIPopUp
    {
        public TextMeshProUGUI title;
        public Image image;
        public TextMeshProUGUI countText;
        [SerializeField]
        private MyButton okButton;
        private System.Action okAction;

        protected override void Start()
        {
            okButton.onClick.AddListener(OkButtonClick);
            base.Start();
        }

        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(OkButtonClick);
        }

        protected override void ActiveStateUpdateHandler()
        {
            base.ActiveStateUpdateHandler();
            CheckMissClick(okAction);
        }

        #region Methods

        protected override void SetContent()
        {
            SetActions();
            image.sprite = popupData.sprite;
            base.SetContent();
        }

        private void SetActions()
        {
            okAction = popupData.confirmAction;
        }

        protected override void SetTexts()
        {
            base.SetTexts();
            countText.text = popupData.intParam.ToString();
        }
        #endregion

        #region Handlers
        protected override void Handler_BackButton(object sender, GameEventArgs e)
        {
            okAction?.Invoke();
            base.Handler_BackButton(sender, e);
        }
        private void OkButtonClick()
        {
            okAction.Invoke();
            CloseThisWindow();
        }
        #endregion
    }
}