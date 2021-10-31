using System.Text.RegularExpressions;

namespace AspNetCore.UnitTest.Service
{
    /// <summary>
    /// 金额正则表达式
    /// </summary>
    public class MoneyRegex
    {
        /// <summary>
        /// 验证金额字符串是否符合正则表达式
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool IsPrize(string amount)
        {
            // 小数点前最多只有5位，小数点后最多只有两位
            Regex moneyRegex = new Regex(@"^([1-9]\d{0,4}|0)(\.\d{1,2})?$");
            return moneyRegex.IsMatch(amount);
        }
    }
}
