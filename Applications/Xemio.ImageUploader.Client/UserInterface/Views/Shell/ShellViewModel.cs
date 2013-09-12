using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Xemio.ImageUploader.Client.Extensions;
using Xemio.ImageUploader.Client.UserInterface.Views.Image;
using Xemio.ImageUploader.Client.WebServices;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Client.UserInterface.Views.Shell
{
    public class ShellViewModel : Conductor<ImageViewModel>.Collection.AllActive
    {
        #region Fields
        private readonly UsersClient _usersClient;
        private readonly UploadedImagesClient _uploadedImagesClient;
        private readonly Session _session;
        private readonly Func<ImageViewModel> _imageViewModelFactory;

        private ObservableCollection<Key> _selectAreaShortcut;
        private ObservableCollection<Key> _selectFileShortcut;
        private ObservableCollection<Key> _currentWindowShortcut;
        private ObservableCollection<Key> _wholeScreenShortcut;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel" /> class.
        /// </summary>
        /// <param name="usersClient">The users client.</param>
        /// <param name="uploadedImagesClient">The uploaded images client.</param>
        /// <param name="session">The session.</param>
        /// <param name="imageViewModelFactory">The image view model factory.</param>
        public ShellViewModel(UsersClient usersClient, UploadedImagesClient uploadedImagesClient, Session session, Func<ImageViewModel> imageViewModelFactory)
        {
            this._usersClient = usersClient;
            this._uploadedImagesClient = uploadedImagesClient;
            this._session = session;
            this._imageViewModelFactory = imageViewModelFactory;
        }
        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets the shortcuts to select an area.
        /// </summary>
        public ObservableCollection<Key> SelectAreaShortcut
        {
            get { return this._selectAreaShortcut; }
            set 
            { 
                this._selectAreaShortcut = value;
                this.NotifyOfPropertyChange(() => this.SelectAreaShortcut);
            }
        }
        /// <summary>
        /// Gets or sets the shortcuts to select an file.
        /// </summary>
        public ObservableCollection<Key> SelectFileShortcut
        {
            get { return this._selectFileShortcut; }
            set
            {
                this._selectFileShortcut = value;
                this.NotifyOfPropertyChange(() => this.SelectFileShortcut);
            }
        }
        /// <summary>
        /// Gets or sets the shortcut to make a screenshot of the current window.
        /// </summary>
        public ObservableCollection<Key> CurrentWindowShortcut
        {
            get { return this._currentWindowShortcut; }
            set
            {
                this._currentWindowShortcut = value;
                this.NotifyOfPropertyChange(() => this.CurrentWindowShortcut);
            }
        }
        /// <summary>
        /// Gets or sets the shortcut to make a screenshot of the whole screen.
        /// </summary>
        public ObservableCollection<Key> WholeScreenShortcut
        {
            get { return this._wholeScreenShortcut; }
            set
            {
                this._wholeScreenShortcut = value;
                this.NotifyOfPropertyChange(() => this.WholeScreenShortcut);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the "SaveSettings" method is executable.
        /// </summary>
        public bool CanSaveSettings
        {
            get
            {
                if (this.SelectAreaShortcut == null ||
                    this.SelectFileShortcut == null ||
                    this.CurrentWindowShortcut == null ||
                    this.WholeScreenShortcut == null)
                    return false;

                return !this.SelectAreaShortcut.SequenceEqual(this._session.CurrentUser.Settings.SelectAreaKeys) ||
                       !this.SelectFileShortcut.SequenceEqual(this._session.CurrentUser.Settings.SelectFileKeys) ||
                       !this.CurrentWindowShortcut.SequenceEqual(this._session.CurrentUser.Settings.CurrentWindowKeys) ||
                       !this.WholeScreenShortcut.SequenceEqual(this._session.CurrentUser.Settings.WholeScreenKeys);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the "CancelSettings" method is executable.
        /// </summary>
        public bool CanCancelSettings
        {
            get { return this.CanSaveSettings; }
        }
        #endregion Properties

        #region IScreen Member
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this.DisplayName = "Xemio Share";

            this.ReLoadShortcuts();
            this.LoadImages();
        }
        #endregion IScreen Member

        #region Methods
        /// <summary>
        /// Saves the settings async.
        /// </summary>
        public async Task SaveSettings()
        {
            this._session.CurrentUser.Settings.SelectAreaKeys = this.SelectAreaShortcut.ToArray();
            this._session.CurrentUser.Settings.SelectFileKeys = this.SelectFileShortcut.ToArray();
            this._session.CurrentUser.Settings.CurrentWindowKeys = this.CurrentWindowShortcut.ToArray();
            this._session.CurrentUser.Settings.WholeScreenKeys = this.WholeScreenShortcut.ToArray();

            HttpResponseMessage response = await this._usersClient.PutUserAsync(this._session.CurrentUser);
            if (response.StatusCode != HttpStatusCode.OK)
            { 
                string message = await response.Content.ReadAsStringAsync();
                MessageBox.Show(message);
                return;
            }

            this.ShortCutsCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        /// <summary>
        /// Cancels the setting updates.
        /// </summary>
        public void CancelSettings()
        {
            this.ReLoadShortcuts();

            this.ShortCutsCollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion Methods

        #region Private Methods
        /// <summary>
        /// Loads the user and creates the images.
        /// </summary>
        private async void LoadImages()
        {
            HttpResponseMessage response = await this._uploadedImagesClient.GetAllUploadedImages(this._session.CurrentUser);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string message = await response.Content.ReadAsStringAsync();
                MessageBox.Show(message);
                return;
            }

            List<UploadedImage> uploadedImages = await response.Content.ReadAsAsync<List<UploadedImage>>();
            foreach (var image in uploadedImages)
            {
                ImageViewModel imageViewModel = this._imageViewModelFactory();
                imageViewModel.Image = image;

                this.ActivateItem(imageViewModel);
            }
        }
        /// <summary>
        /// Reloads the shortcuts.
        /// </summary>
        private void ReLoadShortcuts()
        {
            this.SelectAreaShortcut = new ObservableCollection<Key>(this._session.CurrentUser.Settings.SelectAreaKeys);
            this.SelectFileShortcut = new ObservableCollection<Key>(this._session.CurrentUser.Settings.SelectFileKeys);
            this.CurrentWindowShortcut = new ObservableCollection<Key>(this._session.CurrentUser.Settings.CurrentWindowKeys);
            this.WholeScreenShortcut = new ObservableCollection<Key>(this._session.CurrentUser.Settings.WholeScreenKeys);

            this.SelectAreaShortcut.CollectionChanged += ShortCutsCollectionChanged;
            this.SelectFileShortcut.CollectionChanged += ShortCutsCollectionChanged;
            this.CurrentWindowShortcut.CollectionChanged += ShortCutsCollectionChanged;
            this.WholeScreenShortcut.CollectionChanged += ShortCutsCollectionChanged;
        }
        /// <summary>
        /// Called when a shortcut-collection was changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="notifyCollectionChangedEventArgs">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void ShortCutsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.NotifyOfPropertyChange(() => this.CanSaveSettings);
            this.NotifyOfPropertyChange(() => this.CanCancelSettings);
        }
        #endregion Private Methods
    }
}
