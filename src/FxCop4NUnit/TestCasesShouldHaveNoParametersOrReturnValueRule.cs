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

using Microsoft.Cci;
using Microsoft.Tools.FxCop.Sdk;
using Microsoft.Tools.FxCop.Sdk.Introspection;

using NUnit.Core;
using NUnit.Framework;

namespace Futureware.FxCop.NUnit
{
	public sealed class TestCasesShouldHaveNoParametersOrReturnValueRule : BaseNUnitRule
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TestCasesShouldHaveNoParametersOrReturnValueRule() : 
			base( "TestCasesShouldHaveNoParametersOrReturnValue" )
		{
		}

		public override ProblemCollection Check(TypeNode type)
		{
			Type runtimeType = type.GetRuntimeType();
			if ( IsTestFixture( runtimeType ) )
			{
				System.Reflection.MethodInfo[] methods = runtimeType.GetMethods();

				foreach( System.Reflection.MethodInfo methodInfo in methods )
				{
					// if method starts with "test" and is not marked as [Test],
					// then an explicit [Test] should be added since NUnit will
					// treat it as a test case.
					if ( IsTestCaseMethod( methodInfo ) )
					{
						if ( methodInfo.GetParameters().Length > 0 )
						{
							Resolution resolution = GetNamedResolution( "Parameters", methodInfo.Name );
							Problem problem = new Problem( resolution );
							base.Problems.Add( problem );
						}

						if ( methodInfo.ReturnType != typeof(void) )
						{
							Resolution resolution = GetNamedResolution( "ReturnType", methodInfo.Name );
							Problem problem = new Problem( resolution );
							base.Problems.Add( problem );
						}
					}
				}

				if ( base.Problems.Count > 0 )
					return base.Problems;
			}

			return base.Check (type);
		}
	}
}
