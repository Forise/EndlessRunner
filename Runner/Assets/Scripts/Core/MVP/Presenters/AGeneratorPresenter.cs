﻿using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class AGeneratorPresenter<T> : MonoBehaviour
    {
        [SerializeField]
        public GeneratorModel<T> generatorModel;

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            generatorModel = generatorModel ?? new GeneratorModel<T>();
        }

        protected virtual void StartForceDisable()
        {
            StartCoroutine(ForceDisableObject());
        }

        protected abstract void CreateObject();
        protected abstract void DisableObject();
        protected abstract IEnumerator ForceDisableObject();
    }
}