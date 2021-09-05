using GraphQL.Types;
using Shouldly;
using System.Reflection;
using Xunit;
using System.Collections.Generic;

namespace GraphQL.Annotations.Tests
{
    public class MethodResolverTests
    {
        [Fact]
        public void MethodResolver_ResolvesCorrectly()
        {
            // Arrange
            var method = typeof(Counter).GetMethod(nameof(Counter.Increment));
            var methodResolver = new MethodResolver(method);

            var counter = new Counter();
            var context = new ResolveFieldContext() { Source = counter };

            // Act
            var result = (int)methodResolver.Resolve(context);

            // Assert
            result.ShouldBe(1);
            counter.Value.ShouldBe(1);
        }

        [Fact]
        public void MethodResolver_SupportsEnumArguments()
        {
            // Arrange
            var method = typeof(Counter).GetMethod(nameof(Counter.HasParity));
            var methodResolver = new MethodResolver(method, null, new [] { new QueryArgumentParameterInfo(method.GetParameters()[0]) });

            var counter = new Counter { Value = 6 };
            var context = new ResolveFieldContext
            {
                Source = counter,
                Arguments = new Dictionary<string, Execution.ArgumentValue> { { "parity", new Execution.ArgumentValue(Parity.Even, Execution.ArgumentSource.Literal) } }
            };

            // Act
            var result = (bool)methodResolver.Resolve(context);

            // Assert
            result.ShouldBe(true);
        }
    }
}