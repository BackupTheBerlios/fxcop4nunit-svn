#region Copyright © 2005 Victor Boctor
//
// Futureware.FxCop.NUnit is copyrighted to Victor Boctor
//
// This program is distributed under the terms and conditions of the GPL
// See LICENSE file for details.
//
// For commercial applications to link with or modify MantisConnect, they require the
// purchase of a MantisConnect commerical license.
//
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

using Microsoft.Cci;
using Microsoft.Tools.FxCop.Sdk;
using Microsoft.Tools.FxCop.Sdk.Introspection;

using NUnit.Core;
using NUnit.Framework;

namespace Futureware.FxCop.NUnit
{
	/// <summary>
	/// Base class for all FxCop rules related to NUnit.
	/// </summary>
	public abstract class BaseNUnitRule : BaseIntrospectionRule
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ruleName">Name of rule to use for lookup into the RuleDefinitions.xml.</param>
		protected BaseNUnitRule( string ruleName ) : 
			base( ruleName,
			"Futureware.FxCop.NUnit.RuleDefinitions", 
			typeof( BaseNUnitRule ).Assembly )
		{
		}

		protected static bool IsTestFixture( Type type )
		{
            if ( type == null )
                return false;

			return Reflect.HasTestFixtureAttribute( type );
		}

		protected static int GetSetupMethodsCount( Type type )
		{
			MethodInfo[] methods = type.GetMethods();

			int setupMethodsCount = 0;

			foreach ( MethodInfo methodInfo in methods )
			{
				// Check for [TestFixtureSetUp]
				if ( methodInfo.IsDefined( typeof( TestFixtureSetUpAttribute ), false ) )
				{
					setupMethodsCount++;
					continue;
				}

				// Check for [SetUp]
				if ( methodInfo.IsDefined( typeof( SetUpAttribute ), false ) )
					setupMethodsCount++;
			}

			return setupMethodsCount;
		}

		protected static int GetTearDownMethodsCount( Type type )
		{
			MethodInfo[] methods = type.GetMethods();

			int setupMethodsCount = 0;

			foreach ( MethodInfo methodInfo in methods )
			{
				// Check for [TestFixtureSetUp]
				if ( methodInfo.IsDefined( typeof( TestFixtureTearDownAttribute ), false ) )
				{
					setupMethodsCount++;
					continue;
				}

				// Check for [SetUp]
				if ( methodInfo.IsDefined( typeof( TearDownAttribute ), false ) )
					setupMethodsCount++;
			}

			return setupMethodsCount;
		}

		protected static bool IsTestCaseMethod( MethodInfo methodInfo )
		{
			return ( methodInfo.Name.ToLower( CultureInfo.InvariantCulture ).StartsWith( "test" ) ||
				     Reflect.HasTestAttribute( methodInfo ) );
		}

		protected static bool IsSetupMethod( MethodInfo methodInfo )
		{
			return Reflect.GetSetUpMethod( methodInfo.DeclaringType ) == methodInfo;
		}

		protected static bool IsTearDownMethod( MethodInfo methodInfo )
		{
			return Reflect.GetTearDownMethod( methodInfo.DeclaringType ) == methodInfo;
		}
	}
}
