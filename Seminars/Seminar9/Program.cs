using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;

string jsonFilePath = "input.json";
string xmlOutputFilePath = "output.xml";
Console.OutputEncoding = Encoding.UTF8;

try
{
    string jsonInput = File.ReadAllText(jsonFilePath);

    if (IsValidJson(jsonInput))
    {
        // Динамически десериализуем JSON в List<ExpandoObject>
        List<ExpandoObject> dynamicObjects = DeserializeJsonToExpandoList(jsonInput);

        // Создаем XML-документ из списка ExpandoObject
        XmlDocument xmlDocument = SerializeToXml(dynamicObjects);
        xmlDocument.Save(xmlOutputFilePath);

        // Выводим результат в консоль
        Console.WriteLine("Результат конвертации в XML:");
        Console.WriteLine(xmlDocument.OuterXml);

        Console.WriteLine($"Исходный JSON сохранен в output.json");
        Console.WriteLine($"Результат конвертации в XML сохранен в {xmlOutputFilePath}");
    }
    else
    {
        Console.WriteLine("Введенный JSON невалиден.");
    }
}
catch (FileNotFoundException)
{
    Console.WriteLine($"Файл {jsonFilePath} не найден.");
}
catch (JsonException e)
{
    Console.WriteLine($"Ошибка при парсинге JSON: {e.Message}");
}
catch (Exception e)
{
    Console.WriteLine($"Произошла ошибка: {e.Message}");
}


static bool IsValidJson(string json)
{
    try
    {
        JsonDocument.Parse(json);
        return true;
    }
    catch (JsonException)
    {
        return false;
    }
}

static List<ExpandoObject> DeserializeJsonToExpandoList(string json)
{
    JsonDocument jsonDocument = JsonDocument.Parse(json);

    return jsonDocument.RootElement.EnumerateArray()
        .Select(personElement => DeserializeJsonToExpando(personElement))
        .ToList();
}

static ExpandoObject DeserializeJsonToExpando(JsonElement jsonElement)
{
    var expandoObject = new ExpandoObject();
    var expandoDict = (IDictionary<string, object>)expandoObject;

    foreach (var property in jsonElement.EnumerateObject())
    {
        AddPropertyToExpando(expandoDict, property.Name, property.Value);
    }

    return expandoObject;
}

static void AddPropertyToExpando(IDictionary<string, object> expandoDict, string key, JsonElement value)
{
    if (value.ValueKind == JsonValueKind.Object)
    {
        var nestedExpando = new ExpandoObject();
        var nestedExpandoDict = (IDictionary<string, object>)nestedExpando;

        foreach (var nestedProperty in value.EnumerateObject())
        {
            AddPropertyToExpando(nestedExpandoDict, nestedProperty.Name, nestedProperty.Value);
        }

        expandoDict[key] = nestedExpando;
    }
    else if (value.ValueKind == JsonValueKind.Array)
    {
        var array = new List<object>();

        foreach (var arrayElement in value.EnumerateArray())
        {
            var nestedExpando = new ExpandoObject();
            var nestedExpandoDict = (IDictionary<string, object>)nestedExpando;

            foreach (var nestedProperty in arrayElement.EnumerateObject())
            {
                AddPropertyToExpando(nestedExpandoDict, nestedProperty.Name, nestedProperty.Value);
            }

            array.Add(nestedExpando);
        }

        expandoDict[key] = array;
    }
    else
    {
        // Преобразуем значение в строку
        expandoDict[key] = value.ToString();
    }
}

static XmlDocument SerializeToXml(List<ExpandoObject> dynamicObjects)
{
    XmlDocument xmlDocument = new XmlDocument();
    XmlElement rootElement = xmlDocument.CreateElement("root");
    xmlDocument.AppendChild(rootElement);

    foreach (var dynamicObject in dynamicObjects)
    {
        SerializeExpandoToXml(dynamicObject, rootElement, xmlDocument);
    }

    return xmlDocument;
}

static void SerializeExpandoToXml(ExpandoObject dynamicObject, XmlElement xmlElement, XmlDocument xmlDocument)
{
    XmlElement personElement = xmlDocument.CreateElement("item");
    xmlElement.AppendChild(personElement);

    foreach (var property in dynamicObject)
    {
        if (property.Value is ExpandoObject nestedExpando)
        {
            XmlElement nestedElement = xmlDocument.CreateElement(property.Key);
            personElement.AppendChild(nestedElement);
            SerializeExpandoToXml(nestedExpando, nestedElement, xmlDocument);
        }
        else if (property.Value is List<object> array)
        {
            foreach (var arrayItem in array)
            {
                XmlElement arrayItemElement = xmlDocument.CreateElement(property.Key);
                personElement.AppendChild(arrayItemElement);
                SerializeExpandoToXml((ExpandoObject)arrayItem, arrayItemElement, xmlDocument);
            }
        }
        else
        {
            XmlElement propertyElement = xmlDocument.CreateElement(property.Key);
            propertyElement.InnerText = property.Value.ToString();
            personElement.AppendChild(propertyElement);
        }
    }
}
