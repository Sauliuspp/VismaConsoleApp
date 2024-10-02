namespace VismaConsoleApp
{
    public class ValidityChecker
    {
        public bool StringExists(List<string> viableStr, string targetStr)
        {
            return viableStr.Any(i => targetStr.Contains(i));
        }

        public bool IsNumberInRange(int minNum, int maxNum, int targetNum)
        {
            return targetNum >= minNum && targetNum <= maxNum;
        }
    }
}
