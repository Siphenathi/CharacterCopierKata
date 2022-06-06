using CopierKata.Interfaces;
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

		[TestCase(new[] { '\n' })]
		[TestCase(new[] { '\n', 'd' })]
		[TestCase(new[] { '\n', 'f', 's', 'e', 'r' })]
		public void CopyMultiple_GivenSourceStartingWithANewLine_ShouldNotCallDestination(char[] sourceResults)
		{
			//Arrange
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();
			var copier = CreateCopier(source, destination);

			source.ReadChars(2).Returns(sourceResults);

			//Act
			copier.CopyMultiple(2);

			//Assert
			source.Received(1).ReadChars(2);
			destination.DidNotReceive().WriteChars(Arg.Any<char[]>());
		}


		[TestCase(new[] { 'r', 'd', '\n', 'e' }, 4, new[] { 'r', 'd' })]
		[TestCase(new[] { 'l', 'o', 'y', '\n', 'f', 's', 'e' }, 7, new[] { 'l', 'o', 'y' })]
		[TestCase(new[] { 'r', 'd', 'n', 'x', 'a', '\n', 'd', 'm', 'z' }, 9, new[] { 'r', 'd', 'n', 'x', 'a' })]
		public void CopyMultiple_GivenSourceContainingNewLine_ShouldWriteCharactersBeforeNewLineToDestination(char[] sourceResults, int count, char[] CharactersToCopy)
		{
			//Arrange
			var source = Substitute.For<ISource>();
			var destination = Substitute.For<IDestination>();
			var sut = CreateCopier(source, destination);

			source.ReadChars(count).Returns(sourceResults);

			//Act
			sut.CopyMultiple(count);

			//Assert
			source.Received(1).ReadChars(count);
			destination.Received(1).WriteChars(Arg.Do<char[]>(receivedCharacters => CharactersToCopy = receivedCharacters));
		}

		private static Copier CreateCopier(ISource source, IDestination destination)
		{
			return new Copier(source, destination);
		}
	}
}
