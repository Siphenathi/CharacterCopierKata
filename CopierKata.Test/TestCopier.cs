using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CopierKata.Test
{
	[TestFixture]
	public class TestCopier
	{
		[Test]
		public void Copy_WhenCalledNewLine_ShouldNotCopyTheCharacterToDestination()
		{
			//Arrange
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();

			source.ReadChar().Returns('\0');

			var sut = CreateCopier(source, destination);

			//Act
			sut.Copy();

			//Assert
			destination.DidNotReceive().WriteChar(Arg.Any<char>());
		}

		[Test]
		public void Copy_WhenCalledEmptyCharacter_ShouldNotCopyTheCharacterToDestination()
		{
			//Arrange
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();

			source.ReadChar().Returns('\n');

			var sut = CreateCopier(source, destination);

			//Act
			sut.Copy();

			//Assert
			destination.DidNotReceive().WriteChar(Arg.Any<char>());
		}

		[Test]
		public void Copy_WhenCalled_ShouldCopySingleCharacterToDestination()
		{
			//Arrange
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();

			source.ReadChar().Returns('I');

			var sut = CreateCopier(source, destination);

			//Act
			sut.Copy();

			//Assert
			destination.Received(1).WriteChar(Arg.Is('I'));
		}

		[Test]
		public void CopyMultiple_WhenCalledWithCharacterCountAndEmptyCharacters_ShouldNotCopyCharactersToDestination()
		{
			//Arrange
			const int count = 3;
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();

			source.ReadChars(count).Returns(new char[]{});

			var sut = CreateCopier(source, destination);

			//Act
			sut.CopyMultiple(count);

			//Assert
			destination.DidNotReceive().WriteChars(Arg.Any<char[]>());
		}

		[Test]
		public void Copy_WhenCalled_ShouldCopyMultipleCharacterToDestination()
		{
			//Arrange
			const int numberOfCharactersToCopy = 4;
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();
			var expectedValues = new[]{ 'I', ' ', 'l', 'o', 'v', 'e' };

			source.ReadChars(numberOfCharactersToCopy).Returns(expectedValues);

			var sut = CreateCopier(source, destination);

			//Act
			sut.CopyMultiple(numberOfCharactersToCopy);

			//Assert
			destination.Received(1).WriteChars(Arg.Is(expectedValues));
		}

		private static Copier CreateCopier(ISource source, IDestination destination)
		{
			return new Copier(source, destination);
		}
	}
}
