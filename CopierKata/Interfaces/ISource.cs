namespace CopierKata.Interfaces
{
	public interface ISource
	{
		char ReadChar();
		char[] ReadChars(int count);
	}
}