using System;
namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public class JSONstring : JSONcomponent
    {
		private readonly string Str;
	
	
		public JSONstring(string Str)
		{
			this.Str = Str;
		}

		public string Get() { return this.Str; }

		public override string Print(int indentation)
		{
			return $"\"{this.Str}\"";
		}

		public override JSONcomponent RemoveProperties(params string[] properties)
		{
			return this;
		}

		public override JSONarray RetrieveObjectsWithProperty(string key, string valueKeyword)
		{
			return new JSONarray();
		}

		public bool Contains(string substring)
        {
			return this.Str.Contains(substring);
        }

        public override void RemoveObjectProperty(string key)
        {
			return;
        }
    }
}
