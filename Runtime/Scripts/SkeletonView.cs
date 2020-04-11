﻿using CommonUtils.Extensions;
using UnityEngine;

namespace CommonUtils {
	/// <summary>
	/// Put this component in a SkinnedMeshRenderer to draw its skeleton as gizmos.
	/// </summary>
	[AddComponentMenu("Mesh/Skeleton View")]
	public class SkeletonView : MonoBehaviour { // based on http://answers.unity.com/comments/714888/view.html
		#if UNITY_EDITOR
#pragma warning disable 649
		[SerializeField] private Color       rootColor = Color.green;
		[SerializeField] [Range(0.001f, 0.1f)] private float rootSize = 0.05f;
		[SerializeField] private Color       boneColor = Color.blue;
		[SerializeField] [Range(0.001f, 0.1f)] private float jointSize = 0.01f;
		[SerializeField] private Transform   rootNode;
#pragma warning restore 649

		public Transform RootNode => rootNode;
		public Color RootColor => rootColor;
		public Color BoneColor => boneColor;
		public float RootSize => rootSize;
		public float JointSize => jointSize;
#endif
	}
}