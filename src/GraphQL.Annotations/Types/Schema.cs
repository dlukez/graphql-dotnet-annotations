using System;
using GraphQL.Types;

namespace GraphQL.Annotations.Types
{
    public class Schema<TRootQuery> : Schema where TRootQuery : class
    {
        public Schema(params object[] injectedObjects) : base(new FuncServiceProvider(t =>
            {
                var genericType = t.IsConstructedGenericType ? t.GetGenericTypeDefinition() : null;
                if (genericType == typeof(ObjectGraphType<>) || genericType == typeof(InterfaceGraphType<>))
                    return (GraphType)Activator.CreateInstance(t, injectedObjects);
                return (GraphType)Activator.CreateInstance(t);
            }), false)
        {
            Query = (IObjectGraphType)((IServiceProvider)this).GetService(typeof(ObjectGraphType<TRootQuery>));
        }

        public override string ToString()
        {
            return "Schema - " + typeof (TRootQuery).FullName;
        }
    }
}
