namespace Assets.Scripts.Interaction.Prototype
{
	/// <summary>
	/// Interface providing methods of objects able of interacting
	/// </summary>
	public interface IInteractor
	{
		/// <summary>
		/// Notify's this object, that given interactive object is in their range
		/// </summary>
		/// <param name="interactive"></param>
		void AddInteractive( IInteractive interactive );

		/// <summary>
		/// Notify's this object, that given interactive object is no longer in range
		/// </summary>
		/// <param name="interactive"></param>
		void RemoveInteractive( IInteractive interactive );
	}
}
