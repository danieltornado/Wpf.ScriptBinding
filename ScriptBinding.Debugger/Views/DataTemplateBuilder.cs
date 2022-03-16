using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace ScriptBinding.Debugger.Views
{
    class DataTemplateBuilder
    {
        public DataTemplate Build([NotNull] Type viewType)
        {
            const string viewPrefix = "view";

            string xaml = $"<DataTemplate><{viewPrefix}:{viewType.Name} /></DataTemplate>";

            var context = new ParserContext();
            context.XamlTypeMapper = new XamlTypeMapper(Array.Empty<string>());
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            // ReSharper disable once AssignNullToNotNullAttribute
            context.XamlTypeMapper.AddMappingProcessingInstruction(viewPrefix, viewType.Namespace, Assembly.GetExecutingAssembly().FullName);
            context.XmlnsDictionary.Add(viewPrefix, viewPrefix);

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }
    }
}
