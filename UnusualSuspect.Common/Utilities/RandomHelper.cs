
namespace UnusualSuspect.Common.Utilities;

public static class RandomHelper
{
  public static List<int> GetUniqueRandomNumbers(int min, int max, int count)
  {
    if (count > max - min + 1)
      throw new ArgumentException("Count must be less than or equal to the range of numbers.");
    List<int> selectedNumbers = new List<int>();
    Random random = new Random();
    while (selectedNumbers.Count < count)
    {
      int randomNumber = random.Next(min, max + 1);
      if (!selectedNumbers.Contains(randomNumber))
        selectedNumbers.Add(randomNumber);
    }
    return selectedNumbers;
  }

  // Generate a unique file name
  public static string GetUniqueFileName(string fileName)
  {
    return Guid.NewGuid() + Path.GetExtension(Path.GetFileName(fileName));
  }
}