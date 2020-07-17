using System;
using System.Collections;

namespace HackerNews
{
	/// <summary>
	/// Summary description for Json.
	/// </summary>
	public class Json
	{
		private String source = null;
		private int position = 0;
		private IDictionary map = new Hashtable();

		public static void run()
		{
			String test = "{ \"test\": 1 }";
			Json json = new Json(test);
			json.parse();
		}

		public Json(String source) 
		{
			this.source = source;
		}

		private void consume(char ch) 
		{
			expectChar(ch);
			position++;
		}

		private void expectChar(char ch)
		{
			if (source[position] != ch) 
			{
				throw new Exception("Expected " + ch);
			}
		}

		private boolean charAtCurrentPosition(char ch)
		{
			return source[position] == ch;
		}

		private String consumeUntil(char ch) 
		{
		}

		private void parseObject() 
		{
			String key = null;
			int value = 0;
			consume('"');

		}

		public IDictionary parse() 
		{
			if (charAtCurrentPosition('{')) 
			{
				consume('{');
				parseObject();
			}

			return map;
		}
	}
}