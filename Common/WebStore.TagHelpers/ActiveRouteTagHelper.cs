using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = ActiveRouteAttributeName)]
    public class ActiveRouteTagHelper : TagHelper
    {
        public const string ActiveRouteAttributeName = "is-active-route";
        public const string IgnoreActionAttributeName = "ignore-action";

        private Dictionary<string, string> _RouteValues;

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues
        {
            get => _RouteValues ?? (_RouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
            set => _RouteValues = value;
        }

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var ignore_action = context.AllAttributes.ContainsName(IgnoreActionAttributeName);
            if (ShouldBeActive(ignore_action))
                MakeActive(output);

            output.Attributes.RemoveAll(ActiveRouteAttributeName);
        }

        private bool ShouldBeActive(bool IgnoreAction)
        {
            var route_values = ViewContext.RouteData.Values;
            var currrent_controller = route_values["Controller"].ToString();
            var current_action = route_values["Action"].ToString();

            const StringComparison str_comp = StringComparison.CurrentCultureIgnoreCase;
            if (Controller?.Equals(currrent_controller, str_comp) == false) return false;
            if (!IgnoreAction && Action?.Equals(current_action, str_comp) == false) return false;

            foreach (var (key, value) in RouteValues)
                if (!route_values.ContainsKey(key) || route_values[key].ToString() != value)
                    return false;

            return true;
        }

        private void MakeActive(TagHelperOutput Output)
        {
            const string class_attribute_name = "class";
            const string active_state = "active";
            var class_attribute = Output.Attributes.FirstOrDefault(a => a.Name == class_attribute_name);
            if (class_attribute is null)
            {
                class_attribute = new TagHelperAttribute(class_attribute_name, active_state);
                Output.Attributes.Add(class_attribute);
            }
            else if (class_attribute.Value?.ToString().ToLower().Contains(active_state) != true)
                Output.Attributes.SetAttribute(
                    class_attribute_name,
                    class_attribute.Value is null
                        ? active_state
                        : $"{class_attribute.Value} {active_state}");

        }
    }
}
