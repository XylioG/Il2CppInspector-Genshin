﻿/*
    Copyright 2020-2021 Katy Coe - http://www.djkaty.com - https://github.com/djkaty

    All rights reserved.
*/

using System;

namespace Il2CppInspector.Cpp
{
    // A field in a C++ type
    public class CppField
    {
        // The name of the field
        public string Name { get; }

        // The type of the field
        public CppType Type { get; }

        // Whether the field is const
        public bool IsConst { get; }

        // The offset of the field into the type
        public int Offset { get; internal set; }

        // The size of the field in bits
        public int BitfieldSize { get; }

        // The offset of the field into the type in bytes
        public int OffsetBytes => Offset / 8;

        // The size of the field
        public int Size => (BitfieldSize > 0 ? BitfieldSize : Type.Size);

        public int SizeBytes => (Size / 8) + (Size % 8 > 0 ? 1 : 0);

        // The LSB of the bitfield
        public int BitfieldLSB => Offset % 8;

        // The MSB of the bitfield
        public int BitfieldMSB => BitfieldLSB + Size - 1;

        // Initialize field
        public CppField(string name, CppType type, int bitfieldSize = 0, bool isConst = false) {
            Name = name;
            Type = type;
            BitfieldSize = bitfieldSize;
            IsConst = isConst;
        }

        // C++ representation of field
        public virtual string ToString(string format = "") {
            var offset = format == "o" ? $"/* 0x{OffsetBytes:x2} - 0x{OffsetBytes + SizeBytes - 1:x2} (0x{SizeBytes:x2}) */ " : "";

            var prefix = (IsConst ? "const " : "");

            var field = Type switch {
                // nested anonymous types (trim semi-colon and newline from end)
                CppComplexType t when string.IsNullOrEmpty(t.Name) => (format == "o"? "\n" : "") 
                                    + t.ToString(format)[..^2] + (Name.Length > 0? " " + Name : ""),
                // function pointers
                CppFnPtrType t when string.IsNullOrEmpty(t.Name) => t.ToFieldString(Name, format),
                // regular fields
                _ => $"{Type.ToFieldString(Name, format)}" + (BitfieldSize > 0? $" : {BitfieldSize}" : "")
            };

            var suffix = "";

            // bitfields
            if (BitfieldSize > 0 && format == "o")
                suffix += $" /* bits {BitfieldLSB} - {BitfieldMSB} */";

            return offset + prefix + field + suffix;
        }
        public override string ToString() => ToString();
    }

    // An enum key and value pair
    public class CppEnumField : CppField
    {
        // The enum type this field belongs to
        public CppEnumType DeclaringType { get; }

        // The value of this key name
        public object Value { get; }

        public CppEnumField(CppEnumType declType, string name, CppType type, object value) : base(name, type)
            => (DeclaringType, Value) = (declType, value);

        // We output as hex to avoid unsigned value compiler errors for top bit set values in VS <= 2017
        // We'll get compiler warnings instead but it will still compile
        public override string ToString(string format = "") {
            var signed = Type.Name.StartsWith("int"); // int8/16/32/64_t or uint8/16/32/64_t/bool/float/double

            // Signed number with top bit set (only perform cast if underlying type is signed)
            var fieldIsNegative = signed && ((long) Convert.ChangeType(Value, typeof(long))) < 0;

            var fieldName = (format.Contains('c')? DeclaringType.Name + "_" : "") + Name;

            if (fieldIsNegative)
                return $"{fieldName} = {Value}";

            return string.Format("{0} = 0x{1:x" + Type.SizeBytes * 2 + "}", fieldName, Value);
        }
    }
}
