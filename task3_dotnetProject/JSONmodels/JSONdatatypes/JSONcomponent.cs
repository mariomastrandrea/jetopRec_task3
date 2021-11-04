using System;
namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public abstract class JSONcomponent
    {
		public abstract string Print(int indentation);
		public abstract JSONcomponent RemoveProperties(params string[] properties);
	}
}
