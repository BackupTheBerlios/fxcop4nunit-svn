#region Copyright � 2005 Victor Boctor
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
using System.Collections;
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
	public sealed class IgnoreReasonShouldNotBeEmptyRule : BaseNUnitRule
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public IgnoreReasonShouldNotBeEmptyRule() : 
			base( "IgnoreReasonShouldNotBeEmpty" )
		{
		}

		public override ProblemCollection Check(TypeNode type)
		{
			Type runtimeType = type.GetRuntimeType();

			if ( IsTestFixture( runtimeType ) )
			{
				string ignoreReason = Reflect.GetIgnoreReason( runtimeType );
				if ( ignoreReason.Trim().Length == 0 )
				{
					Resolution resolution = GetNamedResolution( "TestFixture", type.Name.Name );
					Problem problem = new Problem( resolution );
					base.Problems.Add( problem );
				}

				System.Reflection.MethodInfo[] methods = runtimeType.GetMethods( BindingFlags.Instance | BindingFlags.Public );
				foreach( MethodInfo method in methods )
				{
					string methodIgnoreReason = Reflect.GetIgnoreReason( method );
					if ( methodIgnoreReason.Trim().Length == 0 )
					{
						string[] parameters = new string[] { type.Name.Name, method.Name };
						Resolution resolution = GetNamedResolution( "TestCase", parameters );
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
