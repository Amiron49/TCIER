namespace InternalLogic
{
	public interface IBodyModifier
	{
		BodyModifiers Modifies { get; }
		ModifierType How { get; }
		float Value { get; }
	}
}