namespace CopierKata
{
	public interface ISource
	{
		char ReadChar();
		char[] ReadChars(int count);
	}
}