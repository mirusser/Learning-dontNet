using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactoryPattern.Extensions;

namespace FactoryPattern
{
    public class NavigationBar_FactoryExample
    {
        public NavigationBar_FactoryExample() => ButtonFactory.CreateButton();
    }

    public class DropdownMenu_FactoryExample
    {
        public DropdownMenu_FactoryExample() => ButtonFactory.CreateButton();
    }

    public class ButtonFactory
    {
        public static Button CreateButton()
        {
            return new Button { Type = "Default Button".Dump() };
        }
    }
}
