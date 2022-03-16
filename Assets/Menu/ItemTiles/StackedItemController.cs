using System;

namespace Menu.ItemTiles
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

		public event EventHandler OnHoverStart
		{
			add => _plainController.OnHoverStart += value;
			remove => _plainController.OnHoverStart -= value;
		}

		public void HoverStart()
		{
			_plainController.HoverStart();
		}

		public event EventHandler OnHoverEnd
		{
			add => _plainController.OnHoverEnd += value;
			remove => _plainController.OnHoverEnd -= value;
		}

		public void HoverEnd()
		{
			_plainController.HoverEnd();
		}

		public event EventHandler OnUseSuccess
		{
			add => _plainController.OnUseSuccess += value;
			remove => _plainController.OnUseSuccess -= value;
		}
	}
}