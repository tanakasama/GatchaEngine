using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BMSGatchaEngine.Runtime.UI.Tweens
{
    public class TweenRotator : MonoBehaviour
    {
        /// <summary>
        /// The rotation we are going to be using
        /// </summary>
        public Vector3 Rotation;
        /// <summary>
        /// The speed at which we rotate
        /// </summary>
        public float Speed;

        /// <summary>
        /// Self Transform
        /// </summary>
        private RectTransform _selfTransform;

        private void Start()
        {
            _selfTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _selfTransform.Rotate(Rotation * Speed * Time.deltaTime);
        }
    }
}