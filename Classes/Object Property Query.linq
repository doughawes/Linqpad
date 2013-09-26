<Query Kind="Program">
  <Reference>D:\Repositories\IntakeService\IntakeService.Validation\bin\Release\IntakeService.PatientIntake.dll</Reference>
</Query>

void Main()
{
	var type = new IntakeService.PatientIntake.Models.BatchStatus();
	var line = ", {{ field: \"{0}\", title: \"{1}\", width: 160 }};";

	var fields = new List<FieldInfo>();
	foreach (var p in type.GetType().GetProperties())
	{
		var dataType = p.PropertyType;
		if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
			dataType = System.Nullable.GetUnderlyingType(dataType);
		fields.Add(new FieldInfo { Name = p.Name, DataType = dataType.Name });
		var result = String.Format(line, p.Name, Utility.GetFriendlyColumnTitle(p.Name));
		result.Dump();
	}
}

class FieldInfo
{
	public string Name { get; set; }
	public string DataType { get; set; }
}

class WordMap
{
	public string From { get; set; }
	public string To { get; set; }
}

static class Utility
{
	public static string GetFriendlyColumnTitle(string title)
	{
		var friendlyTitle = AddSpaceBeforeCapitalLetters(title);
		return MapKnownWords(friendlyTitle);
	}

	public static string MapKnownWords(string text)
	{
		var map = new List<WordMap>();
		map.Add(new WordMap { From = "Dttm", To = "Date" });
		
		var words = text.Split(' ').Distinct().ToList();
		var matchWords = map.Select (m => m.From).ToList();
		var matches = words.Intersect(matchWords).ToList();
		
		var result = text;
		foreach (var match in matches)
		{
			var replacementWord = map.Where(m => m.From == match).Select(m => m.To).FirstOrDefault();
			result = text.Replace(match, replacementWord);
		}
		return result;
	}

	public static string AddSpaceBeforeCapitalLetters(string text)
	{
		Regex r = new Regex(@"(?!^)(?=[A-Z])");
		return r.Replace(text, " ");
	}

	public static string ConvertDataType(string dataType)
	{
		switch (dataType.ToLower())
		{
		
			case "string":
				return "number";
				
			case "int16":
				return "number";
				
			case "int32":
				return "number";
				
			case "int64":
				return "number";

			case "date":
				return "datetime";

			case "datetime":
				return "datetime";
				
			case "boolean":
				return "boolean";
			
			default:
				return new String('Z', 50);
		}
	}
}