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
            this.ApplyProperties();
            this.ApplyMethods(injectedParameters, true);
            ImplementInterfaces();
        }

        private void ImplementInterfaces()
        {
            var type = typeof (TModelType);
            var baseTypes = GetBaseTypes(type).Where(t => t.GetTypeInfo().IsAbstract);
            var interfaces = type.GetInterfaces();
            var implementedInterfaces = baseTypes.Concat(interfaces);
            foreach (var implementedInterface in implementedInterfaces)
            {
                var interfaceAttr = implementedInterface.GetTypeInfo().GetCustomAttribute<GraphQLInterfaceAttribute>(false);
                if (interfaceAttr == null)
                    continue;

                Interface(implementedInterface.ToGraphType());
            }
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            while (type.GetTypeInfo().BaseType != null)
            {
                type = type.GetTypeInfo().BaseType;
                yield return type;
            }
        }

        public override string ToString()
        {
            return Name + " - Object Type";
        }
    }
}
