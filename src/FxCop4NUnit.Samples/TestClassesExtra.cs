using System;

using NUnit.Framework;

namespace Futureware.FxCop.NUnit.Sample
{
	/// <summary>
	/// Summary description for TestClassesExtra.
	/// </summary>
	[TestFixture]
	public sealed class UnmarkedTestCaseFixture
	{
		public void TestSomething()
		{
		}
	}

	[TestFixture]
	public sealed class NonNUnitPublicMethod
	{
		[Test]
		public void OneOfMyTests()
		{
		}

		public void StrangePublicMethod()
		{
		}
	}

	[Category("InvalidUseOfCategory")]
	public sealed class NonTestFixtureWithCategory
	{
	}

	[Ignore("reason")]
	public sealed class NonTestFixtureWithIgnore
	{
	}

	[Explicit]
	public sealed class NonTestFixtureWithExplicit
	{
	}

	[TestFixture]
	[Ignore("")]
	public sealed class IgnoreTestFixtureWithEmptyReason
	{
		[Test]
		public void AnotherTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureContainingExplicitWithoutTestAttribute
	{
		[Explicit]
		public void ExplicitTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureContainingExceptionExpectedWithoutTestAttribute
	{
		[ExpectedException(typeof(ArgumentException))]
		public void ExceptionTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureContainingIgnoreWithoutTestAttribute
	{
		[Ignore("reason")]
		public void IgnoreTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureContainingCategoryWithoutTestAttribute
	{
		[Category("category")]
		public void CategoryTestCase()
		{
		}
	}

	[TestFixture]
	[Category("   ")]
	public sealed class TestFixtureWithEmptyCategory
	{
		[Test]
		public void SimpleTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureIncludingTestCaseWithEmptyCategory
	{
		[Test]
		[Category("   ")]
		public void TestCaseWithEmptyCategory()
		{
		}
	}

	[TestFixture]
	[Ignore("   ")]
	public sealed class TestFixtureWithEmptyIgnoreReason
	{
		[Test]
		public void SimpleTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureIncludingTestCaseWithEmptyIgnoreReason
	{
		[Test]
		[Ignore("   ")]
		public void TestCaseWithEmptyIgnoreReason()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureWithPublicProperties
	{
		public bool SampleProperty
		{
			get { return true; }
		}

		[Test]
		public void SimpleTestCase()
		{
		}
	}

	public abstract class BaseTestFixtureWithPublicProperty
	{
		public bool SampleProperty
		{
			get { return true; }
		}
	}

	[TestFixture]
	public sealed class TestFixtureWithInheritedPublicProperty : BaseTestFixtureWithPublicProperty
	{
		[Test]
		public void SimpleTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureWithPrivateConstructor
	{
		private TestFixtureWithPrivateConstructor()
		{
		}

		[Test]
		public void SimpleTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureWithPrivateNonDefaultConstructor
	{
		public TestFixtureWithPrivateNonDefaultConstructor()
		{
		}

		private TestFixtureWithPrivateNonDefaultConstructor( int i )
		{
		}

		[Test]
		public void SimpleTestCase()
		{
		}
	}

	[TestFixture]
	public sealed class TestFixtureWithNoDefaultConstructor
	{
		public TestFixtureWithNoDefaultConstructor( int i )
		{
		}

		[Test]
		public void SimpleTestCase()
		{
		}
	}
}
