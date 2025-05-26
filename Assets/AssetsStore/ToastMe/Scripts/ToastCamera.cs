using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToastMe
{
    [RequireComponent(typeof(Camera))]
    public class ToastCamera : MonoBehaviour
    {
        /// <summary>
        /// Name of the LayerMask used to render toasts to the Ui.
        /// </summary>
        private string _toastLayerMask = "Toast";
        
        /// <summary>
        /// Unity Camera that is a child of the main camera with the sole purpose of rendering toasts to the Ui.
        /// </summary>
        private Camera _toastCamera;

        /// <summary>
        /// Unity camera tagged as the Main Camera within a Unity Scene.
        /// </summary>
        private Camera _mainCamera;

        private void Awake()
        {
            // Resolve our references.
            _toastCamera = GetComponent<Camera>();
            _mainCamera = Camera.main;
                     
        }
    }
}