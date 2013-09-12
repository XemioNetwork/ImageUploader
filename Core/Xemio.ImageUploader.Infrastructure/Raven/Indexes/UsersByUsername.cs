using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables querying of users by the username.
    /// </summary>
    public class UsersByUsername : AbstractIndexCreationTask<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersByUsername"/> class.
        /// </summary>
        public UsersByUsername()
        {
            this.Map = users => from user in users
                                select new
                                           {
                                               user.Username
                                           };
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "Users/ByUsername"; }
        }
    }
}
