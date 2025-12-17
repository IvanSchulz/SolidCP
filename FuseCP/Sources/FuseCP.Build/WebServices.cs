// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Collections.Immutable;

namespace FuseCP.Build
{
	public static class INamedTypeSymbolExtensions
	{
		public static IEnumerable<string> GetNamespaces(INamedTypeSymbol symbol)
		{
			var current = symbol.ContainingNamespace;
			while (current != null)
			{
				if (current.IsGlobalNamespace)
					break;
				yield return current.Name;
				current = current.ContainingNamespace;
			}
		}

		public static string GetFullNamespace(this INamedTypeSymbol symbol)
		{
			return string.Join(".", GetNamespaces(symbol).Reverse());
		}

		public static string GetFullTypeName(this INamedTypeSymbol symbol)
		{
			return string.Join(".", GetNamespaces(symbol).Reverse().Concat(new[] { symbol.Name }));
		}

		public static MethodDeclarationSyntax[] WebMethods(this ClassDeclarationSyntax classDeclaration, SemanticModel model)
		{
			return classDeclaration.Members.OfType<MethodDeclarationSyntax>()
						.Where(m => m.AttributeLists
							.Any(l => l.Attributes
							.Any(a => ((INamedTypeSymbol)model.GetTypeInfo(a).Type).GetFullTypeName() == "FuseCP.Web.Services.WebMethodAttribute")))
						.ToArray();

		}

		public static TypeSyntax Globalized(this TypeSyntax type, SemanticModel model)
		{
			if (type is PredefinedTypeSyntax) return type;
			if (type is ArrayTypeSyntax)
			{
				var array = (ArrayTypeSyntax)type;
				return ArrayType(array.ElementType.Globalized(model), array.RankSpecifiers);
			}
			else if (type is GenericNameSyntax)
			{
				var generic = (GenericNameSyntax)type;
				var typeName = ((INamedTypeSymbol)model.GetTypeInfo(type).Type).GetFullTypeName();
				if (typeName.StartsWith("System.Collections.Generic.List")) // change List to array
					return ArrayType(generic.TypeArgumentList.Arguments.FirstOrDefault().Globalized(model))
						.WithRankSpecifiers(SingletonList<ArrayRankSpecifierSyntax>(
							ArrayRankSpecifier(
								SingletonSeparatedList<ExpressionSyntax>(OmittedArraySizeExpression()))))
						.WithTrailingTrivia(TriviaList(Comment("/*List*/")));
				else return GenericName(Identifier(typeName),
					TypeArgumentList(SeparatedList(generic.TypeArgumentList.Arguments
						.Select(arg => arg.Globalized(model)))));
			}
			else if (type is NullableTypeSyntax) return NullableType(((NullableTypeSyntax)type).ElementType.Globalized(model));
			return ParseTypeName(((INamedTypeSymbol)model.GetTypeInfo(type).Type).GetFullTypeName());
		}

		/*public static SyntaxList<AttributeListSyntax> GlobalizedSoapHeader(this SyntaxList<AttributeListSyntax> attributes, SemanticModel model)
		{
			return List(AttributeList(SeparatedList<AttributeSyntax>(attributes.Attributes
				.Select(at => Attribute(ParseName(((INamedTypeSymbol)model.GetTypeInfo(at.Name).Type).GetFullTypeName()))
					.WithArgumentList(at.ArgumentList))
				.Where(at => at.Name == "FuseCP.Providers.SoapHeaderAttribute")));
		}*/
		public static MethodDeclarationSyntax[] GlobalizedWebMethods(this ClassDeclarationSyntax classDeclaration, IEnumerable<MethodDeclarationSyntax> methods, SemanticModel model)
		{
			var globalizedMethods = methods
				.Select(m => {
					var methodDecl = MethodDeclaration(m.ReturnType.Globalized(model), m.Identifier)
						.WithModifiers(m.Modifiers)
						.WithParameterList(ParameterList(SeparatedList<ParameterSyntax>(
							m.ParameterList.Parameters
								.Select(p => Parameter(p.AttributeLists, p.Modifiers, p.Type.Globalized(model), p.Identifier, p.Default)))));
					var soapHeaderAttribute = m.AttributeLists
						.SelectMany(at => at.Attributes)
						.FirstOrDefault(at => Regex.IsMatch(at.Name.ToString(), "(?:(?:FuseCP.)?Providers.)?SoapHeader(?:Attribute)?"));
					if (soapHeaderAttribute != null)
					{
						methodDecl = methodDecl
							.WithAttributeLists(SingletonList(AttributeList(SingletonSeparatedList(Attribute(IdentifierName("FuseCP.Providers.SoapHeader"),
								soapHeaderAttribute.ArgumentList)))));
					}
					return methodDecl;
				})
				.ToArray();
			return globalizedMethods;
		}
	}

	[Generator(LanguageNames.CSharp)]
	public class WebServices : IIncrementalGenerator
	{

		public const string WebServiceAttributeName = "FuseCP.Web.Services.WebServiceAttribute";
		public const bool Debug = false; // Set to true to debug FuseCP.Build
		public const bool EmitOpenApiTypes = false;
        public const bool EmitSwaggerWcfSecurity = true;

        public static readonly string NewLine = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\r\n" : "\n";

		public static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes, SourceProductionContext context)
		{

#if DEBUG
			if (Debug && !Debugger.IsAttached) Debugger.Launch();
#endif

			var classesWithModel = classes
				.Select(c => new
				{
					Model = compilation.GetSemanticModel(c.SyntaxTree),
					Class = c
				})
				.ToList();

			var wcfTtypes = new List<QualifiedNameSyntax>();

			foreach (var ws in classesWithModel)
			{
				var tree = ws.Class.SyntaxTree;
				var oldTree = tree.GetRoot() as CompilationUnitSyntax;
				CompilationUnitSyntax serverTree;
				CompilationUnitSyntax clientTree;
				var methods = ws.Class.WebMethods(ws.Model);
				var globalizedMethods = ws.Class.GlobalizedWebMethods(methods, ws.Model);

				var hasSoapHeaders = methods
					.SelectMany(m => m.AttributeLists)
					.SelectMany(at => at.Attributes)
					.Any(at => at.Name.ToString() == "SoapHeader" || at.Name.ToString() == "SoapHeaderAttribute");

				var attr = ws.Class.AttributeLists
					.SelectMany(l => l.Attributes)
					.FirstOrDefault(a =>
						((INamedTypeSymbol)ws.Model.GetTypeInfo(a).Type).GetFullTypeName() == "FuseCP.Web.Services.WebServiceAttribute");

				var parent = ws.Class.Parent;
				while (parent != null && !(parent is BaseNamespaceDeclarationSyntax)) parent = parent.Parent;
				BaseNamespaceDeclarationSyntax serverNS, clientNS, oldNS;
				if (parent == null)
				{
					oldNS = null;
					serverNS = NamespaceDeclaration(IdentifierName("FuseCP.Services"));
					clientNS = NamespaceDeclaration(IdentifierName("FuseCP.Client"));

					serverTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(
							UsingDirective(ParseName("System.ServiceModel"))
								.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NETFRAMEWORK"), true, true, true))),
							UsingDirective(ParseName("System.ServiceModel.Web")),
							UsingDirective(ParseName("SwaggerWcf.Attributes"))
								.WithTrailingTrivia(Trivia(ElseDirectiveTrivia(true, true))),
							UsingDirective(ParseName("CoreWCF")),
							UsingDirective(ParseName("CoreWCF.Web")),
							UsingDirective(ParseName("CoreWCF.OpenApi.Attributes"))
								 .WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true))));


					clientTree = CompilationUnit()
						//.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						//.AddUsings(UsingDirective(ParseName("CoreWCF"))
						//	.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET"), true, true, true)))
						//	.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true))))
						.AddUsings(
							UsingDirective(ParseName("System.Linq")),
		                    UsingDirective(ParseName("System.ServiceModel")));
							//.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NET48"), true, true, true)))
							//.WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true)))); ;

				}
				else
				{
					oldNS = (BaseNamespaceDeclarationSyntax)parent;

					serverNS = NamespaceDeclaration(
						QualifiedName(oldNS.Name, IdentifierName("Services")))
						.WithUsings(oldNS.Usings)
						.WithExterns(oldNS.Externs);
					clientNS = NamespaceDeclaration(
						QualifiedName(oldNS.Name, IdentifierName("Client")))
						.WithUsings(oldNS.Usings)
						.WithExterns(oldNS.Externs);

					serverTree = CompilationUnit()
						.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
						.AddUsings(UsingDirective(oldNS.Name))
						.AddUsings(
							UsingDirective(ParseName("System.ServiceModel"))
								.WithLeadingTrivia(Trivia(IfDirectiveTrivia(IdentifierName("NETFRAMEWORK"), true, true, true))),
							UsingDirective(ParseName("System.ServiceModel.Web")),
							UsingDirective(ParseName("SwaggerWcf.Attributes"))
								.WithTrailingTrivia(Trivia(ElseDirectiveTrivia(true, true))),
							UsingDirective(ParseName("CoreWCF")),
							UsingDirective(ParseName("CoreWCF.Web")),
							UsingDirective(ParseName("CoreWCF.OpenApi.Attributes"))
								 .WithTrailingTrivia(Trivia(EndIfDirectiveTrivia(true)))
							);
							//UsingDirective(ParseName("System.ServiceModel.Activation")));

					clientTree = CompilationUnit()
                        //.WithUsings(((CompilationUnitSyntax)oldTree).Usings)
                        //.AddUsings(UsingDirective(oldNS.Name))
                        .AddUsings(
                            UsingDirective(ParseName("System.Linq")),
                            UsingDirective(ParseName("System.ServiceModel")));

                }

                var webServiceNamespace = attr.ArgumentList.Arguments
					.Where(a => a.NameEquals != null && a.NameEquals.Name.ToString() == "Namespace" && a.Expression is LiteralExpressionSyntax)
					.Select(a => (string)((LiteralExpressionSyntax)a.Expression).Token.Value)
					.FirstOrDefault() ?? "http://tempuri.org/";
				if (!webServiceNamespace.EndsWith("/")) webServiceNamespace = $"{webServiceNamespace}/";

				// wcf service contract interface
				var intf = ParseMemberDeclaration(
					new ServiceInterface()
					{
						WebServiceNamespace = webServiceNamespace,
						Class = ws.Class,
						WebMethods = methods,
						Model = ws.Model
					}
					.Render())
					.NormalizeWhitespace();

				//wcf service class
				var service = ParseMemberDeclaration(
					new ServiceClass()
					{
						OldNamespace = oldNS.Name.ToString(),
						Class = ws.Class,
						WebMethods = methods
					}
					.Render())
					.NormalizeWhitespace();
				
				serverNS = serverNS
					.WithMembers(List(new MemberDeclarationSyntax[]
						{
							intf, service
						}));

				wcfTtypes.Add(QualifiedName(serverNS.Name, IdentifierName(((ClassDeclarationSyntax)service).Identifier)));

				var client = ParseMemberDeclaration(
					new ClientClass()
					{
						Class = ws.Class,
						WebMethods = globalizedMethods
					}
					.Render())
					.NormalizeWhitespace();


				var policy = ws.Class.AttributeLists
					.SelectMany(a => a.Attributes)
					.FirstOrDefault(a => 
						((INamedTypeSymbol)ws.Model.GetTypeInfo(a).Type).GetFullTypeName() == "FuseCP.Web.Services.PolicyAttribute");

				AttributeListSyntax hasPolicy = null;

				if (policy != null)
				{
					hasPolicy = AttributeList(SingletonSeparatedList(Attribute(ParseName("FuseCP.Web.Clients.HasPolicy"))
						.WithArgumentList(policy.ArgumentList)));
				}

				var clientIntf = ParseMemberDeclaration(
					new ClientInterface()
					{
						HasSoapHeader = hasSoapHeaders,
						HasPolicyAttribute = hasPolicy,
						WebServiceNamespace = webServiceNamespace,
						Class = ws.Class,
						WebMethods = globalizedMethods
					}
					.Render())
					.NormalizeWhitespace();

				var clientAssemblyClass = ParseMemberDeclaration(
					new ClientAssemblyClass()
					{
						OldNamespace = oldNS.Name.ToString(),
						Class = ws.Class,
						WebMethods = globalizedMethods
					}
					.Render())
					.NormalizeWhitespace();

				clientNS = clientNS
					.WithMembers(List(new MemberDeclarationSyntax[] {
						clientIntf, clientAssemblyClass, client
					}));


				serverTree = serverTree
					.AddMembers(serverNS)
					.NormalizeWhitespace();

				clientTree = clientTree
					.AddMembers(clientNS)
					.NormalizeWhitespace();

				var serverText = serverTree.ToString();
				var clientText = clientTree.ToString();
				serverText = Regex.Replace(serverText, "#(?:end)?region.*?(?=\\r?\\n)", "", RegexOptions.Singleline);
				clientText = Regex.Replace(clientText, "#(?:end)?region.*?(?=\\r?\\n)", "", RegexOptions.Singleline);

				var typeName = ws.Class.Identifier.Text;

				context.AddSource($"{typeName}.g", $"#if !Client{NewLine}{serverText}{NewLine}#endif");
				context.AddSource($"{typeName}Client.g", $"#if Client{NewLine}{clientText}{NewLine}#endif");
			}

			var typesText = ParseCompilationUnit(new WCFServiceTypes()
			{
				Types = wcfTtypes
			}
			.Render())
			.NormalizeWhitespace()
			.ToString();

			context.AddSource("WCFServiceTypes.g", $"#if !Client{NewLine}{typesText}{NewLine}#endif");
		}

		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
				.CreateSyntaxProvider(
					predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
					transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
				.Where(static m => m is not null);

			static bool IsSyntaxTargetForGeneration(SyntaxNode node)
				=> node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0;

			static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
			{
				// we know the node is a MethodDeclarationSyntax thanks to IsSyntaxTargetForGeneration
				var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

				// loop through all the attributes on the method
				foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
				{
					foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
					{
						IMethodSymbol attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;
						if (attributeSymbol == null)
						{
							// weird, we couldn't get the symbol, ignore it
							continue;
						}

						INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
						string fullName = attributeContainingTypeSymbol.ToDisplayString();

						// Is the attribute the [WebService] attribute?
						if (fullName == WebServiceAttributeName)
						{
							// return the parent class of the method
							return classDeclarationSyntax;
						}
					}
				}

				// we didn't find the attribute we were looking for
				return null;
			}

			IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
				= context.CompilationProvider.Combine(classDeclarations.Collect());

			context.RegisterSourceOutput(compilationAndClasses,
				static (spc, source) => Execute(source.Item1, source.Item2, spc));
		}
	}
}
