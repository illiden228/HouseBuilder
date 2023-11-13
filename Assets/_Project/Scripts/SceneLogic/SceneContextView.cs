using Core;
using UnityEngine;

namespace SceneLogic
{
    public abstract class SceneContextView : BaseMonobehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Transform uiParent;

        public Camera MainCamera => mainCamera;

        public Transform UiParent => uiParent;

        public Canvas MainCanvas => mainCanvas;
    }
}