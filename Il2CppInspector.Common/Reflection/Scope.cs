﻿/*
    Copyright 2017-2019 Katy Coe - http://www.djkaty.com - https://github.com/djkaty

    All rights reserved.
*/

using System.Collections.Generic;

namespace Il2CppInspector.Reflection
{
    // A code scope with which to evaluate how to output type references
    public class Scope
    {
        // A scope at the root level with no available namespaces (guarantees full-name retrieval for any type)
        public static Scope Empty = new Scope();

        // The scope we are currently in
        public TypeInfo Current;

        // The list of namespace using directives in the file
        public IEnumerable<string> Namespaces;
    }
}
