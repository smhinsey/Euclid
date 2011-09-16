namespace Euclid.Common.Policy
{
	/// <summary>
	/// Policies are used to enforce domain-level business rules during the execution of certain events.
	/// 
	/// Due to a policy's need to be aware of agent-level events this will probably be moved.
	/// </summary>
	/// <typeparam name="TPolicyContext"></typeparam>
	public interface IRuntimePolicy<in TPolicyContext>
	{
		/// <summary>
		/// Executes a policy against its context data and returns an indicator of whether or not it is upheld.
		/// </summary>
		/// <param name="context">Data required by the policy to determine its applicability.</param>
		/// <returns>Whether or not the policy is upheld by the data in its context.</returns>
		bool Assert(TPolicyContext context);
	}
}