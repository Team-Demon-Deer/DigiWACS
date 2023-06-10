using System.ComponentModel;

namespace Measurement_Tool {
	public partial class MeasurementTool : Component {
		public MeasurementTool() {
			InitializeComponent();
		}

		public MeasurementTool( IContainer container ) {
			container.Add( this );

			InitializeComponent();
		}
	}
}
