namespace Helpers;

public static class UserInputHelper
{
    public static int GetInteger(string displayedQuestion)
    {
        int numberFromUser;
        bool isUserInputAnIntger;
        do
        {
            Console.WriteLine(displayedQuestion);
            var userInput = Console.ReadLine();

            isUserInputAnIntger = int.TryParse(userInput, out numberFromUser);

            if (!isUserInputAnIntger || numberFromUser == 0)
            {
                Console.WriteLine("Given input is not an proper value (value must be an integer and different than zero)");
            }
        } while (!isUserInputAnIntger);

        return numberFromUser;
    }
}