// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading;
using Debug = System.Diagnostics.Debug;

using Internal.TypeSystem;

namespace Internal.TypeSystem.Ecma
{
    // This file has implementations of the .Interfaces.cs logic from its base type.

    public sealed partial class EcmaType : MetadataType
    {
        private DefType[] _implementedInterfaces;

        public override DefType[] ExplicitlyImplementedInterfaces
        {
            get
            {
                if (_implementedInterfaces == null)
                    return InitializeImplementedInterfaces();
                return _implementedInterfaces;
            }
        }

        private DefType[] InitializeImplementedInterfaces()
        {
            var interfaceHandles = _typeDefinition.GetInterfaceImplementations();

            int count = interfaceHandles.Count;
            if (count == 0)
                return (_implementedInterfaces = Array.Empty<DefType>());

            DefType[] implementedInterfaces = new DefType[count];
            int i = 0;
            foreach (var interfaceHandle in interfaceHandles)
            {
                var interfaceImplementation = this.MetadataReader.GetInterfaceImplementation(interfaceHandle);
                implementedInterfaces[i++] = (DefType)_module.GetType(interfaceImplementation.Interface);
            }

            return (_implementedInterfaces = implementedInterfaces);
        }
    }
}
