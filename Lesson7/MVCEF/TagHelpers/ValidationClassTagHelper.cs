using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace MVCEF.TagHelpers
{
    [HtmlTargetElement(input, Attributes = ValidationAttributeName)]
    public class ValidationClassTagHelper : TagHelper
    {
        private const string ValidationAttributeName = "bootstrap-validation";
        private const string input = "input";
        private const string aspFor = "asp-for";
        private const string isInvalid = "is-invalid";
        private const string isValid = "is-valid";



        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            ViewContext.ViewData.ModelState.TryGetValue(GetTargetName(), out var entry);
            switch (entry)
            {
                case null: return;
                case var _ when entry.Errors.Any():
                    SetClass(isInvalid);
                    return;
                case var _:
                    SetClass(isValid);
                    break;
            }


            void SetClass(string cssClass)
            {

                var tagBuilder = new TagBuilder(input);
                tagBuilder.AddCssClass(cssClass);
                output.MergeAttributes(tagBuilder);
            }

            string GetTargetName() =>
                ((ModelExpression)context.AllAttributes[aspFor].Value).Name;

        }
    }
}
