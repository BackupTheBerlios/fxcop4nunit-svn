#region Copyright © 2005 Victor Boctor
//
// Futureware.FxCop.NUnit is copyrighted to Victor Boctor
//
// This program is distributed under the terms and conditions of the GPL
// See LICENSE file for details.
//
// For commercial applications to link with or modify MantisConnect, they require the
// purchase of a MantisConnect commerical license.7
//
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

using Microsoft.Cci;
using Microsoft.Tools.FxCop.Sdk;
using Microsoft.Tools.FxCop.Sdk.Introspection;

namespace Futureware.FxCop.NUnit
{
	public sealed class TestFixturesShouldNotContainPublicPropertiesRule : BaseNUnitRule
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TestFixturesShouldNotContainPublicPropertiesRule() : 
			base( "TestFixturesShouldNotContainPublicProperties" )
		{
		}

		public override ProblemCollection Check(TypeNode type)
		{
			Type runtimeType = type.GetRuntimeType();
			if ( IsTestFixture( runtimeType ) )
			{
				PropertyInfo[] properties;

				// check for instance public properties
				properties = runtimeType.GetProperties( BindingFlags.Instance | BindingFlags.Public );
				foreach( PropertyInfo instanceProperty in properties )
				{
					Resolution resolution = GetResolution( instanceProperty.DeclaringType.Name, instanceProperty.Name );
					Problem problem = new Problem( resolution );
					base.Problems.Add( problem );
				}

				// check for static public properties, whether declared in the class
				// or one of its base classes.
				properties = runtimeType.GetProperties( BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy );
				foreach( PropertyInfo staticProperty in properties )
				{
					Resolution resolution = GetResolution( staticProperty.DeclaringType.Name, staticProperty.Name );
					Problem problem = new Problem( resolution );
					base.Problems.Add( problem );
				}
			}

			if ( base.Problems.Count > 0 )
				return base.Problems;

			return base.Check (type);
		}
	}
}
