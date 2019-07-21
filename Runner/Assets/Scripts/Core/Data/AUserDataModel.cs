using UnityEngine;

namespace Core
{
    public abstract class AUserDataModel : Model
    {
        //This time stamp is used for data validation
        [SerializeField]
        private string stringDateTime;

        public string StringDateTime
        {
            get => stringDateTime;
            set
            {
                stringDateTime = value;
                DateTimeChanged();
            }
        }

        public event ModelChangedDelegate OnDateTimeChanged;
        public event ModelChangedDelegate UserDataChanged;

        protected void DateTimeChanged()
        {
            OnDateTimeChanged?.Invoke();
        }

        protected void NotifyUserDataChanged()
        {
            UserDataChanged?.Invoke();
        }
    }
}