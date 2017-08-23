﻿using System;
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
            var whatsmytype = Model.ModelExplorer.ModelType;
            var whatsmytype2 = Model.ModelExplorer.Model.GetType();
            ModelMetadata metadata = metadataProvider.GetMetadataForProperty(Model.ModelExplorer.Model.GetType(), Prop);
            ModelExplorer modelExplorer = new ModelExplorer(metadataProvider, metadata, Model);
            ModelExpression nm = new ModelExpression("SearchTerm", modelExplorer);

            TagHelperAttribute aspFor = new TagHelperAttribute("asp-for", nm, HtmlAttributeValueStyle.DoubleQuotes);
            TagHelperAttributeList attrs = new TagHelperAttributeList();
            attrs.Add(aspFor);

            TagHelperContext newContext = new TagHelperContext(context.TagName, attrs, context.Items, context.UniqueId);
            
            List<Variance> variances = new List<Variance>();
            FieldInfo[] fi = For.GetType().GetFields();
            foreach (FieldInfo f in fi)
            {
                Variance v = new Variance();
                v.Prop = f.Name;
                v.valA = f.GetValue(For);
                v.valB = f.GetValue(nm);
                if (!v.valA.Equals(v.valB))
                    variances.Add(v);

            }

            await base.ProcessAsync(newContext, output);

            var test = output;
        }

        private class Variance
        {
            public string Prop { get; set; }
            public object valA { get; set; }
            public object valB { get; set; }
        }
    }
}
