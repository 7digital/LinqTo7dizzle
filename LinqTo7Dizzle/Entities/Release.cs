using System;
using System.Xml.Linq;

namespace LinqTo7Dizzle.Entities
{
	public class Release : Entity
	{
		public int Id { get; private set; }
		public string Title { get; private set; }
		public string ImageUrl { get; private set; }

		public static Release CreateFromChartItem(XElement chartItem)
		{
			if (chartItem == null)
			{
				return null;
			}

			var releaseNode = chartItem.Element("release");

			if (releaseNode == null)
				throw new Exception("Could not find the release");

			var idAttribute = releaseNode.Attribute("id");

			if (idAttribute == null)
				throw new Exception("No id attr");

			var idString = idAttribute.Value;
			int id;
			if (Int32.TryParse(idString, out id) == false)
				throw new Exception("Invalid id value");

			return new Release
			{
				Id = id,
				Title = GetStringValue(releaseNode, "title"),
				ImageUrl = GetStringValue(releaseNode, "image"),
			};
		}

		private static string GetStringValue(XContainer element, string node)
		{
			var titleNode = element.Element(node);
			return titleNode != null ? titleNode.Value : "";
		}

		public override string ToString()
		{
			return string.Format("Id: {0}, Title: {1}", Id, Title);
		}
	}
}