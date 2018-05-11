using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cassoway.Forms
{
	public partial class MainPage : ContentPage
	{
		bool _toggle;

		public MainPage()
		{
			InitializeComponent();
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			_toggle = !_toggle;
			Layout.Constraints = (Layout.ConstraintCollection)Resources[_toggle ? "Constraints" : "AltConstraints"];
		}
	}
}
