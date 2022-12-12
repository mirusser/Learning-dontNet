using System;

public class UserInputHelper
{
    public static int GetIntegerFromUser(string displayedQuestion)
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
                Console.WriteLine("Given input is not an proper value (integer different than zero)");
            }
        } while (!isUserInputAnIntger);

        return numberFromUser;
    }
}