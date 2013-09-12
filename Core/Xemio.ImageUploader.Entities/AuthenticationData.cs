namespace Xemio.ImageUploader.Entities
{
    /// <summary>
    /// Contains information about the authentication data from the <see cref="User"/>.
    /// </summary>
    public class AuthenticationData : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        public byte[] PasswordHash { get; set; }
    }
}