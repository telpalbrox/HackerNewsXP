using System;
using System.Collections;

namespace HackerNews
{
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
}
