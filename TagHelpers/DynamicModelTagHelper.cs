using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Reflection;

namespace TagHelpers
{
    [HtmlTargetElement("input", Attributes = "asp-model")]
    public class DynamicModelTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-property")]
        public string Prop { get; set; }

        [HtmlAttributeName("asp-model")]
        public ModelExpression Model { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var input = Model.ModelExplorer.GetExplorerForProperty("SearchTerm");

            IModelMetadataProvider metadataProvider = new EmptyModelMetadataProvider();
            
            ModelMetadata metadata = metadataProvider.GetMetadataForProperty(Model.ModelExplorer.Model.GetType(), Prop);
            ModelExplorer container = input;
            ModelExplorer modelExplorer = new ModelExplorer(metadataProvider, input, metadata, null);
            ModelExpression nm = new ModelExpression("SearchTerm", modelExplorer);

            TagHelperAttribute aspFor = new TagHelperAttribute("asp-for", nm, HtmlAttributeValueStyle.DoubleQuotes);
            TagHelperAttributeList attrs = new TagHelperAttributeList();
            attrs.Add(aspFor);

            TagHelperContext newContext = new TagHelperContext(context.TagName, attrs, context.Items, context.UniqueId);
            TagHelperOutput newOutput = new TagHelperOutput("input", output.Attributes, output.GetChildContentAsync);

            await base.ProcessAsync(newContext, newOutput);

            var test = output;
        }

       
    }
}
