using System;
using System.IO;
using System.Text;

namespace Karma.Internal
{
    internal class KarmaReader : IDisposable
    {
        private BinaryReader reader;
        private Stream stream;
        private bool leaveOpen;

        public KarmaReader(Stream stream, bool leaveOpen = false)
        {
            this.reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen);
            this.stream = stream;
            this.leaveOpen = leaveOpen;
        }

        public bool ReadBool() => reader.ReadBoolean();
        public short ReadInt16() => reader.ReadInt16();
        public int ReadInt32() => reader.ReadInt32();
        public long ReadInt64() => reader.ReadInt64();
        public ushort ReadUInt16() => reader.ReadUInt16();
        public uint ReadUInt32() => reader.ReadUInt32();
        public ulong ReadUInt64() => reader.ReadUInt64();
        public double ReadDouble() => reader.ReadDouble();
        public float ReadSingle() => reader.ReadSingle();
        public byte ReadByte() => reader.ReadByte();
        public sbyte ReadSByte() => reader.ReadSByte();
        public byte[] ReadBytes(int count) => reader.ReadBytes(count);
        public Guid ReadGuid() => new Guid(ReadBytes(16));

        public string ReadString()
        {
            var length = ReadInt32();
            var bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public void Dispose()
        {
            if (!leaveOpen)
            {
                stream.Dispose();
                reader.Dispose();
            }
        }
    }
}
