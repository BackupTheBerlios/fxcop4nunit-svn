#region Copyright � 2005 Victor Boctor
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

using NUnit.Framework;

namespace Futureware.FxCop.NUnit
{
	public sealed class TestFixturesShouldHaveSingleSetupMethodRule : BaseNUnitRule
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TestFixturesShouldHaveSingleSetupMethodRule() : 
			base( "TestFixturesShouldHaveSingleSetupMethod" )
		{
		}

		public override ProblemCollection Check(TypeNode type)
		{
			Type runtimeType = type.GetRuntimeType();
			if ( IsTestFixture( runtimeType ) )
			{
				// if more than one setup, trigger error.
				if ( GetSetupMethodsCount( runtimeType ) > 1 )
				{
					Resolution resolution = GetResolution( type.Name.Name );
					Problem problem = new Problem( resolution );
					base.Problems.Add( problem );
					return base.Problems;
				}
			}

			return base.Check (type);
		}
	}
}
