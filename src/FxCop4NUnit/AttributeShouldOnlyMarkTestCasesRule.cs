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

using NUnit.Core;

namespace Futureware.FxCop.NUnit
{
    public sealed class AttributeShouldOnlyMarkTestCasesRule : BaseNUnitRule
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AttributeShouldOnlyMarkTestCasesRule() : 
            base( "AttributeShouldOnlyMarkTestCases" )
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
            Type runtimeType = type.GetRuntimeType();

            if ( IsTestFixture( runtimeType ) )
            {
                System.Reflection.MethodInfo[] methods = runtimeType.GetMethods( BindingFlags.Instance | BindingFlags.Public );

                foreach( MethodInfo method in methods )
                {
                    if ( !Reflect.HasTestAttribute( method ) &&
                         ( Reflect.HasExpectedExceptionAttribute( method ) ||
                           Reflect.HasIgnoreAttribute( method ) ||
                           Reflect.HasCategoryAttribute( method ) ||
                           Reflect.HasExplicitAttribute( method ) 
                         )
                       )
                    {
                        Resolution resolution = GetResolution( method.Name );
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
