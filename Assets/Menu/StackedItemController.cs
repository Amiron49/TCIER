namespace Menu
{
	public class StackedItemController : IItemController
	{
		private readonly StackedItemTile _stackedItemTile;
		private readonly IItemController _plainController;

		public StackedItemController(StackedItemTile stackedItemTile, IItemController plainController)
		{
			_stackedItemTile = stackedItemTile;
			_plainController = plainController;
		}
    
		public bool Use(ItemTile tile)
		{
			var anyAvailable = _stackedItemTile.CurrentAmount > 0;
			if (!anyAvailable)
				return false;

			var useSuccess = _plainController.Use(tile);

			if (!useSuccess)
				return false;
        
			_stackedItemTile.Decrement();

			return true;
		}
	}
}