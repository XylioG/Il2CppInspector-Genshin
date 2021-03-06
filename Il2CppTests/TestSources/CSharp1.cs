/*
    Copyright 2017-2021 Katy Coe - http://www.djkaty.com - https://github.com/djkaty
    Copyright 2020 Robert Xiao - https://robertxiao.ca

    All rights reserved.
*/

using System;
using System.Runtime.InteropServices;

/* C# 1.0 feature test */
namespace Il2CppTests.TestSources
{
    public class SimpleClass : SimpleInterface
    {
        public SimpleStruct ss;
        public int i;
        public const int fieldWithDefaultValue = 3;
        public const LayoutKind fieldWithDefaultEnumValue = LayoutKind.Sequential;
        public const SimpleClass fieldWithDefaultReferenceValue = null;

        public static SimpleStruct StaticFunc(SimpleStruct ss) {
            Console.WriteLine(ss);
            return new SimpleStruct();
        }

        public SimpleStruct InstanceFunc(SimpleStruct ss) {
            Console.WriteLine(ss);
            return this.ss;
        }

        public int func(int val) {
            return val + 42;
        }

        public void funcWithDefaultValue(int a, int b = 2) { }
        public void funcWithDefaultEnumValue(int a, LayoutKind b = LayoutKind.Explicit) { }
        public void funcWithDefaultReferenceValue(int a, SimpleClass b = null) { }

        public delegate SimpleStruct SimpleDelegate(SimpleStruct ss);
        public event SimpleDelegate SimpleEvent;
        public int SimpleProperty {
            get { return 0; }
            set { SimpleEvent(ss); }
        }
    }

    public struct SimpleStruct : SimpleInterface
    {
        public SimpleClass sc;
        public int i;

        public static SimpleStruct StaticFunc(SimpleStruct ss) {
            Console.WriteLine(ss);
            return new SimpleStruct();
        }

        public SimpleClass InstanceFunc(SimpleStruct ss) {
            Console.WriteLine(ss);
            return this.sc;
        }

        public int func(int val) {
            return ((SimpleInterface)sc).func(val) + 13;
        }
    }

    public interface SimpleInterface
    {
        int func(int val);
    }
}
