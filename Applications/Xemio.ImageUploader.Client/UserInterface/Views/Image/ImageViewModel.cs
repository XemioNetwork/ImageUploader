using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.ImageUploader.Client.WebServices;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Client.UserInterface.Views.Image
{
    public class ImageViewModel : Screen
    {
        #region Fields
        private readonly UploadedImagesClient _uploadedImagesClient;
        private readonly Session _session;

        private UploadedImage _image;
        private DateTime _createdDate;
        private byte[] _imageData;
        private string _imageExtension;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageViewModel" /> class.
        /// </summary>
        /// <param name="_uploadedImagesClient">The _uploaded images client.</param>
        /// <param name="session">The session.</param>
        public ImageViewModel(UploadedImagesClient _uploadedImagesClient, Session session)
        {
            this._uploadedImagesClient = _uploadedImagesClient;
            this._session = session;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public UploadedImage Image
        {
            get { return _image; }
            set
            {
                this._image = value;
                this.ImageExtension = this.Image.ImageExtension;
                this.CreatedDate = this.Image.CreatedDate;
            }
        }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate
        {
            get { return this._createdDate; }
            private set 
            { 
                this._createdDate = value;
                this.NotifyOfPropertyChange(() => this.CreatedDate);
            }
        }
        /// <summary>
        /// Gets or sets the image extension.
        /// </summary>
        public string ImageExtension
        {
            get { return this._imageExtension; }
            private set
            {
                this._imageExtension = value;
                this.NotifyOfPropertyChange(() => this.ImageExtension);
            }
        }
        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] ImageData
        {
            get { return this._imageData; }
            private set
            {
                this._imageData = value;
                this.NotifyOfPropertyChange(() => this.ImageData);
            }
        }
        #endregion

        #region Screen Member
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this.LoadImageAsync();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Opens the image in the browser.
        /// </summary>
        public void OpenImageInBrowser()
        {
            string url = this._uploadedImagesClient.GetUploadedImageAddress(this._session.CurrentUser, this.Image);

            Process.Start(url);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the image async.
        /// </summary>
        private async void LoadImageAsync()
        {
            if (this.Image == null)
                return;

            HttpResponseMessage response = await this._uploadedImagesClient.GetUploadedImage(this._session.CurrentUser, this.Image);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string message = await response.Content.ReadAsStringAsync();
                MessageBox.Show(message);
            }
            else 
            {
                this.ImageData = await response.Content.ReadAsByteArrayAsync();
            }
        }
        #endregion
    }
}
