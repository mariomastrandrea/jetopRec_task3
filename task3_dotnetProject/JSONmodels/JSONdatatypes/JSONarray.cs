using System;
using System.Collections.Generic;
using System.Text;

namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public class JSONarray : JSONcomponent
    {
		private readonly List<JSONcomponent> Array;


		public JSONarray()
		{
			this.Array = new List<JSONcomponent>();
		}

		public override string Print(int indentation)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var component in this.Array)
			{
				string printedComponent = component.Print(indentation + 1);

				if (sb.Length > 0)
					sb.Append(",\n");

				for (int i = 0; i <= indentation; i++)
					sb.Append('\t');

				sb.Append(printedComponent);
			}

			sb.Insert(0, "[\n").Append('\n');

			for (int i = 0; i < indentation; i++)
				sb.Append('\t');

			return sb.Append(']').ToString();
		}

		public bool Add(JSONcomponent component)
		{
			this.Array.Add(component);
			return true;
		}

		public override JSONcomponent RemoveProperties(params string[] properties)
		{
			for (int i = 0; i < this.Array.Count; i++)
			{
				JSONcomponent element = this.Array[i];
				this.Array.RemoveAt(i);
				element = element.RemoveProperties(properties);
				this.Array.Insert(i, element);
			}

			return this;
		}

		public override JSONarray RetrieveObjectsWithProperty(string key, string valueKeyword)
		{
			JSONarray result = new JSONarray();

			foreach(JSONcomponent element in this.Array)
            {
				JSONarray subresult = element.RetrieveObjectsWithProperty(key, valueKeyword);
				result.AddAll(subresult);
            }

			return result;
		}

        public void AddAll(JSONarray array)
        {
			foreach (JSONcomponent element in array.Array)
				this.Array.Add(element);
        }

		public override void RemoveObjectProperty(string key)
		{
			foreach (JSONcomponent element in this.Array)
				element.RemoveObjectProperty(key);
		}
	}
}
