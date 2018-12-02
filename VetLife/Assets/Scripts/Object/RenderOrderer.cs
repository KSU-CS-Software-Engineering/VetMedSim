using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Object
{
	/// <summary>
	/// Handles the position of game objects which need to position themselves dynamically relatively to other object
	/// </summary>
	[RequireComponent( typeof( SortingGroup ) )]
	public class RenderOrderer : MonoBehaviour
	{
		#region Constants

		/// <summary>
		/// Label of the sorting layer used for game objects in the foreground
		/// </summary>
		private readonly string FOREGROUND_LAYER = "Foreground";

		/// <summary>
		/// Label of the sorting layer used for game objects in the background
		/// </summary>
		private readonly string BACKGROUND_LAYER = "Furniture";

		#endregion

		#region Fields

		/// <summary>
		/// Sorting group component attached to group game object
		/// </summary>
		private SortingGroup _sortingGroup;

		/// <summary>
		/// Transform of object which serves as target for owner of this script
		/// </summary>
		private Transform _targetTransform;

		/// <summary>
		/// The lowest point of whole object group
		/// </summary>
		private float _lowestPoint = Mathf.Infinity;

		/// <summary>
		/// The highest point of whole object group
		/// </summary>
		private float _highestPoint = Mathf.NegativeInfinity;

		#endregion

		#region Properties

		/// <summary>
		/// <see cref="GameObject"/> to base orientation on
		/// </summary>
		public GameObject OrientationTarget;

		#endregion

		#region Overrides

		private void Start()
		{
			_sortingGroup = GetComponent<SortingGroup>();
			_targetTransform = OrientationTarget.GetComponent<Transform>();

			FindBounds( GetComponentsInChildren<SpriteRenderer>().ToList() );
		}

		private void Update()
		{
			if( ShouldMoveForward() )
			{
				MoveToLayer( FOREGROUND_LAYER );
			}
			else if( ShouldMoveBackward() )
			{
				MoveToLayer( BACKGROUND_LAYER );
			}
		}

		#endregion

		#region Functions

		/// <summary>
		/// Finds the bounds (min and max) of object group
		/// </summary>
		/// <param name="renderers">List of sprite renderers of affected objects</param>
		private void FindBounds( List<SpriteRenderer> renderers )
		{
			if( !renderers.Any() )
			{
				throw new MissingComponentException( "Object does not have any renderable children!" );
			}

			foreach( var renderer in renderers )
			{
				var bounds = renderer.bounds;

				if( bounds.max.y > _highestPoint )
				{
					_highestPoint = bounds.max.y;
				}

				if( bounds.min.y < _lowestPoint )
				{
					_lowestPoint = bounds.min.y;
				}
			}
		}

		/// <summary>
		/// Determines, whether this object should be moved to the foreground sorting layer
		/// </summary>
		/// <returns>True in case this object should be moved forwards, false otherwise</returns>
		private bool ShouldMoveForward()
		{
			return _highestPoint < _targetTransform.position.y
				&& _sortingGroup.sortingLayerName.Equals( BACKGROUND_LAYER );
		}

		/// <summary>
		/// Determines, whether this object should be moved to the background sorting layer
		/// </summary>
		/// <returns>True in case this object should be moved backwards, false otherwise</returns>
		private bool ShouldMoveBackward()
		{
			return _targetTransform.position.y < _lowestPoint
				&& _sortingGroup.sortingLayerName.Equals( FOREGROUND_LAYER );
		}

		/// <summary>
		/// Moves this object to layer specified by its label
		/// </summary>
		/// <param name="layerLabel">Label of sorting layer this object should be moved to</param>
		private void MoveToLayer( string layerLabel )
		{
			_sortingGroup.sortingLayerName = layerLabel;
		}

		#endregion
	}
}
