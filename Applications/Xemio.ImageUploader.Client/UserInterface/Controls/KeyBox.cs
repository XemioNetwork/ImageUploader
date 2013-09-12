using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Xemio.ImageUploader.Client.UserInterface.Controls
{
    /// <summary>
    /// A textbox displaying pressed keys.
    /// </summary>
    public class KeyBox : TextBox
    {
        #region Fields
        private bool _keysReleased = true;
        #endregion Fields

        #region Properties
        /// <summary>
        /// The <see cref="Keys"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeysProperty =
            DependencyProperty.Register("Keys", typeof (ObservableCollection<Key>), typeof (KeyBox), new PropertyMetadata(default(ObservableCollection<Key>), KeysChanged));

        /// <summary>
        /// Gets or sets the keys.
        /// </summary>
        public ObservableCollection<Key> Keys
        {
            get { return (ObservableCollection<Key>) GetValue(KeysProperty); }
            set { SetValue(KeysProperty, value); }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBox"/> class.
        /// </summary>
        public KeyBox()
        {
            this.IsReadOnly = true;
            this.IsReadOnlyCaretVisible = true;

            this.Keys = new ObservableCollection<Key>();
        }
        #endregion Constructors

        #region Private Methods
        /// <summary>
        /// Called when the <see cref="Keys"/> property will be changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void KeysChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                KeyBox keyBox = dependencyObject as KeyBox;

                ObservableCollection<Key> oldCollection = e.OldValue as ObservableCollection<Key>;
                oldCollection.CollectionChanged -= keyBox.KeysCollectionChanged;
            }

            if (e.NewValue != null)
            {
                KeyBox keyBox = dependencyObject as KeyBox;

                ObservableCollection<Key> newCollection = e.NewValue as ObservableCollection<Key>;
                newCollection.CollectionChanged += keyBox.KeysCollectionChanged;

                keyBox.KeysCollectionChanged(keyBox, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        /// <summary>
        /// Called when the <see cref="Keys"/> collection changed in any way.
        /// Will update the text.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="notifyCollectionChangedEventArgs">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void KeysCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            StringBuilder keyString = new StringBuilder();

            foreach (Key key in this.Keys)
            {
                if (keyString.Length > 0)
                {
                    keyString.Append(" + ");
                }

                keyString.Append(key.ToString());
            }

            this.Text = keyString.ToString();
        }
        #endregion Private Methods

        #region TextBox Member
        /// <summary>
        /// Invoked when a <see cref="Key"/> was pressed.
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this._keysReleased)
            {
                this.Keys.Clear();
                this._keysReleased = false;
            }

            if (!this.Keys.Contains(e.Key))
            {
                this.Keys.Add(e.Key);
            }
        }
        /// <summary>
        /// Invoked whenever <see cref="Key"/> was released.
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            this._keysReleased = true;
        }
        #endregion TextBox Member
    }
}
