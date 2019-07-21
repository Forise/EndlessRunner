﻿using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class PickableItemModel : GameObjectModel
    {
        [SerializeField]
        private PickableItemType type;

        public PickableItemType Type
        {
            get => type;
            set
            {
                type = value;
                TypeChanged?.Invoke();
            }
        }

        public event ModelChangedDelegate TypeChanged;
    }

    public enum PickableItemType
    {
        Cherry,
        Gem
    }
}