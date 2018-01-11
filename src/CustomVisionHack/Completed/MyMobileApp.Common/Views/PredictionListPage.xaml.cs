using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyCommonLibrary;
using Xamarin.Forms;

namespace MyMobileApp.Common
{
	public partial class PredictionListPage : ContentPage
	{
		PredictionListViewModel _viewModel = new PredictionListViewModel();

		public PredictionListPage()
		{
			InitializeComponent();
			BindingContext = _viewModel;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as Prediction;
			if(item == null)
				return;

			//await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

			// Manually deselect item
			listView.SelectedItem = null;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if(_viewModel.Items.Count == 0)
				_viewModel.LoadItemsCommand.Execute(null);
		}
	}
}
