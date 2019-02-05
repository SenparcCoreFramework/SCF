using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Senparc.Scf.Mvc.UI
{
    public class ContentBox : IDisposable
    {
        // Fields
        private bool _disposed;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;
        private bool _showDefaultTabContainer;

        public ContentBox(ViewContext viewContext, bool showDefaultTabContainer)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            this._viewContext = viewContext;
            this._writer = viewContext.Writer;
            this._showDefaultTabContainer = showDefaultTabContainer;
        }

        public void EndCotnentBox()
        {
            this.Dispose(true);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                string contentBoxFooter = (_showDefaultTabContainer ? @"
        </div>
        <!-- End #tab1 -->" :"") +
@"
    </div>
    <!-- End .content-box-content -->
</div>
<!-- End .content-box -->";
                this._disposed = true;
                this._writer.Write(contentBoxFooter);
            }
        }
    }
}
