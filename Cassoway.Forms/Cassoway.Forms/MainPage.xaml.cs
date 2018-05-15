using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassoway.Forms.Layout;
using Xamarin.Forms;

namespace Cassoway.Forms
{
	public partial class MainPage : ContentPage
	{
		bool _toggle;

		public MainPage()
		{
			InitializeComponent();

			var mainCollection = (ConstraintCollection) Resources["Constraints"];
			var altCollection = (ConstraintCollection) Resources["AltConstraints"];
			
			mainCollection.Add(PositionConstraint.CenterX(Toggle).ToCenterX(Layout));			
			mainCollection.Add(PositionConstraint.CenterY(Toggle).ToCenterY(Layout));
			mainCollection.Add(DimensionConstraint.Width(Toggle, -20).ToWidth(Layout, 0.5f));
			mainCollection.Add(DimensionConstraint.Height(Toggle, -20).ToHeight(Layout, 0.3f));
			
			altCollection.Add(PositionConstraint.Left(Toggle).ToLeft(LeftLabel));
			altCollection.Add(DimensionConstraint.Width(Toggle, 400));
			altCollection.Add(PositionConstraint.Top(Toggle).ToBottom(BottomLabel));
			altCollection.Add(PositionConstraint.Bottom(Toggle).ToBottom(Layout));
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			_toggle = !_toggle;
			Layout.Constraints = (ConstraintCollection)Resources[_toggle ? "Constraints" : "AltConstraints"];
		}
	}
}
