using Xunit;

namespace GraphQL.Annotations.Tests
{
    public class SchemaBuilderTests
    {
        [Fact] public void SchemaBuilder_Fields_AreIgnored(){}

        [Fact] public void SchemaBuilder_Properties_WithoutAttributeAreIgnored(){}
        [Fact] public void SchemaBuilder_Properties_WithAttributeAreRegistered(){}
        [Fact] public void SchemaBuilder_Properties_AreRegisteredWithCorrectType(){}

        [Fact] public void SchemaBuilder_Methods_WithoutAttributeAreIgnored(){}
        [Fact] public void SchemaBuilder_Methods_WithAttributeAreRegistered(){}
        [Fact] public void SchemaBuilder_Methods_AreRegisteredWithCorrectTypes(){}

        [Fact] public void SchemaBuilder_Methods_SignaturesAreValidated(){}
        [Fact] public void SchemaBuilder_Methods_CanTakeInjectableParameters(){}

        [Fact] public void SchemaBuilder_Methods_CanTakeAdditionalQueryArguments(){}
        [Fact] public void SchemaBuilder_Methods_CanDefinedOptionalArguments(){}
        [Fact] public void SchemaBuilder_Methods_CanDefinedDefaultValueForArguments(){}

    }
}