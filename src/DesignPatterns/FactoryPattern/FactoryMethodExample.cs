using FactoryPattern.Extensions;

namespace FactoryPattern
{
    public abstract class Element
    {
        protected abstract Button CreateButton();

        public Element() => CreateButton();
    }

    public class NavigationBar_FactoryMethodExample : Element
    {
        protected override Button CreateButton()
        {
            return new Button { Type = "Defatult Button".Dump() };
        }
    }

    public class DropdownMenu_FactoryMethodExample : Element
    {
        protected override Button CreateButton()
        {
            return new Button { Type = "Defatult Button".Dump() };
        }
    }

    public class AndroidNavigationBar_FactoryMethodExample : Element
    {
        protected override Button CreateButton()
        {
            return new Button { Type = "Android Button".Dump() };
        }
    }

    public class AndroidDropdownMenu_FactoryMethodExample : Element
    {
        protected override Button CreateButton()
        {
            return new Button { Type = "Android Button".Dump() };
        }
    }
}