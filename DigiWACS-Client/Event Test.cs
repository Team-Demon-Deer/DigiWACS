using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWACS.Client {
	internal class Event_Test {

		public void OnTestedEvent(object sender, EventArgs e) {
			Trace.WriteLine("OnTestedEvent Triggered");
		}
	}
}
