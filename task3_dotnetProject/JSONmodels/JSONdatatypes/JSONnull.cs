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
	}
}
