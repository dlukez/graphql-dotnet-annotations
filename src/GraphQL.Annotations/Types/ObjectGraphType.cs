using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQL.Annotations.Attributes;

namespace GraphQL.Annotations.Types
{
    public class ObjectGraphType<TModelType> : GraphQL.Types.ObjectGraphType<TModelType>
        where TModelType : class
    {
        public ObjectGraphType(params object[] injectedParameters)
        {
            this.ApplyTypeData<TModelType>();
            this.ApplyProperties<TModelType>();
            this.ApplyMethods<TModelType>(injectedParameters, true);
            ImplementInterfaces();
        }

        private void ImplementInterfaces()
        {
            var type = typeof (TModelType);
            var abstractBaseTypes = GetBaseTypes(type).Where(t => t.GetTypeInfo().IsAbstract);
            var interfaces = type.GetInterfaces();
            foreach (var item in abstractBaseTypes.Concat(interfaces)
                    .Select(t => t.GetGraphTypeFromAttribute<GraphQLInterfaceAttribute>())
                    .Where(t => t != null))
            {
                Interfaces.Add(item);
            }
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            while ((type = type.GetTypeInfo().BaseType) != null)
                yield return type;
        }

        public override string ToString()
        {
            return Name + " - Object Type";
        }
    }
}
