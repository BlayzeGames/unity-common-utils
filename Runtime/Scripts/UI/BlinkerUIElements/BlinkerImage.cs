﻿using System.Collections.Generic;
using System.Linq;
using CommonUtils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace CommonUtils.UI.BlinkerUIElements {
	[AddComponentMenu("UI/Blinker Image")]
    [RequireComponent(typeof(Image))]
    public class BlinkerImage : MonoBehaviour, IBlinkerUIElement {
		#pragma warning disable 649
        [SerializeField] private float blinkingRate;
        [SerializeField] private bool autoStart;
		#pragma warning restore 649

        public float BlinkingRate { get => blinkingRate; set => blinkingRate = value; }

		private IEnumerable<Graphic> graphics;

        public IEnumerable<Graphic> BlinkerGraphics => graphics ?? (graphics = new[] {GetComponent<Image>()});

        private IReadOnlyDictionary<Graphic, float> originalAlphaValues;

        public IReadOnlyDictionary<Graphic, float> OriginalAlphaValues => originalAlphaValues ??
                                                                          (originalAlphaValues = BlinkerGraphics.ToDictionary(g => g, g => g.color.a));

		private void OnEnable() {
			if(autoStart) this.StartBlinking();
		}

		private void OnDisable() {
			if(autoStart) this.StopBlinking();
		}
	}
}