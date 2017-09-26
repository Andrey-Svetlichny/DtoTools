using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoTools
{
    static class DtoParser
    {
        public static IEnumerable<DtoPropertyInfo> Parse(string dtoSourceCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(dtoSourceCode);
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
            if (classes.Count != 1)
            {
                throw new ArgumentException("classes.Count != 1");
            }

            var props = classes[0].DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();
            foreach (var prop in props)
            {
                var type = prop.Type.ToString();
                var attrs = prop.AttributeLists.SelectMany(a => a.Attributes).ToList();
                var attrArguments = attrs.SelectMany(a => a.ArgumentList.Arguments).ToList();
                yield return new DtoPropertyInfo
                {
                    // assume we have only one attribute like DTOdescription
                    Description = attrArguments.FirstOrDefault()?.ToString().TrimStart('"').TrimEnd('"'),
                    Name = prop.Identifier.Text,
                    Type = type.TrimEnd('?'),
                    IsNullable = type.EndsWith("?")
                };
            }
        }
    }
}
