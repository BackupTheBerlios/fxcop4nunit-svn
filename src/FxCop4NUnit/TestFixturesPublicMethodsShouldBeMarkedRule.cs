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
	public sealed class TestFixturesPublicMethodsShouldBeMarkedRule : BaseNUnitRule
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TestFixturesPublicMethodsShouldBeMarkedRule() : 
			base( "TestFixturesPublicMethodsShouldBeMarked" )
		{
		}

		public override ProblemCollection Check(TypeNode type)
		{
			Type runtimeType = type.GetRuntimeType();
			if ( IsTestFixture( runtimeType ) )
			{
				System.Reflection.MethodInfo[] methods = runtimeType.GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly );

				// Note that if an method with an invalid signature is marked as a setup method, then
				// it will be considered as not marked and hence setupMethod will be null.
				// Same applies for tearDownMethod.
				System.Reflection.MethodInfo setupMethod = Reflect.GetSetUpMethod( runtimeType );
				System.Reflection.MethodInfo tearDownMethod = Reflect.GetTearDownMethod( runtimeType );

				foreach( System.Reflection.MethodInfo methodInfo in methods )
				{
					if ( !IsTestCaseMethod( methodInfo ) && 
						 ( methodInfo != setupMethod ) && 
						 ( methodInfo != tearDownMethod ) )
					{
						Resolution resolution = GetResolution( methodInfo.Name );
						Problem problem = new Problem( resolution );
						base.Problems.Add( problem );
					}
				}

				if ( base.Problems.Count > 0 )
					return base.Problems;
			}

			return base.Check (type);
		}
	}
}
