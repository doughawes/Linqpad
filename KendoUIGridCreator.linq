void Main()
{
	var type = new IntakeService.PatientIntake.Models.BatchStatus();
	List<ColumnInfo> columns = Utility.GetColumnInfo(type.GetType());

	//Kendo.GetColumnWidthDictionary(columns, "columWidths");
	Dictionary<string, int> columWidths = new Dictionary<string, int>();
	columWidths.Add("BatchId", 100);
	columWidths.Add("BatchStatusTypeId", 300);
	columns.Where(c => columWidths.ContainsKey(c.Name)).Select(c => { c.Width = columWidths[c.Name]; return c; }).ToList();

	//Utility.GetArrayFromList(columns.Select(c => c.Name).ToList()).Dump();
	var notGroupable = new string [] { "EffectiveDttm", "TerminationDttm" };
	columns.Where(c => notGroupable.Contains(c.Name)).Select(c => { c.Groupable = false; return c; }).ToList();

	//Kendo.GetColumnFormatDictionary(columns, "columnFormats");
	Dictionary<string, string> columnFormats = new Dictionary<string, string>();
	columnFormats.Add("EffectiveDttm", "{0:MM/dd/yyyy}");
	columnFormats.Add("LastEditedDttm", "{0:MM/dd/yyyy}");
	columns.Where(c => columnFormats.ContainsKey(c.Name)).Select(c => { c.Format = columnFormats[c.Name]; return c; }).ToList();

	//Kendo.GetTemplateDictionary(columns, "columnTemplates");
	Dictionary<string, string> columnTemplates = new Dictionary<string, string>();
	columnTemplates.Add("ErrorText", "template: function (item) { return item.ErrorText; }");
	columns.Where(c => columnTemplates.ContainsKey(c.Name)).Select(c => { c.Template = columnTemplates[c.Name]; return c; }).ToList();
	
	//Kendo.GetAttributesDictionary(columns, "columnAttributes");
	Dictionary<string, string> columnAttributes = new Dictionary<string, string>();
	columnAttributes.Add("BatchId", "attributes: { style: \"text-align: right;\" }");
	columns.Where(c => columnAttributes.ContainsKey(c.Name)).Select(c => { c.Attributes = columnAttributes[c.Name]; return c; }).ToList();
	
	var datasourceFormat = ", {{ {0}: \"{1}\" }};";
	var displayFormat = ", {{ field: \"{0}\", title: \"{1}\", width: {2} }};";




	columns.ForEach(c => String.Format(datasourceFormat, c.Name, c.JavaScriptDataType).Dump());
	Environment.NewLine.Dump();
	columns.ForEach(c => Kendo.GetDisplayColumn(c, displayFormat).Dump());
}

static class Kendo
{
	public static string GetDisplayColumn(ColumnInfo column, string format)
	{
		var result = String.Format(format, column.Name, column.Title, column.Width);
		var endMarker = " };";
		var marker = "~@#$%^&*()";
		result = result.Replace(endMarker, marker + endMarker);
		
		if (!column.Groupable)
			result = result.Replace(marker, ", groupable: false" + marker);
		
		if (!String.IsNullOrWhiteSpace(column.Format))
			result = result.Replace(marker, ", " + column.Format + marker);

		if (!String.IsNullOrWhiteSpace(column.Template))
			result = result.Replace(marker, ", " + column.Template + marker);

		if (!String.IsNullOrWhiteSpace(column.Attributes))
			result = result.Replace(marker, ", " + column.Attributes + marker);
		
		return result.Replace(marker, String.Empty);
	}

	public static string GetColumnFormatDictionary(List<ColumnInfo> columns, string name)
	{
		var result = new StringBuilder();
		result.AppendLine(String.Format("Dictionary<string, string> {0} = new Dictionary<string, string>();", name));
		columns.ForEach(c => result.AppendLine(String.Format("{0}.Add(\"{1}\", \"{2}\");", name, c.Name, Utility.GetDefaultFormatForJavaScriptDataType(c))));
		return result.ToString().Dump();
	}

	public static string GetColumnWidthDictionary(List<ColumnInfo> columns, string name)
	{
		var result = new StringBuilder();
		result.AppendLine(String.Format("Dictionary<string, int> {0} = new Dictionary<string, int>();", name));
		columns.ForEach(c => result.AppendLine(String.Format("{0}.Add(\"{1}\", {2});", name, c.Name, GetDefaultColumnWidth(c))));
		return result.ToString().Dump();
	}

	public static string GetTemplateDictionary(List<ColumnInfo> columns, string name)
	{
		var result = new StringBuilder();
		result.AppendLine(String.Format("Dictionary<string, string> {0} = new Dictionary<string, string>();", name));
		columns.ForEach(c => result.AppendLine(String.Format("{0}.Add(\"{1}\", \"{2}\");", name, c.Name, GetDefaultTemplate(c))));
		return result.ToString().Dump();
	}

	public static string GetAttributesDictionary(List<ColumnInfo> columns, string name)
	{
		var result = new StringBuilder();
		result.AppendLine(String.Format("Dictionary<string, string> {0} = new Dictionary<string, string>();", name));
		columns.ForEach(c => result.AppendLine(String.Format("{0}.Add(\"{1}\", \"{2}\");", name, c.Name, GetDefaultAttributes(c))));
		return result.ToString().Dump();
	}

	public static string GetDefaultAttributes(ColumnInfo column)
	{
		return String.Format("attributes: {{ style: \"text-align: right;\" }}", column.Name );
	}
	
	public static string GetDefaultTemplate(ColumnInfo column)
	{
		return String.Format("template: function (item) {{ return item.{0}; }}", column.Name );
	}
	
	public static int GetDefaultColumnWidth(ColumnInfo column)
	{
		return 150;
	}

}

class ColumnInfo
{
	public string Name { get; set; }
	public string Title { get; set; }
	public string DataType { get; set; }
	public string JavaScriptDataType { get; set; }
	public int Width { get; set; }
	public string Format { get; set; }
	public bool Groupable { get; set; }	
	public string Template { get; set; }
	public string Attributes { get; set; }
	
	public ColumnInfo()
	{
		Width = 150;
		Groupable = true;
	}
}

class WordMap
{
	public string From { get; set; }
	public string To { get; set; }
}

static class Utility
{

	public static string GetListPairFromList(List<string> list, string name)
	{
		var result = new StringBuilder();
		result.AppendLine(String.Format("Dictionary<string, string> {0} = new Dictionary<string, string>();", name));
		list.ForEach(i => result.AppendLine(String.Format("{0}.Add(\"{1}\", \"\");", name, i)));
		return result.ToString().Dump();
	}

	public static string GetDefaultFormatForJavaScriptDataType(ColumnInfo column)
	{
		switch (column.JavaScriptDataType)
		{
			case "number":
				return "";
				
			case "datetime":
				return "{0:MM/dd/yyyy}";
				
			case "boolean":
				return String.Empty;
				
			default:
				return String.Empty;
		}
	}

	public static string GetArrayFromList(List<string> list)
	{
		return "new string[] { \"" + String.Join("\", \"", list) + "\" };";
	}

	public static List<ColumnInfo> GetColumnInfo(Type type)
	{
		var columns = new List<ColumnInfo>();
		foreach (var p in type.GetProperties())
		{
			var dataType = p.PropertyType;
			if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
				dataType = System.Nullable.GetUnderlyingType(dataType);
				
			columns.Add(new ColumnInfo { Name = p.Name, DataType = dataType.Name,
				Title = Utility.GetFriendlyColumnTitle(p.Name),
				JavaScriptDataType = Utility.ConvertToJavaScriptDataType(dataType.Name) });
		}
		return columns;
	}

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

	public static string ConvertToJavaScriptDataType(string dataType)
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
