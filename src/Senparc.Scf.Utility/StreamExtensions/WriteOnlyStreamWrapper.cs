using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Scf.Utility.StreamExtensions
{
    /// <summary>
    /// Write-Only Streams https://blog.stephencleary.com/2016/11/ziparchive-on-write-only-streams.html
    /// </summary>
    public class WriteOnlyStreamWrapper : Stream
    {
        private readonly Stream _stream;
        private long _position;

        public WriteOnlyStreamWrapper(Stream stream)
        {
            _stream = stream;
        }

        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;

        public override long Position
        {
            get { return _position; }
            set
            {
                throw new NotSupportedException();
            }
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            _position += count;
            _stream.Write(buffer, offset, count);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            _position += count;
            return _stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult) => _stream.EndWrite(asyncResult);

        public override void WriteByte(byte value)
        {
            _position += 1;
            _stream.WriteByte(value);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            _position += count;
            return _stream.WriteAsync(buffer, offset, count, cancellationToken);
        }



        // Crap that we just have to forward.

        public override bool CanTimeout => _stream.CanTimeout;
        public override int ReadTimeout
        {
            get { return _stream.ReadTimeout; }
            set { _stream.ReadTimeout = value; }
        }
        public override int WriteTimeout
        {
            get { return _stream.WriteTimeout; }
            set { _stream.WriteTimeout = value; }
        }

        public override void Flush() => _stream.Flush();
        public override Task FlushAsync(CancellationToken cancellationToken) => _stream.FlushAsync(cancellationToken);

        public override void Close()
        {
            _stream.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _stream.Dispose();
        }


        // Unsupported operations.

        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
