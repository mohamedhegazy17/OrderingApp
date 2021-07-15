using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ordering.Client
{
    #region oEnvelop


    /// <summary> SerializationReader.  Extends BinaryReader to add additional data types,
    /// handle null strings and simplify use with ISerializable. </summary>
    public class SerializationReader : BinaryReader
    {

        private SerializationReader(Stream s) : base(s) { }


        public static SerializationReader GetReader(Stream s)
        {
            return new SerializationReader(s);
        }

        /// <summary> Reads a string from the buffer.  Overrides the base implementation so it can cope with nulls. </summary>
        public override string ReadString()
        {
            var t = ReadByte();
            if (t == 11) return base.ReadString();
            return null;
        }

        /// <summary> Reads a byte array from the buffer, handling nulls and the array length. </summary>
        public byte[] ReadByteArray()
        {
            int len = ReadInt32();
            if (len > 0) return ReadBytes(len);
            if (len < 0) return null;
            return new byte[0];
        }

        /// <summary> Reads a char array from the buffer, handling nulls and the array length. </summary>
        public char[] ReadCharArray()
        {
            int len = ReadInt32();
            if (len > 0) return ReadChars(len);
            if (len < 0) return null;
            return new char[0];
        }

        /// <summary> Reads a DateTime from the buffer. </summary>
        public DateTime ReadDateTime() { return new DateTime(ReadInt64()); }

        /// <summary> Reads a generic list from the buffer. </summary>
        public IList<T> ReadList<T>()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            IList<T> d = new List<T>();
            for (int i = 0; i < count; i++) d.Add((T)ReadObject());
            return d;
        }

        public ArrayList ReadList()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            ArrayList d = new ArrayList();
            for (int i = 0; i < count; i++) d.Add(ReadObject());
            return d;
        }

        public BitArray ReadBitArray()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            BitArray d = new BitArray(count, false);
            for (int i = 0; i < count; i++) d[i] = ReadBoolean();
            return d;
        }

        /// <summary> Reads a generic Dictionary from the buffer. </summary>
        public Dictionary<T, U> ReadDictionary<T, U>()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            Dictionary<T, U> d = new Dictionary<T, U>();
            for (int i = 0; i < count; i++) d[(T)ReadObject()] = (U)ReadObject();
            return d;
        }

        /// <summary> Reads an object which was added to the buffer by WriteObject. </summary>
        public object ReadObject()
        {
            return base.ReadString();
        }

        public object[] ReadObjects()
        {
            int count = ReadInt32();
            if (count < 0) return null;
            object[] d = new object[count];
            for (int i = 0; i < count; i++)
                d[i] = ReadObject();
            return d;
        }

        public ArrayList ReadInnerMessages(_MessageType MessageType)
        {
            int count = ReadInt32();
            if (count < 0) return null;
            ArrayList d = new ArrayList();
            for (int i = 0; i < count; i++)
                d.Add(ReadInnerMessage(MessageType));
            return d;
        }

        public object ReadInnerMessage(_MessageType MessageType)
        {
            switch (MessageType)
            {
                case _MessageType.ClientLogIn:
                    return (new oClientLogIn().Deserialize(BaseStream));
                    break;
                case _MessageType.UserInfo:
                    return (new oUserInfo().Deserialize(BaseStream));
                    break;
               

                default:
                    return new BinaryFormatter().Deserialize(BaseStream);
                    break;
            }
        }
    } // SerializationReader

    #endregion
}
