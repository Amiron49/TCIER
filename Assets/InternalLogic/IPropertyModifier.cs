namespace InternalLogic
{
	public interface IPropertyModifier<out TProperty>
	{
		TProperty Modifies { get; }
		ModifierType How { get; }
		float Value { get; }
	}
}