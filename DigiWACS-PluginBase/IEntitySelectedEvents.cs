using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.PluginBase {
	public class SelectedEntitiesArgs : EventArgs {
		// TODO: Make sure that this can return an array of 1 item or multiple items.
		public ArraySegment<int> EntitiyID { get; set; }
	}

	internal interface IEntitySelectedEvents {

		/// <summary>
		/// No Selected Entities
		/// Called when clicking in open space on the map and no entity is under the mouse pointer
		/// </summary>
		/// <param name="source"></param>
		/// <param name="eventArgs"></param>
		public event EventHandler<SelectedEntitiesArgs> NoEntitySelected;
		void OnNoSelectedEntities( object sender, SelectedEntitiesArgs e ) { }

		/// <summary>
		/// Single Selected Entities
		/// Called when only a single entity is selected
		/// </summary>
		/// <param name="source"></param>
		/// <param name="eventArgs"></param>
		public event EventHandler<SelectedEntitiesArgs> SingleEntitySelected;
		void OnSingleSelectedEntities( object sender, SelectedEntitiesArgs e ) { }

		/// <summary>
		/// Multi Selected Entities
		/// Called when a modifier is held and multiple entities are selected. 
		/// Planned to be array format. 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="eventArgs"></param>
		public event EventHandler<SelectedEntitiesArgs> MultiEntitySelected;
		void OnMultiSelectedEntities( object sender, SelectedEntitiesArgs e ) { }
	}
}
