using System;
using System.IO;
using System.Text;

namespace Karma.Internal
{
    internal class KarmaWriter : IDisposable
    {
        private BinaryWriter writer;
        private Stream stream;
        private bool leaveOpen;

        public KarmaWriter(Stream stream, bool leaveOpen = false)
        {
            this.writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen);
            this.stream = stream;
            this.leaveOpen = leaveOpen;
        }

        public void WriteBool(bool value) => writer.Write(value);
        public void WriteInt16(short value) => writer.Write(value);
        public void WriteInt32(int value) => writer.Write(value);
        public void WriteInt64(long value) => writer.Write(value);
        public void WriteUInt16(ushort value) => writer.Write(value);
        public void WriteUInt32(uint value) => writer.Write(value);
        public void WriteUInt64(ulong value) => writer.Write(value);
        public void WriteDouble(double value) => writer.Write(value);
        public void WriteSingle(float value) => writer.Write(value);
        public void WriteByte(byte value) => writer.Write(value);
        public void WriteSByte(sbyte value) => writer.Write(value);
        public void WriteBytes(byte[] value) => writer.Write(value);
        public void WriteGuid(Guid value) => WriteBytes(value.ToByteArray());

        public void WriteString(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            WriteInt32(bytes.Length);
            WriteBytes(bytes);
        }

        public void Dispose()
        {
            if (!leaveOpen)
            {
                stream.Dispose();
                writer.Dispose();
            }
        }
    }
}
