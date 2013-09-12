using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.Core.Logging;
using Raven.Client;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Infrastructure.Controllers
{
    /// <summary>
    /// Base class for all api controllers handling the session management.
    /// </summary>
    public abstract class BaseController : ApiController
    {
        #region Properties
        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IAsyncDocumentSession DocumentSession { get; private set; }
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        public Task<User> CurrentUser
        {
            get
            {
                return this.DocumentSession.LoadAsync<User>(Thread.CurrentPrincipal.Identity.Name);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        protected BaseController(IAsyncDocumentSession documentSession)
        {
            this.DocumentSession = documentSession;
            this.Logger = NullLogger.Instance;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        public async override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var response = await base.ExecuteAsync(controllerContext, cancellationToken);

            using (this.DocumentSession)
            { 
                await this.DocumentSession.SaveChangesAsync();
            }

            return response;
        }
        #endregion
    }
}
