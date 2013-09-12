namespace Xemio.ImageUploader.Entities
{
    /// <summary>
    /// A base class for all aggregate roots.
    /// </summary>
    public class AggregateRoot
    {
        /// <summary>
        /// Gets or sets the id of the aggregate root.
        /// </summary>
        public string Id { get; set; }
    }
}