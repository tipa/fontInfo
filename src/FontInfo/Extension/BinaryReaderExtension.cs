﻿using System;
using System.IO;

namespace FontInfo.Extension
{
    internal static class BinaryReaderExtension
    {
        private static uint toUInt32BE(uint value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return value;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            return (uint)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | (bytes[3]));
        }

        private static int toInt32BE(int value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return value;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            return ((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | (bytes[3]));
        }

        private static ushort toUInt16BE(ushort value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return value;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            return (ushort)((bytes[0] << 8) | (bytes[1]));
        }

        private static short toInt16BE(short value)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return value;
            }
            byte[] bytes = BitConverter.GetBytes(value);
            return (short)((bytes[0] << 8) | (bytes[1]));
        }

        public static uint ReadUInt32BE(this BinaryReader reader)
        {
            return toUInt32BE(reader.ReadUInt32());
        }

        public static int ReadInt32BE(this BinaryReader reader)
        {
            return toInt32BE(reader.ReadInt32());
        }

        public static ushort ReadUInt16BE(this BinaryReader reader)
        {
            return toUInt16BE(reader.ReadUInt16());
        }

        public static short ReadInt16BE(this BinaryReader reader)
        {
            return toInt16BE(reader.ReadInt16());
        }

        public static double ReadInt32FixedBE(this BinaryReader reader)
        {   
            short hi = toInt16BE(reader.ReadInt16());
            ushort lo = toUInt16BE(reader.ReadUInt16());
            
            double result = Math.Round(((hi) + ((double)lo/ushort.MaxValue)) * 10000) / 10000;
            return result;
        }

        

        public static void Skip(this BinaryReader reader, int byteCount)
        {
            reader.ReadBytes(byteCount);
        }
    }
}