using System.Collections.Generic;
using Assets.Scripts.Interaction.Prototype;
using UnityEngine;

namespace Assets.Scripts.Commands
{
	/// <summary>
	/// Static object providing means to distinguish user commands
	/// </summary>
	public static class CommandRecognizer
	{
		#region Functions

		/// <summary>
		/// Determines type of user input command
		/// </summary>
		/// <param name="interactives">List of interactive objects in proximity</param>
		/// <param name="mainTouch">Current main touch gesture</param>
		/// <returns>Type of user input command</returns>
		public static CommandType Recognize( List<IInteractive> interactives, Touch? mainTouch )
		{
			if( !mainTouch.HasValue )
			{
				return CommandType.NONE;
			}

			var touchPoint = Camera.main.ScreenToWorldPoint( mainTouch.Value.position );
			if( interactives.Exists( interactive => interactive.IsTargeted( touchPoint ) ) )
			{
				return CommandType.INTERACTION;
			}
			else
			{
				return CommandType.MOVEMENT;
			}
		}

		#endregion
	}
}
