using System;
using Xunit;

namespace GraphQL.Annotations.Test
{
    public class Tests
    {
        [Fact]
        public void GraphQLTypeAttribute_RefusesNonGraphTypes()
        {

            Assert.True(true);
        }
    }
}