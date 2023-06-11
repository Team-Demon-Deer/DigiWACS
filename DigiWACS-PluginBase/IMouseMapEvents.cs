using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.PluginBase {
	internal interface IMouseMapEvents {

		//Theorized Mouse Actions
		void OnNoEntitySelect() { }
		void OnSingleEntitySelect() { }
		void OnMultiEntitySelect() { }
	}
}
