using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Senparc.Scf.Core.Enums;

namespace System.Web.Mvc
{
    public static class ShowMessageBoxExtension
    {
        public static ShowMessageBox ShowMessageBox(this HtmlHelper helper, MessageType messageType)
        {
            return new ShowMessageBox(helper.ViewContext, messageType);
        }
    }
    public class ShowMessageBox : IDisposable
    {
        // Fields
        private bool _disposed;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;


        public MessageType MessageType { get; set; }
        public string Message { get; set; }

        public ShowMessageBox(ViewContext viewContext, MessageType messageType)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            this._viewContext = viewContext;
            this._writer = viewContext.Writer;

            this._writer.Write(@"
<div class=""notification {0} png_bg"">
            <a href=""#"" class=""close"">
                <img src=""/Content/backend/images/icons/cross_grey_small.png"" title=""关闭此提示""
                    alt=""close""></a>
            <div>",messageType.ToString());
        }


        public void EndSection()
        {
            this.Dispose();
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
                this._disposed = true;
                this._writer.Write(@"</div></div>");
            }
        }
    }
}
