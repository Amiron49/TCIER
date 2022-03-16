using System;

namespace Menu.ItemTiles
{
	public interface IItemController
	{
		bool Use(ItemTile tile);
		event EventHandler OnHoverStart;
		public void HoverStart();
		event EventHandler OnHoverEnd;
		public void HoverEnd();
		event EventHandler OnUseSuccess;
	}
}