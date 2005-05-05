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

		protected static int GetFixtureSetupMethodsCount( Type type )
		{
			MethodInfo[] methods = type.GetMethods( BindingFlags.Instance | BindingFlags.Public );

			int setupMethodsCount = 0;

			foreach ( MethodInfo methodInfo in methods )
			{
				// Check for [TestFixtureSetUp]
				if ( methodInfo.IsDefined( typeof( TestFixtureSetUpAttribute ), false ) )
					setupMethodsCount++;
			}

			return setupMethodsCount;
		}

		protected static int GetTestSetupMethodsCount( Type type )
		{
			MethodInfo[] methods = type.GetMethods( BindingFlags.Instance | BindingFlags.Public );

			int setupMethodsCount = 0;

			foreach ( MethodInfo methodInfo in methods )
			{
				// Check for [SetUp]
				if ( methodInfo.IsDefined( typeof( SetUpAttribute ), false ) )
					setupMethodsCount++;
			}

			return setupMethodsCount;
		}

		protected static int GetFixtureTearDownMethodsCount( Type type )
		{
			MethodInfo[] methods = type.GetMethods( BindingFlags.Instance | BindingFlags.Public );

			int setupMethodsCount = 0;

			foreach ( MethodInfo methodInfo in methods )
			{
				// Check for [TestFixtureTearDown]
				if ( methodInfo.IsDefined( typeof( TestFixtureTearDownAttribute ), false ) )
					setupMethodsCount++;
			}

			return setupMethodsCount;
		}

		protected static int GetTestTearDownMethodsCount( Type type )
		{
			MethodInfo[] methods = type.GetMethods( BindingFlags.Instance | BindingFlags.Public );

			int setupMethodsCount = 0;

			foreach ( MethodInfo methodInfo in methods )
			{
				// Check for [TearDown]
				if ( methodInfo.IsDefined( typeof( TearDownAttribute ), false ) )
					setupMethodsCount++;
			}

			return setupMethodsCount;
		}

		protected static bool HasSetUpAttribute( MethodInfo method )
		{
			return method.IsDefined( Reflect.SetUpType, false );
		}

		protected static bool HasTearDownAttribute( MethodInfo method )
		{
			return method.IsDefined( Reflect.TearDownType, false );
		}

		protected static bool HasFixtureSetUpAttribute( MethodInfo method )
		{
			return method.IsDefined( Reflect.FixtureSetUpType, false );
		}

		protected static bool HasFixtureTearDownAttribute( MethodInfo method )
		{
			return method.IsDefined( Reflect.FixtureTearDownType, false );
		}

		protected static bool IsTestCaseMethod( MethodInfo methodInfo )
		{
			if ( Reflect.HasTestAttribute( methodInfo ) )
				return true;

			if ( methodInfo.Name.ToLower( CultureInfo.InvariantCulture ).StartsWith( "test" ) )
			{
				if ( !HasSetUpAttribute( methodInfo ) &&
					 !HasTearDownAttribute( methodInfo ) &&
					 !HasFixtureSetUpAttribute( methodInfo ) &&
					 !HasFixtureTearDownAttribute( methodInfo ) )
					return true;
			}

			return false;
		}

		protected static bool IsFixtureSetupMethod( MethodInfo methodInfo )
		{
			return Reflect.GetFixtureSetUpMethod( methodInfo.DeclaringType ) == methodInfo;
		}

		protected static bool IsFixtureTearDownMethod( MethodInfo methodInfo )
		{
			return Reflect.GetFixtureTearDownMethod( methodInfo.DeclaringType ) == methodInfo;
		}

		protected static bool IsTestSetupMethod( MethodInfo methodInfo )
		{
			return Reflect.GetSetUpMethod( methodInfo.DeclaringType ) == methodInfo;
		}

		protected static bool IsTestTearDownMethod( MethodInfo methodInfo )
		{
			return Reflect.GetTearDownMethod( methodInfo.DeclaringType ) == methodInfo;
		}

		protected static bool IsNUnitMethod( MethodInfo methodInfo )
		{
			return ( IsTestCaseMethod( methodInfo ) ||
					 IsFixtureSetupMethod( methodInfo ) ||
					 IsFixtureTearDownMethod( methodInfo ) ||
					 IsTestSetupMethod( methodInfo ) ||
					 IsTestTearDownMethod( methodInfo ) );
		}
	}
}
