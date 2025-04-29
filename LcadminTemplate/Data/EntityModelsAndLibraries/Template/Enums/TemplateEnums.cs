
namespace Data
{

    public static class TemplateEnums
    {


        public enum SampleDropdown
        {
            Item1 = 1,
            Item2 = 2,
            Item3 = 3
        }
        public static string SampleDropdownDesc(SampleDropdown SampleDropdown)
        {
            if (SampleDropdown == SampleDropdown.Item1)
                return "Item1";
            else if (SampleDropdown == SampleDropdown.Item2)
                return "Item2";
            else if (SampleDropdown == SampleDropdown.Item3)
                return "Item 3";
            else
                return "";
        }
    }
}

