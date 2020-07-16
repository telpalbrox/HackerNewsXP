using System;

namespace HackerNews
{
	public class HNItem 
	{
		public String Title;
		public String Description;
		public String Url;
		public String User;

		public HNItem(String title, String description, String url, String user) 
		{
			Title = title;
			Description = description;
			Url = url;
			User = user;
		}
	}
}
