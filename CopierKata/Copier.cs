using System.Collections.Generic;
using System.Linq;
using CopierKata.Interfaces;

namespace CopierKata
{
	public class Copier
	{
		private readonly ISource _source;
		private readonly IDestination _destination;

		public Copier(ISource source, IDestination destination)
		{
			_source = source;
			_destination = destination;
		}

		public void Copy()
		{
			var character = _source.ReadChar();
			if (AbleToCopy(character))
			{
				_destination.WriteChar(character);
				//Copy();
			}
		}

		public void CopyMultiple(int numberOfCharactersToCopy)
		{
			var characters = _source.ReadChars(numberOfCharactersToCopy);
			if (!AbleToCopy(characters)) return;
			var charactersToCopy = characters.TakeWhile(character => character != '\n').ToArray();
			_destination.WriteChars(charactersToCopy);

			if (characters.Contains('\n'))
				return;

			//CopyMultiple(numberOfCharactersToCopy);
		}

		private static bool AbleToCopy(IReadOnlyCollection<char> characters)
		{
			return characters != null && characters.Count != 0 && !CharacterIsNewLine(characters.First());
		}

		private static bool AbleToCopy(char character)
		{
			return !CharacterIsEmpty(character) && !CharacterIsNewLine(character);
		}

		private static bool CharacterIsNewLine(char character)
		{
			return character == '\n';
		}

		private static bool CharacterIsEmpty(char character)
		{
			return character == 0;
		}
	}
}
