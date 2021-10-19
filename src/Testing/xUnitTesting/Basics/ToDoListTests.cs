using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using xUnitTesting.Units;

namespace xUnitTesting
{
    public class ToDoListTests
    {
        //One approach could be to put unit logic in here in tests, 
        //and tests it that way, but its kinda a wrong approach
        //cause you are not testing said unit(application) itself
        //You should test the unit (application) itself
        //In other words you should use testing framework as a client that interacts with 
        //your applicaton (logic) and tests it that way

        [Fact]
        public void Shouled_Add_SavesToDoItem()
        {
            //arrange
            var list = new ToDoList();

            //act
            list.Add(new("Test content"));

            //assert
            var savedItem = list.All.First();
            savedItem.Should().NotBeNull();
            savedItem.Content.Should().Equals("Test content");
            savedItem.Id.Should().Equals(1);
            savedItem.Complete.Should().BeFalse();
        }

        [Fact]
        public void Should_TodoItemIdIncrementsEveryTimeWeAdd()
        {
            //arrange
            var list = new ToDoList();

            //act
            list.Add(new("Test 1"));
            list.Add(new("Test 2"));

            //assert
            var items = list.All.ToArray();
            items[0].Id.Should().Equals(1);
            items[1].Id.Should().Equals(2);
        }

        [Fact]
        public void Should_Complete_SetsTodoItemCompleteFlagToTrue()
        {
            //arrange
            var list = new ToDoList();
            list.Add(new("Test 1"));

            //act
            list.Complete(1);

            //assert
            var completedItem = list.All.First();

            completedItem.Should().NotBeNull();
            completedItem.Id.Should().Equals(1);
            completedItem.Complete.Should().BeTrue();
        }
    }
}
