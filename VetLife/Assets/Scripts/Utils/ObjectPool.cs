using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utils
{
	/// <summary>
	/// Represents component attached to pooled objects to identify their parenting pools
	/// </summary>
	internal class PooledObject : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// Reference to the parenting object pool
		/// </summary>
		internal ObjectPool ParentPool { get; set; }

		#endregion
	}

	/// <summary>
	/// Represents bsaic implementation of object pool
	/// </summary>
	public class ObjectPool : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Stack of currently inactive instances of prefab
		/// </summary>
		private readonly Stack<GameObject> _inactiveInstances = new Stack<GameObject>();

		#endregion

		#region Properties

		/// <summary>
		/// Reference to the prefab, which should be stored in this pool
		/// </summary>
		public GameObject Prefab;

		#endregion

		#region Functions
		
		/// <summary>
		/// Retrieves inactive (or new) object from this pool
		/// </summary>
		/// <returns>Inactive object ready to be used</returns>
		public GameObject RetrieveObject()
		{
			GameObject retrievedObject;

			if( _inactiveInstances.Any() )
			{
				retrievedObject = _inactiveInstances.Pop();
			}
			else
			{
				retrievedObject = Instantiate( Prefab );
				retrievedObject.AddComponent<PooledObject>().ParentPool = this;
			}

			retrievedObject.transform.SetParent( null, false );
			retrievedObject.SetActive( true );

			return retrievedObject;
		}

		/// <summary>
		/// Returns given <see cref="GameObject"/> to this pool
		/// </summary>
		/// <param name="gameObject"><see cref="GameObject"/> to be returned to this pool</param>
		/// <remarks>Please note, that if passed <see cref="GameObject"/> does not belong to this pool, it will get destroyed</remarks>
		public void ReturnObject(GameObject gameObject)
		{
			var pooledObject = gameObject.GetComponent<PooledObject>();
			if( pooledObject == null || pooledObject.ParentPool != this )
			{
				Destroy( gameObject );

				return;
			}

			gameObject.transform.SetParent( transform, false );
			gameObject.SetActive( false );

			_inactiveInstances.Push( gameObject );
		}

		#endregion
	}
}
