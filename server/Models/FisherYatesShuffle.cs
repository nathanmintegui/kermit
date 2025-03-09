namespace Kermit.Models;

public class FisherYatesShuffle : IShuffler
{
    public void Shuffle(int[] array)
    {
        if (array.Length == 0)
        {
            return;
        }

        Random rand = new();

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
