using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public class JSONobject : JSONcomponent
    {
		private readonly Dictionary<string, JSONcomponent> KeyValuePairProperties;
		private readonly List<string> OrderedKeys;


		public JSONobject()
		{
			this.KeyValuePairProperties = new Dictionary<string, JSONcomponent>();
			this.OrderedKeys = new List<string>();
		}

		public override string Print(int indentation)
		{
			StringBuilder sb = new StringBuilder();

			foreach (string key in this.OrderedKeys)
			{
				JSONcomponent value = this.KeyValuePairProperties[key];
				string printedValue = value.Print(indentation + 1);

				if (sb.Length > 0)
					sb.Append(",\n");

				for (int i = 0; i <= indentation; i++)
					sb.Append('\t');

				sb.Append($"\"{key}\": {printedValue}");
			}

			sb.Insert(0, "{\n").Append('\n');

			for (int i = 0; i < indentation; i++)
				sb.Append('\t');

			return sb.Append('}').ToString();
		}

		public void AddProperty(string key, JSONcomponent value)
		{
			if(!this.KeyValuePairProperties.ContainsKey(key))
            {
				this.KeyValuePairProperties.Add(key, value);
				this.OrderedKeys.Add(key);
			}
			else
            {
				this.KeyValuePairProperties.Remove(key);
				this.KeyValuePairProperties.Add(key, value);
            }
		}

		public override JSONcomponent RemoveProperties(params string[] properties)
		{
			foreach (string PropertyName in this.OrderedKeys)
			{
				//retrieve object
				JSONcomponent PropertyValue = this.KeyValuePairProperties[PropertyName];

				//removing properties
				PropertyValue = PropertyValue.RemoveProperties(properties);

				//update references
				this.KeyValuePairProperties.Remove(PropertyName);
				this.KeyValuePairProperties.Add(PropertyName, PropertyValue); ////
			}

			if (this.OrderedKeys.Count != 1)
				return this;

			//here, there is only 1 property
			string propertyName = this.OrderedKeys.First();
			JSONcomponent propertyValue = this.KeyValuePairProperties[propertyName];

			// if this property is among those that must be removed, its value object is returned instead of this object
			foreach (string propertyToRemove in properties)
			{
				if (propertyName.Equals(propertyToRemove))
					return propertyValue;
			}

			return this;
		}
	}
}
