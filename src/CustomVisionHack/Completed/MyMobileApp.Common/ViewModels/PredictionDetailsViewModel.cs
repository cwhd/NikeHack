﻿using System.IO;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MyMobileApp.Common
{
	public class PredictionDetailsViewModel : BaseViewModel
	{
		public ICommand ResetDataCommand => new Command(ResetData);
		public ICommand TakePictureCommand => new Command(TakePicture);
		public ICommand MakePredictionCommand => new Command(MakePrediction);

		//public Prediction Prediction

		string _status = "Snap a pic of a shoe to get started";
		public string Status
		{
			get { return _status; }
			set { SetProperty(ref _status, value); }
		}

		public bool CanMakePrediction
		{
			get { return true; }
		}

		byte[] _imageBytes;
		ImageSource _imageSource;
		public ImageSource ImageSource
		{
			get { return _imageSource; }
			set { SetProperty(ref _imageSource, value); OnPropertyChanged(nameof(HasImageSource)); }
		}

		public bool HasImageSource => ImageSource != null;

		public PredictionDetailsViewModel()
		{
			Title = "Make Prediction";
		}

		void ResetData()
		{
			ImageSource = null;
			Status = null;
		}

		#region Take/Choose Picture

		async void TakePicture()
		{
			MediaFile file;

			if(!CrossMedia.Current.IsCameraAvailable)
			{
				//Probably a simulator - let's choose a photo from the library
				file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());
			}
			else
			{
				var options = new StoreCameraMediaOptions
				{
					CompressionQuality = 50,
					PhotoSize = PhotoSize.Small,
				};

				file = await CrossMedia.Current.TakePhotoAsync(options);
			}

			if(file == null)
				return;

			var stream = file.GetStream();
			file.Dispose();

			using(var ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				_imageBytes = ms.ToArray();
			}

			stream.Position = 0;
			ImageSource = ImageSource.FromStream(() => { return stream; });
		}

		#endregion

		#region Make Prediction

		async void MakePrediction()
		{
			if(IsBusy)
				return;

			if(ImageSource == null)
			{
				Status = "Please take a picture first";
				return;
			}

			IsBusy = true;
			Status = "Analyzing picture...";

			var result = await DataStore.Instance.MakePredictionAsync(_imageBytes);

			Status = result == null ? "Bad request" : result.Description;
			IsBusy = false;
		}

		#endregion
	}


}
