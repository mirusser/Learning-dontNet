namespace BuberDinner.Application.UnitTests.TestUtils.Constants;

public static partial class Constants
{
    public static class Menu
    {
        public const string Name = "Menu name";
        public const string Description = "Menu description";
        public const string SectionName = "Menu section name";
        public const string SectionDescription = "Menu section description";
        public const string ItemName = "Menu item name";
        public const string ItemDescription = "Menu item description";

        public static string SectionNameFromIndex(int index) 
            => $"{SectionName} {index}";

        public static string SectionDescriptionFromIndex(int index)
            => $"{SectionDescription} {index}";

        public static string ItemNameFromIndex(int index)
            => $"{ItemName} {index}";

        public static string ItemDescriptionFromIndex(int index)
            => $"{ItemDescription} {index}";
    }
}