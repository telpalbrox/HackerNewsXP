using System;
using System.Collections;

namespace HackerNews
{
	public enum JsonType 
	{
		Number,
		String,
		Object,
		Array,
		Boolean,
		Null
	}

	public class JsonValue 
	{
		public readonly double Number;
		public readonly String String;
		public readonly IDictionary Object;
		public readonly IList Array;
		public readonly bool Boolean;
		public readonly bool IsNull;
		public readonly JsonType Type;

		public static JsonValue NullValue()
		{
			return new JsonValue(JsonType.Null);
		}

		public JsonValue(double number) 
		{
			Number = number;
			Type = JsonType.Number;
		}

		public JsonValue(bool boolean) 
		{
			Boolean = boolean;
			Type = JsonType.Boolean;
		}

		public JsonValue(IDictionary obj)
		{
			Object = obj;
			Type = JsonType.Object;
		}

		public JsonValue(String str)
		{
			String = str;
			Type = JsonType.String;
		}

		public JsonValue(IList array)
		{
			Array = array;
			Type = JsonType.Array;
		}

		private JsonValue(JsonType type) 
		{
			Type = type;
		}

		public bool True 
		{
			get 		
			{
				return Type == JsonType.Boolean && Boolean;
			}
		}

		public bool False 
		{
			get 		
			{
				return Type == JsonType.Boolean && !Boolean;
			}
		}

		public bool Null 
		{
			get 		
			{
				return Type == JsonType.Null;
			}
		}

		public bool IsObject
		{
			get
			{
				return Type == JsonType.Object;
			}
		}

		public static explicit operator double(JsonValue number)
		{
			return number.Number;
		}
	}
	/// <summary>
	/// Summary description for Json.
	/// </summary>
	public class Json
	{
		private String input = null;
		private int index = 0;
		private IDictionary map = new Hashtable();

		private static void assert(bool assertion)
		{
			if (!assertion)
			{
				throw new Exception("Assertion is not true");
			}
		}

		public static void run()
		{
			String test1 = "null";
			Json json1 = new Json(test1);
			JsonValue result1 = json1.parse();
			assert(result1.Null);

			String test2 = "true";
			Json json2 = new Json(test2);
			JsonValue result2 = json2.parse();
			assert(result2.True);

			String test3 = "false";
			Json json3 = new Json(test3);
			JsonValue result3 = json3.parse();
			assert(result3.False);

			String test4 = "{}";
			Json json4 = new Json(test4);
			JsonValue result4 = json4.parse();
			assert(result4.IsObject);
			IDictionary obj4 = result4.Object;
			assert(obj4.Keys.Count == 0);

			String test5 = "{ \"test\": true }";
			Json json5 = new Json(test5);
			JsonValue result5 = json5.parse();
			assert(result5.IsObject);
			IDictionary obj5 = result5.Object;
			assert(obj5.Keys.Count == 1);
			JsonValue value5 = (JsonValue)obj5["test"];
			assert(value5.True);

			String test6 = "\"lol\"";
			Json json6 = new Json(test6);
			JsonValue result6 = json6.parse();
			assert(result6.String == "lol");

			String test7 = "27";
			Json json7 = new Json(test7);
			JsonValue result7 = json7.parse();
			assert(result7.Number == 27);

			String test8 = "27.42";
			Json json8 = new Json(test8);
			JsonValue result8 = json8.parse();
			assert(result8.Number == 27.42);

			String test9 = "[1, \"dos\", null]";
			Json json9 = new Json(test9);
			JsonValue result9 = json9.parse();
			assert(result9.Array.Count == 3);
		}

		public Json(String input) 
		{
			this.input = input;
		}

		private bool isWhitespace(char ch)
		{
			return ch == ' ' || ch == '\n' || ch == '\t' || ch == '\v' || ch == '\r';
		}

		private void consumeSpecific(char ch) 
		{
			expectChar(ch);
			index++;
		}

		private void expectChar(char ch)
		{
			if (input[index] != ch) 
			{
				throw new Exception("Expected " + ch);
			}
		}

		private char peek()
		{
			if (index >= input.Length) 
			{
				return '\0';
			}
			return input[index];
		}

		private char consume()
		{
			return input[index++];
		}
		
		private void consumeWhitespace()
		{
			while(isWhitespace(peek()))
			{
				consume();
			}
		}

		private void consumeString(String str) 
		{
			for (int i = 0; i < str.Length; i++) 
			{
				consumeSpecific(str[i]);
			}
		}

		private String consumeQuotedString()
		{
			consumeSpecific('"');

			String finalSb = "";

			for (;;)
			{
				int peekIndex = index;
				char ch = '0';
				for (;;) 
				{
					if (peekIndex == input.Length)
					{
						break;
					}
					ch = input[peekIndex];
					if (ch == '"' || ch == '\\') 
					{
						break;
					}
					peekIndex++;
				}

				if (peekIndex != index)
				{
					while(peekIndex != index)
					{
						finalSb += consume();
					}
				}

				if (index == input.Length)
				{
					break;
				}

				if (ch == '"')
				{
					break;
				}

				if (ch != '\\')
				{
					finalSb += consume();
					continue;
				}

				consumeSpecific('\\');
				char escapedCh = consume();
				switch(escapedCh) 
				{
					case 'n':
						finalSb += '\n';
						break;
					case 'r':
						finalSb += '\r';
						break;
					case 't':
						finalSb += '\t';
						break;
					case 'b':
						finalSb += '\b';
						break;
					case 'f':
						finalSb += '\f';
						break;
					// TODO: Handle 'u'
					default:
						finalSb += escapedCh;
						break;
				}
			}
			consumeSpecific('"');

			return finalSb;
		}

		private JsonValue parseObject() 
		{
			IDictionary obj = new Hashtable();

			consumeSpecific('{');

			for (;;)
			{
				consumeWhitespace();
				if (peek() == '}')
				{
					break;
				}
				consumeWhitespace();
				String name = consumeQuotedString();
				if (name.Length == 0)
				{
					return null;
				}

				consumeWhitespace();
				consumeSpecific(':');
				consumeWhitespace();
				JsonValue val = parseValue();
				if (val == null)
				{
					return null;
				}
				obj.Add(name, val);

				consumeWhitespace();
				if (peek() == '}') 
				{
					break;
				}
				consumeSpecific(',');
				consumeWhitespace();
				if (peek() == '}')
				{
					return null;
				}
			}

			consumeSpecific('}');

			return new JsonValue(obj);
		}

		private JsonValue parseArray()
		{
			consumeSpecific('[');
			IList array = new ArrayList();

			for(;;)
			{
				consumeWhitespace();
				if (peek() == ']')
				{
					break;
				}
				JsonValue element = parseValue();
				if (element == null)
				{
					return null;
				}
				array.Add(element);
				consumeWhitespace();
				if (peek() == ']')
				{
					break;
				}
				consumeSpecific(',');
				if (peek() == ']')
				{
					return null;
				}
			}

			consumeWhitespace();
			consumeSpecific(']');

			return new JsonValue(array);
		}

		private JsonValue parseNumber()
		{
			String numberStr = "";
			String fractionStr = "";
			bool isDouble = false;

			for(;;)
			{
				char ch = peek();
				if (ch == '.')
				{
					isDouble = true;
					consume();
					continue;
				}
				if (ch == '-' || (ch >= '0' && ch <= '9'))
				{
					if (isDouble)
					{
						fractionStr += ch;
					} 
					else
					{
						numberStr += ch;
					}
					consume();
					continue;
				}
				break;
			}

			if (numberStr.Length == 0 || (isDouble && fractionStr.Length == 0))
			{
				return null;
			}

			double number;
			if (isDouble)
			{
				String finalNumberStr = numberStr + '.' + fractionStr;
				number = Double.Parse(finalNumberStr);
			}
			else
			{
				number = Double.Parse(numberStr);
			}

			return new JsonValue(number);
		}

		private JsonValue parseTrue()
		{
			consumeString("true");
			return new JsonValue(true);
		}

		private JsonValue parseFalse()
		{
			consumeString("false");
			return new JsonValue(false);
		}

		private JsonValue parseNull()
		{
			consumeString("null");
			return JsonValue.NullValue();
		}

		private JsonValue parseString()
		{
			String result = consumeQuotedString();
			if (result.Length == 0) 
			{
				return null;
			}
			return new JsonValue(result);
		}

		private JsonValue parseValue()
		{
			consumeWhitespace();
			char typeHint = peek();
			switch(typeHint) 
			{
				case '"':
					return parseString();
				case 't':
					return parseTrue();
				case 'f':
					return parseFalse();
				case 'n':
					return parseNull();
				case '{':
					return parseObject();
				case '[':
					return parseArray();
				case '-':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '0':
					return parseNumber();
			}
			return null;
		}

		public JsonValue parse() 
		{
			return parseValue();
		}
	}
}