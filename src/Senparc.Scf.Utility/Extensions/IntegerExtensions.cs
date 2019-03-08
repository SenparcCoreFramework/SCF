namespace Senparc.Scf.Core.Extensions
{
    public static class IntegerExtensions
    {
        public static string ThousandChange(this int num)
        {
            if (num > 999)
            {
                num = num / 1000;
                return num.ToString() + "K";
            }
            else
            {
                return num.ToString();
            }
        }
    }
}
