using System;
namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public class JSONbool : JSONcomponent
    {
		private readonly bool Boolean;
	
	
		public JSONbool(bool boolean)
		{
			this.Boolean = boolean;
		}

		public bool Get() { return this.Boolean; }

		
		public override string Print(int indentation)
		{
			return this.Boolean? "true" : "false";
		}

		public override JSONcomponent RemoveProperties(params string[] properties)
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
