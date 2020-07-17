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

		private void expectChar(char ch)
		{
			if (source[position] != ch) 
			{
				throw new Exception("Expected " + ch);
			}
		}

		public IDictionary parse() 
		{
			this.source = source;
			this.expectChar('{');

			return map;
		}
	}
}