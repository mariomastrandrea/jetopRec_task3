using System;
namespace task3_dotnetProject.JSONmodels.JSONdatatypes
{
    public class JSONnumber : JSONcomponent
    {
		private readonly double? DoubleNumber;
		private readonly long? LongNumber;
		private readonly string StringRepresentation;


		public JSONnumber(double number)
		{
			this.DoubleNumber = number;
			this.LongNumber = null;
		}

		public JSONnumber(long number)
		{
			this.LongNumber = number;
			this.DoubleNumber = null;
		}

		public JSONnumber(double number, string possibleNumber) : this(number)
		{
			this.StringRepresentation = possibleNumber;
		}

		public double? GetDouble() { return this.DoubleNumber; }
		public long? GetLong() { return this.LongNumber; }
		public double? Get() { return this.DoubleNumber != null ? this.DoubleNumber : this.LongNumber; }

		public override string Print(int indentation)
		{
			if (this.StringRepresentation != null)
				return this.StringRepresentation;

			if (this.DoubleNumber != null)
				return $"{this.DoubleNumber}";

			if (this.LongNumber != null)
				return $"{this.LongNumber}";

			return null;
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
