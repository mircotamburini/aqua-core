﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Aqua.Tests.Serialization.Dynamic.DynamicObject
{
    using Aqua.Dynamic;
    using Shouldly;
    using System;
    using Xunit;

    public abstract class When_using_dynamic_object_with_circular_reference
    {
#pragma warning disable SA1128 // Put constructor initializers on their own line
#pragma warning disable SA1502 // Element should not be on a single line

        public class JsonSerializer : When_using_dynamic_object_with_circular_reference
        {
            public JsonSerializer() : base(JsonSerializationHelper.Serialize) { }
        }

        public class DataContractSerializer : When_using_dynamic_object_with_circular_reference
        {
            public DataContractSerializer() : base(DataContractSerializationHelper.Serialize) { }
        }

        // XML serialization doesn't support circular references
#if NET
        public class BinaryFormatter : When_using_dynamic_object_with_circular_reference
        {
            public BinaryFormatter() : base(BinarySerializationHelper.Serialize) { }
        }
#endif

#if NET && !NETCOREAPP2
        public class NetDataContractSerializer : When_using_dynamic_object_with_circular_reference
        {
            public NetDataContractSerializer() : base(NetDataContractSerializationHelper.Serialize) { }
        }
#endif

#pragma warning restore SA1502 // Element should not be on a single line
#pragma warning restore SA1128 // Put constructor initializers on their own line

        private readonly DynamicObject serializedObject;

        protected When_using_dynamic_object_with_circular_reference(Func<DynamicObject, DynamicObject> serialize)
        {
            dynamic object_0 = new DynamicObject();
            dynamic object_1 = new DynamicObject();
            dynamic object_2 = new DynamicObject();

            object_0.Ref_1 = object_1;
            object_1.Ref_2 = object_2;
            object_2.Ref_0 = object_0;

            serializedObject = serialize(object_0);
        }

        [Fact]
        public void Clone_should_contain_circular_reference()
        {
            var reference = serializedObject
                .Get<DynamicObject>("Ref_1")
                .Get<DynamicObject>("Ref_2")
                .Get<DynamicObject>("Ref_0");

            reference.ShouldBeSameAs(serializedObject);
        }
    }
}
