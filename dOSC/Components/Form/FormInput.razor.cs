using Microsoft.AspNetCore.Components;

namespace dOSC.Components.Form
{
    public partial class FormInput
    {
        [Parameter]
        public FormInputType InputType { get; set; } = FormInputType.None;

        [Parameter]
        public dynamic Value { get; set; }

        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Parameter] 
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment? TopContent { get; set; }

        [Parameter] 
        public RenderFragment? BottomContent { get; set; }

        public enum FormInputType
        {
            Text,
            Number,
            Email,
            Password,
            Date,
            Time,
            DateTime,
            Month,
            Week,
            Search,
            Tel,
            Url,
            Color,
            Range,
            File,
            Checkbox,
            Radio,
            Select,
            TextArea,
            Hidden,
            Submit,
            Reset,
            Button,
            Image,
            None,
        }
    }
}
