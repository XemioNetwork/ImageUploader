using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables querying of <see cref="AuthenticationData"/> by an user id.
    /// </summary>
    public class AuthenticationDataByUserId : AbstractIndexCreationTask<AuthenticationData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationDataByUserId"/> class.
        /// </summary>
        public AuthenticationDataByUserId()
        {
            this.Map = authenticationDatas => from data in authenticationDatas
                                              select new
                                                         {
                                                             data.UserId
                                                         };
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        public override string IndexName
        {
            get { return "AuthenticationDatas/ByUserId"; }
        }
    }
}
