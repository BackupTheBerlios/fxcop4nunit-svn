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

namespace Futureware.FxCop.NUnit
{
	public sealed class TestFixturesShouldHavePublicDefaultConstructorRule : BaseNUnitRule
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TestFixturesShouldHavePublicDefaultConstructorRule() : 
			base( "TestFixturesShouldHavePublicDefaultConstructor" )
		{
		}

		public override ProblemCollection Check(TypeNode type)
		{
			Type runtimeType = type.GetRuntimeType();
			if ( IsTestFixture( runtimeType ) && !runtimeType.IsAbstract )
			{
				MemberList constructors = type.GetConstructors();

				for ( int i = 0; i < constructors.Length; ++i )
				{
					Member constructor = constructors[i];

					// only examine non-static constructors
					Microsoft.Cci.InstanceInitializer instanceConstructor = 
							constructor as Microsoft.Cci.InstanceInitializer;
					
					if ( instanceConstructor == null )
						continue;

					// TODO: we probably should only check the default constructor
					if ( !constructor.IsVisibleOutsideAssembly )
					{
						Resolution resolution = GetResolution( runtimeType.Name );
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
