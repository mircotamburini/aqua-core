﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [Serializable]
    [DataContract]
    [KnownType(typeof(object)), XmlInclude(typeof(object))]
    [KnownType(typeof(object[])), XmlInclude(typeof(object[]))]
    [KnownType(typeof(string)), XmlInclude(typeof(string))]
    [KnownType(typeof(string[])), XmlInclude(typeof(string[]))]
    [KnownType(typeof(DateTimeOffset)), XmlInclude(typeof(DateTimeOffset))]
    [KnownType(typeof(BigInteger)), XmlInclude(typeof(BigInteger))]
    [KnownType(typeof(Complex)), XmlInclude(typeof(Complex))]
    [DebuggerDisplay("{Name,nq}: {Value}")]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Preferred name")]
    public class Property
    {
        public Property()
        {
        }

        public Property(string name, object? value)
        {
            Name = name.CheckNotNull(nameof(name));
            Value = value;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Constructor was removed, use static method Property.From(...) instead.", true)]
        public Property(KeyValuePair<string, object?> property)
            : this(property.Key, property.Value)
        {
        }

        internal protected Property(Property property)
            : this(property.CheckNotNull(nameof(property)).Name, property?.Value)
        {
        }

        [DataMember(Order = 1, IsRequired = true)]
        public string Name { get; set; } = null!;

        [DataMember(Order = 2)]
        public object? Value { get; set; }

        public KeyValuePair<string, object?> ToKeyValuePair()
            => new KeyValuePair<string, object?>(Name ?? string.Empty, Value);

        public static Property From<T>(KeyValuePair<string, T> kvp)
            => new Property(kvp.Key, kvp.Value);

        public static implicit operator Property((string name, object? value) property)
            => new Property(property.name, property.value);

        public static implicit operator (string name, object? value)(Property property)
            => (property.CheckNotNull(nameof(property)).Name, property?.Value);

        public static implicit operator KeyValuePair<string, object?>(Property property)
            => property.CheckNotNull(nameof(property)).ToKeyValuePair();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Method was renamed to 'From'", true)]
        public static Property ToProperty(KeyValuePair<string, object?> keyValuePair)
            => From(keyValuePair);
    }
}