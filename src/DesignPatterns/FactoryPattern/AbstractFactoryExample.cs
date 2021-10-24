using FactoryPattern.Extensions;

namespace FactoryPattern
{
    public class NavigationBar_AbstractFactoryExample
    {
        public NavigationBar_AbstractFactoryExample(IUIFactory factory) => factory.CreateButton();
    }

    public class DropdownMenu_AbstractFactoryExample
    {
        public DropdownMenu_AbstractFactoryExample(IUIFactory factory) => factory.CreateButton();
    }

    public interface IUIFactory
    {
        public Button CreateButton();
    }

    public class AppleFactory : IUIFactory
    {
        public Button CreateButton()
        {
            return new Button { Type = "iOS Button".Dump() };
        }
    }

    public class AndroidFactory : IUIFactory
    {
        public Button CreateButton()
        {
            return new Button { Type = "Android Button".Dump() };
        }
    }
}