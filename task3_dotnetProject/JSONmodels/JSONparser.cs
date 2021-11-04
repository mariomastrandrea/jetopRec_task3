using System;
using System.IO;
using System.Text;
using task3_dotnetProject.JSONmodels.JSONdatatypes;

namespace task3_dotnetProject.JSONmodels
{
    public class JSONparser
    {
		public JSONparser() { }

		public JSONcomponent Parse(StringBuilder currentLine, TextReader tr) 
		{
			string currentLineString = currentLine?.ToString();
		
			if(currentLineString == null) {
				currentLineString = tr.ReadLine();
				if(currentLineString == null) return null;
			}
		
			// ignore blank strings (here currentLineString is not null)
			while(string.IsNullOrWhiteSpace(currentLineString)) {
				currentLineString = tr.ReadLine();
				if(currentLineString == null) return null;
			}

			// remove whitespace characters
			currentLine.Clear();
			currentLine.Append(currentLineString.Trim());
			char firstChar = currentLine[0];

			if (firstChar == '{')   // there is a JSON object
				return this.ParseObject(currentLine, tr);

			if (firstChar == '[')   // there is a JSON array
				return this.ParseArray(currentLine, tr);

			if (firstChar == '"')   // there is a string
				return this.ParseString(currentLine);

			JSONcomponent result;

			if ((result = this.ParseNull(currentLine)) != null) // there is a null value
				return result;

			if ((result = this.ParseBool(currentLine)) != null) // there is a boolean value
				return result;

			// it may be a number	
			return this.ParseNumber(currentLine, tr);
		}

		private JSONobject ParseObject(StringBuilder currentLine, TextReader tr) 
		{
			// remove starting '{'
			string currentLineString = currentLine.Remove(0,1).ToString();
		
			// remove blank lines (here currentLineString is not null)
			while(string.IsNullOrWhiteSpace(currentLineString))
			{
				currentLineString = tr.ReadLine();
				if(currentLineString == null) return null;
			}

			// remove whitespaces
			currentLineString = currentLineString.Trim();
			currentLine.Clear();
			currentLine.Append(currentLineString);
			
			JSONobject obj = new JSONobject();
		
			while(!currentLineString.StartsWith("}"))
			{
				string[] fields = currentLineString.Split(":", 2);
				string key = fields[0].Trim();
				key = key.Substring(1, key.Length - 2);
			
				// update current line buffer: removing 'key:'
				currentLine.Remove(0, fields[0].Length + 1); 
			
				JSONcomponent value = this.Parse(currentLine, tr);
				obj.AddProperty(key, value);
			
				// remove comma separator
				if(currentLine.Length > 0 && currentLine[0] == ',') 
					currentLine.Remove(0,1); 
			
				currentLineString = currentLine.ToString();
			
				while(string.IsNullOrWhiteSpace(currentLineString))
				{
					currentLineString = tr.ReadLine();
					if(currentLineString == null) break;
				}

				currentLineString = currentLineString.Trim();
				currentLine.Clear();
				currentLine.Append(currentLineString);
			}
		
			// remove starting '{'
			currentLine.Remove(0,1);

			return obj;
		}

		private JSONarray ParseArray(StringBuilder currentLine, TextReader tr)
		{
			// remove starting '['
			string currentLineString = currentLine.Remove(0,1).ToString();
				
			// removing blank lines
			while(string.IsNullOrWhiteSpace(currentLineString)) {
				currentLineString = tr.ReadLine();
				if(currentLineString == null) return null;
			}

			// remove whitespaces
			currentLineString = currentLineString.Trim();
			currentLine.Clear();
			currentLine.Append(currentLineString);
		
			JSONarray array = new JSONarray();
		
			while(!currentLineString.StartsWith("]"))
			{
					
				JSONcomponent nextComponent = this.Parse(currentLine, tr);
				array.Add(nextComponent);
			
				// remove comma separator
				if(currentLine.Length > 0 && currentLine[0] == ',') 
					currentLine.Remove(0,1); 
			
				currentLineString = currentLine.ToString();
			
				while(string.IsNullOrWhiteSpace(currentLineString))
				{
					currentLineString = tr.ReadLine();
					if(currentLineString == null) break;
				}

				currentLineString = currentLineString.Trim();
				currentLine.Clear();
				currentLine.Append(currentLineString);
			}
		
			// remove starting '['
			currentLine.Remove(0,1);

			return array;
		}

		private JSONstring ParseString(StringBuilder currentLine) 
		{
			// remove starting '"'
			string currentLineString = currentLine.Remove(0,1).ToString();

			string[] fields = currentLineString.Split("\"", 2);
			string stringValue = fields[0];	//retrieving String value
		
			//check if there is an escaped '"' character
			while(stringValue.Length > 0 && 
					stringValue[stringValue.Length - 1] == '\\' &&
					!(stringValue.Length > 1 && stringValue[stringValue.Length - 2] == '\\'))
			{
				fields = fields[1].Split("\"", 2);
				stringValue = stringValue + '"' + fields[0];
			}

			// update current line buffer: delete the string value and trailing '"'
			currentLine.Remove(0, stringValue.Length+1);
		
			return new JSONstring(stringValue);
		}

		private JSONnull ParseNull(StringBuilder currentLine)
		{
			if (currentLine.Length >= 4 &&
					currentLine.ToString().Substring(0, 4).Equals("null"))
			{ // there is a null value
				currentLine.Remove(0, 4); //remove 'null'
				return new JSONnull();
			}

			return null;
		}

		private JSONbool ParseBool(StringBuilder currentLine)
		{
			if (currentLine.Length >= 4 &&
					currentLine.ToString().Substring(0, 4).ToLower().Equals("true"))
			{ // there is a 'true' boolean value
				currentLine.Remove(0, 4);
				return new JSONbool(true);
			}

			if (currentLine.Length >= 5 &&
					currentLine.ToString().Substring(0, 4).ToLower().Equals("false"))
			{ // there is a 'false' boolean value
				currentLine.Remove(0, 5);
				return new JSONbool(false);
			}

			return null;
		}

		private JSONnumber ParseNumber(StringBuilder currentLine, TextReader br) 
		{
			string possibleNumber = this.RetrievePossibleLeadingDouble(currentLine.ToString());

			bool parsed = double.TryParse(possibleNumber, out double number);

			if(!parsed)
            {
				Console.WriteLine("Number format exception with line: \"" + currentLine + "\"");   //for debugging
				return null;
			}
			
			currentLine.Remove(0, possibleNumber.Length);
			return new JSONnumber(number, possibleNumber);
		}

		private string RetrievePossibleLeadingDouble(string line)
		{

			char[] chars = line.ToCharArray();
			int resultPointer = 0;
			bool dotFlag = false;

			foreach (char c in chars)
			{
				//possible chars are only digits, one dot and an optional starting '-'
				if ((!char.IsDigit(c) && c != '.' && c != '-' && resultPointer > 0) || (c == '.' && dotFlag))
					break;

				if (c == '.') dotFlag = true;
				resultPointer++;
			}

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < resultPointer; i++)
				sb.Append(chars[i]);

			return sb.ToString();
		}
	}
}
