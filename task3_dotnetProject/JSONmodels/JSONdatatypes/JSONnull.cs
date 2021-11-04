using System;
namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public class JSONnull : JSONcomponent
    {
		public JSONnull() { }

		public override string Print(int indentation)
		{
			return "null";
		}

		public override JSONcomponent RemoveProperties(params string[] properties)
		{
			return this;
		}

		public override JSONarray RetrieveObjectsWithProperty(string key, string valueKeyword)
		{
			return new JSONarray();
		}

		public override void RemoveObjectProperty(string key)
		{
			return;
		}
	}
}
